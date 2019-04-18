// ===========================================================================
using System;
//
namespace NSG.Library.Logger
{
    //
    /// <summary>
    /// Structure of the logging data.
    /// </summary>
    interface ILogData
    {
        //
        /// <summary>
        /// The id/key of the log record.
        /// </summary>
        long Id { get; set; }
        /// <summary>
        /// The date and time of the log (now).
        /// </summary>
        DateTime Date { get; set; }
        /// <summary>
        /// The application passed in the constructor.
        /// </summary>
        string Application { get; set; }
        /// <summary>
        /// The method that called log.
        /// </summary>
        string Method { get; set; }
        /// <summary>
        /// The log level, can be used to limit, depending on the implementation.
        /// </summary>
        byte LogLevel { get; set; }
        /// <summary>
        /// The human value of the above 'LogLevel'.
        /// </summary>
        string Level { get; set; }
        /// <summary>
        /// The user identity of the caller.
        /// </summary>
        string UserAccount { get; set; }
        /// <summary>
        /// Log message.
        /// </summary>
        string Message { get; set; }
        /// <summary>
        /// The exception, including the stack trace.
        /// </summary>
        string Exception { get; set; }
        //
        //
        /// <summary>
        /// Create a 'to string'.
        /// </summary>
        /// <returns>formatted string of the properties.</returns>
        string ToString();
    }
}
// ===========================================================================
