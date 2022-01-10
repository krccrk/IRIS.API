using System;
using System.Text.Json.Serialization;

namespace IRISNDT.Models
{
    public class Position
    {
        public int X { get; set; }

        public int Y { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Directions Direction { get; set; }

    }

    public enum Directions
    {
        North = 'N',
        South = 'S',
        East = 'E',
        West = 'W'
    }

    public class StartPosition : Position
    {

    }

    public class FinishPosition:Position
    {

    }
}
