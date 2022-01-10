using IRISNDT.RoverControl.CommandProcessor;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRISNDT.Models
{
    public class CommandFactory
    {

        public static ICommandProcess GetCommand(char instruction, Rover rover, Plateau plateau)
        {
            switch (instruction)
            {
                case 'M':
                    return new MovementCommand(plateau,rover, instruction);
                    
                default:
                    return new RotationCommand(rover, instruction);
            }
        }
    }
}
