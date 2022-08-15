using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wsTeleavenceService.Models
{
    public class Logger
    {
        public static readonly ILog log = LogManager.GetLogger("RollingFileLog");
    }
}