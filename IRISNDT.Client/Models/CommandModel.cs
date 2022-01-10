using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IRISNDT.Client.Models
{
    public class CommandModel
    {
        [Display(Name = "Rover Instructions")]
        public string RoverInstructions { get; set; }

        [Display(Name = "Rover Instructionstest")]
        public string test { get; set; } = string.Empty;
    }
}
