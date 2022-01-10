using IRISNDT.Common.Models;
using IRISNDT.Models;
using IRISNDT.RoverControl.CommandProcessor;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRISNDT.API.RoverControl.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NavigationController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<NavigationController> _logger;

        public CommandProcessor CommandProcessor { get; }

        public NavigationController(ILogger<NavigationController> logger, CommandProcessor commandProcessor)
        {
            _logger = logger;
            CommandProcessor = commandProcessor;
        }

        /// <summary>
        /// Generate navigation Routes and track the changes
        /// </summary>
        /// <param name="roverNavigation"></param>
        /// <remarks>
        /// A sample request :
        /// {
        ///             "UUID":"12229",
        ///             "PlateauXaxis":5 ,
        ///             "PlateauYaxis":5, 
        ///             "roverNavigationCommands":[
        ///                 {
        ///                     "RoverStartXaxis":1,
        ///                     "RoverStartYaxis":2,
        ///                     "Direction":"North",
        ///                     "Instruction":"LMLMLMLMM"
        ///                 },
        ///                 {
        ///                     "RoverStartXaxis":3,
        ///                     "RoverStartYaxis":3,
        ///                     "Direction":"East",
        ///                     "Instruction":"MMRMMRMRRM"          
        ///                 }
        ///             ]
        /// 
        ///
        ///     }
        /// </remarks>
        /// <returns>
        /// </returns>
        [HttpPost("NavigateRover")]
        [ProducesResponseType(typeof(RoverNavigation), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult NavigateRover(RoverNavigation roverNavigation)
        {
            RoverPositioning roverAuditData = null;
            try
            {
                roverAuditData = this.CommandProcessor.ProcessNavigation(roverNavigation);
            }
            catch (CommandValidationExceptions ex)
            {
                return BadRequest(ex.ToString());
            }
            return Ok(new JsonResult(roverAuditData));

        }


        [HttpGet("GetHistory")]
        [ProducesResponseType(typeof(JsonResult),StatusCodes.Status200OK)]
        public IActionResult GetHistory()
        {
            return new JsonResult(RoverCommandAudit.GetTransactionHistory());

        }

        [HttpGet("GetSessionDetail")]
        public IActionResult GetSessionDetail(string sessionId)
        {
            //var sessionId = HttpContext.Request.Query["sessionId"];
            var retVal = RoverCommandAudit.GetAuditData(sessionId);
            return Ok(new JsonResult(retVal));
        }

    }
}
