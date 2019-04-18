// ===========================================================================
//
// ===========================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
//
namespace NSG.Library.Logger
{
    /// <summary>
    /// The <see cref="NSG.Library.Logger"/> namespace contains classes
    /// for defining an interface for logging and a simple implementation
    /// of the interface.
    /// </summary>
    [System.Runtime.CompilerServices.CompilerGenerated]
    class NamespaceDoc
    {
    }
    //
    /// <summary>
    /// A simple implementation of ILogger
    /// </summary>
    public partial class ListLogger : ILogger
    {
        //
        private string _application = "";
        private List<LogData> _logs = null;
        private int _max = 10;
        //
        /// <summary>
        /// A simple implementation of ILogger using List of Log
        /// </summary>
        /// <param name="application">
        /// An application name for reporting the log.
        /// </param>
        /// <param name="max">
        /// The maximum number of logs.  Default is 100 and the minimum is 10.
        /// </param>
        public ListLogger(string application, int max = 100)
        {
            //
            this._application = application;
            if( max > 10 )
                this._max = max;
            this._logs = new List<LogData>();
            //
        }
        //
        /// <summary>
        /// Insert one row into Log
        /// (Calling log from C#)
        /// </summary>
        /// <param name="severity">
        /// Enum of 'LoggingLevel', must be between 0 and 4
        /// </param>
        /// <param name="user">The user identity</param>
        /// <param name="method">MethodBase.GetCurrentMethod()</param>
        /// <param name="message">the log message</param>
        /// <param name="exception">the exception, including the stack trace</param>
        /// <returns>the id</returns>
        public long Log(LoggingLevel severity, string user, MethodBase method, string message, Exception exception = null)
        {
            string _method = method.DeclaringType.FullName + "." + method.Name;
            string _exception = (exception == null ? "" : exception.ToString());
            return Log((byte)severity, user, _method, message, _exception);
        }
        //
        /// <summary>
        /// Insert one row into Log
        /// Calling log from the web api
        /// </summary>
        /// <param name="severity">
        ///  Range of byte: "0", "4"
        ///  See 'LoggingLevel' enum, must be between 0 and 4
        /// </param>
        /// <param name="user">The user identity</param>
        /// <param name="method">method calling log</param>
        /// <param name="message">the log message</param>
        /// <param name="exception">the exception, including the stack trace</param>
        /// <returns>the id</returns>
        public long Log(byte severity, string user, string method, string message, string exception = "")
        {
            long _ret = 0;
            LoggingLevel _level = (LoggingLevel)severity;
            try
            {
                LogData _log = new LogData();
                _log.Date = DateTime.Now;
                _log.Application = _application;
                _log.Method = (method.Length > 255 ? method.Substring(0, 255) : method);
                _log.LogLevel = severity;
                _log.Level = _level.GetName();  // extension method in Helpers
                _log.UserAccount = user;
                _log.Message = (message.Length > 4000 ? message.Substring(0, 4000) : message);
                _log.Exception = (exception.Length > 4000 ? exception.Substring(0, 4000) : exception);
                _log.Id = (_logs.Count == 0 ? 1 : _logs.Max(_l => _l.Id) + 1);
                _logs.Add(_log);
                _ret = _log.Id;
                if (_logs.Count > this._max)
                    _logs.Remove(_logs[0]);
            }
            catch (Exception _ex)
            {
                Console.WriteLine(_ex);
            }
            return _ret;
        }
        //
        /// <summary>
        /// Return a string listing.
        /// </summary>
        /// <param name="lastCount">
        ///  Count of last log records to return
        /// </param>
        /// <returns>List of string</returns>
        public List<string> ListString(int lastCount)
        {
            return _logs.OrderByDescending(_l => _l.Id).Take(lastCount)
                .Select(_r => _r.ToString()).ToList();
        }
        //
    }
}
// ===========================================================================
