using IRISNDT.Client.Models;
using IRISNDT.Common.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IRISNDT.Client.Controllers
{
    public class CommandCentre : Controller
    {
        private readonly IHttpClientFactory clientFactory;

        public CommandCentre(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
        }

        public IActionResult Index()
        {
            return View(new CommandModel() );
        }

        public async Task<IActionResult> AuditHistory()
        {
            var client = this.clientFactory.CreateClient("API");
            var response = await client.GetAsync("Navigation/GetHistory");
            string responseJson = await response.Content.ReadAsStringAsync();
            var CommandHistory = JsonSerializer.Deserialize<List<CommandHistory>>(responseJson);
            return View("Audit", CommandHistory);
        }

        public async Task<IActionResult> GetSessionDetails(string sessionId)
        {
            var client = this.clientFactory.CreateClient("API");
            var response = await client.GetAsync("Navigation/GetSessionDetail?sessionId="+sessionId);
            string responseJson = await response.Content.ReadAsStringAsync();
            return new JsonResult(responseJson);
        }

        [HttpPost]
        public async Task<IActionResult> ExecuteCommand(CommandModel model)
        {
            RoverNavigation roverNavigation = new RoverNavigation();
            List<String> errorLst = new List<string>();
            roverNavigation.UUID = Guid.NewGuid().ToString();
            roverNavigation.roverNavigationCommands = new List<NavigationCommands>();
            int lineNumber = 1;
            NavigationCommands currentCommad = null;
            if(model.RoverInstructions == null || model.RoverInstructions.Trim().Length==0)
            {
                errorLst.Add("Invalid Instructions");
                return BadRequest(JsonSerializer.Serialize(errorLst));
            }
            foreach (var item in model.RoverInstructions.Split('\n'))
            {
                string command = item.Replace('\r', ' ');
                if (lineNumber == 1 && command.Trim().Length>0 && command.Trim().Split(' ').Length == 2)
                {
                    this.ExtractPlateauAxis(roverNavigation, command, errorLst);
                }
                else
                {
                    if(lineNumber % 2 ==0)
                    {
                        currentCommad = ExtractRoverPosition(roverNavigation, command, errorLst);
                        roverNavigation.roverNavigationCommands.Add(currentCommad);
                    }
                    else
                    {
                        ExtractRoverInstruction(currentCommad, command, errorLst);
                    }
                }
                lineNumber += 1;
            }
            if(errorLst.Count > 0) return BadRequest(JsonSerializer.Serialize(errorLst));
            string jsonString = JsonSerializer.Serialize(roverNavigation);
            var payload = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var client = this.clientFactory.CreateClient("API");
            var response = await client.PostAsync("/Navigation/NavigateRover", payload);
            string responseJson = await response.Content.ReadAsStringAsync();
            model.test = responseJson;
            return new JsonResult(responseJson);
        }

        private NavigationCommands ExtractRoverPosition(RoverNavigation roverNavigation, string command, List<string> error)
        {
            string[] commands = command.Trim().Split(' ');
            NavigationCommands navigationCommand = new NavigationCommands();
            if (command.Trim().Length > 0 && commands.Length == 3)
            {
                int xaxis = 0;
                if (int.TryParse(commands[0], out xaxis))
                {
                    navigationCommand.RoverStartXaxis = xaxis;
                }
                else
                {
                    error.Add("Rover X axis not valid.");
                }

                if (int.TryParse(commands[1], out xaxis))
                {
                    navigationCommand.RoverStartYaxis = xaxis;
                }
                else
                {
                    error.Add("Rover Y axis not valid.");
                }
                switch (commands[2].ToUpper())
                {
                    case "N":
                        navigationCommand.Direction = "North";
                        break;
                    case "S":
                        navigationCommand.Direction = "South";
                        break;
                    case "E":
                        navigationCommand.Direction = "East";
                        break;
                    case "W":
                        navigationCommand.Direction = "West";
                        break;
                    default:
                        error.Add("Rover direction is not valid. ");
                        break;
                }

                return navigationCommand;
            }
            else
            {
                //throw error
                return null;
            }
        }
        private void ExtractRoverInstruction(NavigationCommands navigationCommand, string command, List<string> error)
        {
            string cmd = command.ToUpper().Replace('L', ' ').Replace('R', ' ').Replace('M', ' ').Trim();
            if (cmd.Length == 0)
            {
                navigationCommand.Instruction = command;
            }
            else
            {
                error.Add("Invalid Navigation Instruction.");
            }
        }

        private void ExtractPlateauAxis(RoverNavigation roverNavigation, string command, List<string> error)
        {
            int count = 1;
            string[] plateauPosition = command.Trim().Split(' ');
            int xaxis = 0;
            if (int.TryParse(plateauPosition[0], out xaxis))
            {
                roverNavigation.PlateauXaxis = xaxis;
            }
            else
            {
                error.Add("Plateau X axis is not valid.");
            }

            if (int.TryParse(plateauPosition[1], out xaxis))
            {
                roverNavigation.PlateauYaxis = xaxis;
            }
            else
            {
                error.Add("Plateau Y axis is not valid.");
            }
           
        }
    }
}
