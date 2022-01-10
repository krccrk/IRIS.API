using IRISNDT.Common.Models;
using IRISNDT.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRISNDT.RoverControl.CommandProcessor
{
    public static class ExtensionMethods
    {
        public static RoverPositioning InitializeWithRoverNavigation(this RoverPositioning roverPositioning, RoverNavigation roverNavigation)
        {
            var roverPositioningRetVal = roverPositioning;
            roverPositioningRetVal.UUID = roverNavigation.UUID;
            roverPositioningRetVal.Plateau = new Plateau() { GridXColumn = roverNavigation.PlateauXaxis, GridYColum = roverNavigation.PlateauYaxis };
            roverPositioningRetVal.Rovers = new List<Rover>();
            roverPositioningRetVal.DateTime = DateTime.UtcNow;
            return roverPositioningRetVal;

        }

        public static void Rotate(this Position poistion, char instruction)
        {
            Directions result = poistion.Direction;
            switch (poistion.Direction)
            {
                case Directions.North:

                    if (instruction == 'L')
                    {
                        result = Directions.West;
                        break;
                    }
                        result = result = Directions.East;
                    break;
                case Directions.South:

                    if (instruction == 'L')
                    {
                        result = Directions.East;
                        break;
                    }
                    result = result = Directions.West;
                    break;
                case Directions.East:

                    if (instruction == 'L')
                    {
                        result = Directions.North;
                        break;
                    }
                    result = result = Directions.South;
                    break;
                case Directions.West:

                    if (instruction == 'L')
                    {
                        result = Directions.South;
                        break;
                    }
                    result = result = Directions.North;
                    result = result = Directions.North;
                    break;
            }
            poistion.Direction = result;


        }
    }
}
