using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Tracing;

namespace SupportPanda.Core
{
    public class Logger : ILogger
    {
        ILog log;

        public Logger()
        {
            log = LogManager.GetLogger("SupportPanda");
            XmlConfigurator.Configure();
        }

        public void Debug(string message)
        {
            System.Diagnostics.Debug.WriteLine(String.Format("DEBUG/{0} : {1}" ,DateTime.UtcNow,message));
            System.Diagnostics.Trace.WriteLine(String.Format("DEBUG/{0} : {1}" ,DateTime.UtcNow, message));

            log.Debug(message);
        }

        public void Error(string message)
        {
            System.Diagnostics.Debug.WriteLine(String.Format("ERROR/{0} : {1}", DateTime.UtcNow, message));
            System.Diagnostics.Trace.WriteLine(String.Format("ERROR/{0} : {1}", DateTime.UtcNow, message));

            log.Error(message);
        }

        public void Fatal(object message, Exception exception)
        {
            System.Diagnostics.Debug.WriteLine(String.Format("EXCEPTION/{0} : {1}", DateTime.UtcNow, message + "/" + exception.ToString()));
            System.Diagnostics.Trace.WriteLine(String.Format("EXCEPTION/{0} : {1}", DateTime.UtcNow, message + "/" + exception.ToString()));

            log.Fatal(message, exception);
        }

        public void Info(string message)
        {
            System.Diagnostics.Debug.WriteLine(String.Format("INFO/{0} : {1}", DateTime.UtcNow, message));
            System.Diagnostics.Trace.WriteLine(String.Format("INFO/{0} : {1}", DateTime.UtcNow, message));

            log.Info(message);
        }

        public void Warning(string message)
        {
            System.Diagnostics.Debug.WriteLine(String.Format("WARNING/{0} : {1}", DateTime.UtcNow, message));
            System.Diagnostics.Trace.WriteLine(String.Format("WARNING/{0} : {1}", DateTime.UtcNow, message));

            log.Warn(message);
        }
    }
}
