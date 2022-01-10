using System;
using System.Collections.Generic;

namespace IRISNDT.Common.Models
{
 
    public class RoverNavigation
    {
        public string UUID { get; set; }

        public int PlateauXaxis { get; set; }

        public int PlateauYaxis { get; set; }

        public IList<NavigationCommands> roverNavigationCommands { get; set; }

    }

    public class NavigationCommands
    {
        public int RoverStartXaxis { get; set; }

        public int RoverStartYaxis { get; set; }

        public string Instruction { get; set; }

        public string Direction { get; set; }
    }

    
}
