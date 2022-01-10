using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace IRISNDT.RoverControl.CommandProcessor
{
    public class CommandValidationExceptions : Exception
    {
        public HttpStatusCode HTTPStatusCode { get; set; }

        private IList<string> ErrorList { get; set; }

        public CommandValidationExceptions(IList<string> errorList, HttpStatusCode statusCode) : base()
        {
            this.ErrorList = errorList;
            this.HTTPStatusCode = statusCode;
        }

        public override string ToString()
        {
            string errors = string.Empty;
            foreach (var item in this.ErrorList)
            {
                errors += item.Trim(new char[] { '\0', ' ', '\n' });
            }
            return errors;
        }
    }
}
