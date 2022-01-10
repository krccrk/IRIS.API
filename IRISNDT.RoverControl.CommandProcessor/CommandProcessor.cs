using IRISNDT.Common.Models;
using IRISNDT.Models;
using System;
using System.Collections.Generic;
using System.Net;

namespace IRISNDT.RoverControl.CommandProcessor
{
    public class CommandProcessor
    {

        public RoverPositioning ProcessNavigation(RoverNavigation roverNavigation)
        {
            this.Validate(roverNavigation);
            RoverPositioning roverAuditData = RoverCommandAudit.GetAuditData(roverNavigation.UUID);
            if(roverAuditData == null)
            {
                roverAuditData = new RoverPositioning().InitializeWithRoverNavigation(roverNavigation);
            }
            int id = 1;
            foreach (var item in roverNavigation.roverNavigationCommands)
            {
                Rover rover = new Rover();
                rover.ID = id;
                id += 1;
                rover.Color = $"#{Guid.NewGuid().ToString().Substring(0,6)}";
                rover.Name = $"Rover_{rover.ID}-({item.RoverStartXaxis},{item.RoverStartYaxis}) :{item.Direction}";
                roverAuditData.Rovers.Add(rover);
                rover.StartPosition = new StartPosition()
                {
                    X = item.RoverStartXaxis,
                    Y = item.RoverStartYaxis,
                    Direction = Enum.Parse<Directions>(item.Direction)
                };
                rover.CurrentPosition = new StartPosition()
                {
                    X = item.RoverStartXaxis,
                    Y = item.RoverStartYaxis,
                    Direction = Enum.Parse<Directions>(item.Direction)
                };
                rover.Instructions = item.Instruction;
                this.NavigateRover(roverAuditData.Plateau, rover);
            }
            
            RoverCommandAudit.Audit(roverNavigation.UUID, roverAuditData);
            return roverAuditData;

        }

        private void NavigateRover(Plateau plateau, Rover rover)
        {
            foreach (var item in rover.Instructions)
            {
                ICommandProcess cmd  = CommandFactory.GetCommand(item, rover, plateau);
                cmd.Execute();
            }
            rover.Audit.Add($"The rover finally reached {rover.CurrentPosition.X} , {rover.CurrentPosition.Y} and facing {rover.CurrentPosition.Direction}.");

            
        }

        private void Validate(RoverNavigation roverNavigation)
        {
           
            IList<string> error = new List<string>();
            int count = 0;
            foreach (var item in roverNavigation.roverNavigationCommands)
            {
                count+= 1;
                if (item.RoverStartXaxis > roverNavigation.PlateauXaxis || item.RoverStartXaxis < 0)
                {

                    error.Add($"Rover No {count} :  X axis is not alligned with the plateau.");
                }

                if (item.RoverStartYaxis > roverNavigation.PlateauYaxis || item.RoverStartYaxis < 0)
                {
                    error.Add(" Rover No {count} :  Y axis is not alligned with the plateau.");
                }

                if (item.Instruction.ToUpper().Replace('L', ' ').Replace('R', ' ').Replace('M', ' ').Trim().Length > 0)
                {
                    error.Add(" Rover No {count} :  instruction is not valid.");
                }

                if (error.Count > 0) throw new CommandValidationExceptions(error, HttpStatusCode.BadRequest);
                
            }
            
           
        }

        
    }
}
