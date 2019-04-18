// ===========================================================================
//
// ===========================================================================
using System;
using System.Collections.Generic;
using System.Reflection;
//
namespace NSG.Library.Logger
{
    //
    /// <summary>
    /// Enum of various logging levels
    /// Note:
    ///  [Range(typeof(byte), "0", "4", ErrorMessage = "'LogLevel' must be between 0 and 4")]
    /// </summary>
    public enum LoggingLevel {
        /// <summary>
        /// if one would like to implement some sort of change auditing
        /// </summary>
        Audit = 0,
        /// <summary>
        /// For exceptions and other errors
        /// </summary>
        Error = 1,
        /// <summary>
        /// Warning level of logs
        /// </summary>
        Warning = 2,
        /// <summary>
        /// Info level of logs
        /// </summary>
        Info = 3,
        /// <summary>
        /// Debug level of logs
        /// </summary>
        Debug = 4,
        /// <summary>
        /// Verbose level of logs
        /// </summary>
        Verbose = 5
    };
    //
    /// <summary>
    /// Interface for Logger
    /// </summary>
    public interface ILogger
    {
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
        long Log(byte severity, string user, string method, string message, string exception = "");
        //
        /// <summary>
        /// Insert one row into Log
        /// (Calling log from C#)
        /// </summary>
        /// <param name="severity">
        /// Enum of 'LoggingLevel', must be between 0 and 4
        /// </param>
        /// <param name="user">The user identity</param>
        /// <param name="method">MethodBase</param>
        /// <param name="message">the log message</param>
        /// <param name="exception">the exception, including the stack trace</param>
        /// <returns>the id</returns>
        long Log(LoggingLevel severity, string user, MethodBase method, string message, Exception exception = null);
        //
        /// <summary>
        /// Return a string listing.
        /// </summary>
        /// <param name="lastCount">Count of last log records to return</param>
        /// <returns>List of string</returns>
        List<string> ListString(int lastCount);
        //
    }
}
// ===========================================================================
