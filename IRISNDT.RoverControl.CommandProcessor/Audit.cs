using IRISNDT.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Linq;
using IRISNDT.Common.Models;

namespace IRISNDT.RoverControl.CommandProcessor
{
    public static class RoverCommandAudit
    {

        public static RoverPositioning GetAuditData(string UUID)
        {
            RoverPositioning roverPositioning = null;
            if (File.Exists($"Audit/{UUID}.txt"))
            {
                var fileStream = File.ReadAllText($"Audit/{UUID}.txt");
                roverPositioning = JsonSerializer.Deserialize<RoverPositioning>(fileStream);
            }
            return roverPositioning;
        }

        public static bool Audit(string UUID, RoverPositioning roverPositioning)
        {
            string content = JsonSerializer.Serialize<RoverPositioning>(roverPositioning);
            File.WriteAllText($"Audit/{UUID}.txt", content);
            return true;
        }

        public static IList<CommandHistory> GetTransactionHistory()
        {
           var files = new DirectoryInfo("Audit").GetFiles().OrderByDescending(o => o.CreationTime).Take(10);
            IList<CommandHistory> commandHistiry = new List<CommandHistory>();
            foreach (var item in files)
            {
                var fileStream = item.OpenText().ReadToEnd();
                RoverPositioning roverPositioning = JsonSerializer.Deserialize<RoverPositioning>(fileStream);
                CommandHistory commandHistory = new CommandHistory();
                commandHistory.sessionId = roverPositioning.UUID;
                commandHistory.initiatedDtm = roverPositioning.DateTime;
                commandHistory.noOfRoversCommand = roverPositioning.Rovers.Count;
                commandHistiry.Add(commandHistory);
            }
            return commandHistiry;

        }

        public static void GetSessionDetails(string sessionId)
        {

        }

    }
}
