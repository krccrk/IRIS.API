using System;
using System.Collections.Generic;
using System.Text;

namespace IRISNDT.Models
{
    public class RoverPositioning
    {
        public string UUID { get; set; }

        public Plateau Plateau { get; set; }

        public DateTime DateTime { get; set; }

        public IList<Rover> Rovers { get; set; }
    }

    public class Plateau
    {
        public int GridXColumn { get; set; }

        public int GridYColum { get; set; }
    }

    public class Rover
    {
        public string Name { get; set; }

        public int ID { get; set; }

        public string Color { get; set; }

        public Position StartPosition { get; set; }

        public string Instructions { get; set; }

        public List<String> Audit { get; set; } = new List<string>();

        public IList<Position> NavigatedPostions { get; set; } = new List<Position>();

        public Position CurrentPosition { get; set; }
    }
}
