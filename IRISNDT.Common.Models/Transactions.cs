using System;
using System.Collections.Generic;
using System.Text;

namespace IRISNDT.Common.Models
{
    public class CommandHistory
    {
        public string sessionId { get; set; }

        public DateTime initiatedDtm { get; set; }

        public int noOfRoversCommand { get; set; }
    }
}
