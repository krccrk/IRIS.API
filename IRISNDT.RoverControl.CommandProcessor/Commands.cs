using IRISNDT.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRISNDT.RoverControl.CommandProcessor
{
    public interface ICommandProcess
    {
        void Execute();

    }
    public class MovementCommand : ICommandProcess
    {
        public Rover rover { get; set; }
        private char instrauction;
        private Plateau plateau { get; set; }
        public MovementCommand(Plateau plateau, Rover rover, char instrauction)
        {
            this.rover = rover;
            this.plateau = plateau;
            this.instrauction = instrauction;
        }

        public void Execute()
        {
            Position newPosition = new Position();
            switch (this.rover.CurrentPosition.Direction)
            {
                case Directions.North:
                    newPosition.X = this.rover.CurrentPosition.X;
                    newPosition.Y = this.rover.CurrentPosition.Y + 1;
                    newPosition.Direction = this.rover.CurrentPosition.Direction;
                    break;
                case Directions.South:
                    newPosition.X = this.rover.CurrentPosition.X;
                    newPosition.Y = this.rover.CurrentPosition.Y - 1;
                    newPosition.Direction = this.rover.CurrentPosition.Direction;
                    break;
                case Directions.East:
                    newPosition.X = this.rover.CurrentPosition.X+1;
                    newPosition.Y = this.rover.CurrentPosition.Y ;
                    newPosition.Direction = this.rover.CurrentPosition.Direction;
                    break;
                case Directions.West:
                    newPosition.X = this.rover.CurrentPosition.X-1;
                    newPosition.Y = this.rover.CurrentPosition.Y;
                    newPosition.Direction = this.rover.CurrentPosition.Direction;
                    break;
                default:
                    break;
            }
            var error = this.Validate(newPosition, this.plateau);
            if (error.Count > 0)
            {
                throw new CommandValidationExceptions(error, System.Net.HttpStatusCode.BadRequest);
            }
            else
            {
                this.rover.Audit.Add($"Rower is moved from {this.rover.CurrentPosition.X} ,  {this.rover.CurrentPosition.Y} to {newPosition.X}, {newPosition.Y}.");
                this.rover.CurrentPosition = newPosition;
                this.rover.NavigatedPostions.Add(newPosition);
            }
            
        }

        private IList<string> Validate(Position position, Plateau plateau)
        {
            IList<string> retVal = new  List<String>();
            if (position.X > plateau.GridXColumn || position.X < 0)
            {
                var error = $" Rower is not permitted to cross the boundry X axis  {plateau.GridXColumn}." +
                    $"If moved the boundry will be {position.X} ";
                this.rover.Audit.Add(error);
                retVal.Add(error);
            }

            if (position.Y > plateau.GridYColum || position.Y <0)
            {
                var error = $"Rower is not permitted to cross the boundry Y axis - {plateau.GridYColum}." +
                    $"If moved the boundry will be {position.Y}."; 
                this.rover.Audit.Add(error);
                retVal.Add(error);
            }
            return retVal;

        }
    }


    public class RotationCommand : ICommandProcess
    {
        public Rover rover { get; set; }
        private char instrauction;
        public RotationCommand(Rover rover, char instrauction)
        {
            this.rover = rover;
            this.instrauction = instrauction;
        }

        public void Execute()
        {
            this.PositionRover();
        }

        private void PositionRover()
        {
            this.rover.Audit.Add($"The rover {rover.Name} is rotated to {this.instrauction} from {this.rover.CurrentPosition.Direction} ");
            this.rover.CurrentPosition.Rotate(this.instrauction);

        }
    }
}
