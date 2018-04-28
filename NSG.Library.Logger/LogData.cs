// ===========================================================================
using System;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//
namespace NSG.Library.Logger
{
    //
    /// <summary>
    /// The Log class.
    /// </summary>
    public partial class LogData : ILogData
    {
        //
        /// <summary>
        /// The id/key of the log record.
        /// </summary>
        [Key, Column(Order = 1)]
        [Required(ErrorMessage = "Id is required.")]
        public long Id { get; set; }
        /// <summary>
        /// The date and time of the log (now).
        /// </summary>
        [Required(ErrorMessage = "Date is required.")]
        public DateTime Date { get; set; }
        /// <summary>
        /// The application passed in the constructor.
        /// </summary>
        [Required(ErrorMessage = "Application is required."), MaxLength(30, ErrorMessage = "'Application' must be 30 or less characters.")]
        public string Application { get; set; }
        /// <summary>
        /// The method that called log.
        /// </summary>
        [Required(ErrorMessage = "Method is required."), MaxLength(255, ErrorMessage = "'Method' must be 255 or less characters.")]
        public string Method { get; set; }
        /// <summary>
        /// The log level, can be used to limit, depending on the implementation.
        /// </summary>
        [Required(ErrorMessage = "LogLevel is required.")]
        [Range(typeof(byte), "0", "4", ErrorMessage = "'LogLevel' must be between 0 and 4")]
        public byte LogLevel { get; set; }
        /// <summary>
        /// The human value of the above 'LogLevel'.
        /// </summary>
        [Required(ErrorMessage = "Level is required."), MaxLength(8, ErrorMessage = "'Level' must be 8 or less characters.")]
        public string Level { get; set; }
        /// <summary>
        /// The user identity of the caller.
        /// </summary>
        [Required(ErrorMessage = "UserAccount is required."), MaxLength(255, ErrorMessage = "'UserAccount' must be 255 or less characters.")]
        public string UserAccount { get; set; }
        /// <summary>
        /// Log message.
        /// </summary>
        [Required(ErrorMessage = "Message is required."), MaxLength(4000, ErrorMessage = "'Message' must be 4000 or less characters.")]
        public string Message { get; set; }
        /// <summary>
        /// The exception, including the stack trace.
        /// </summary>
        [MaxLength(4000, ErrorMessage = "'Exception' must be 4000 or less characters.")]
        public string Exception { get; set; }
        //
        /// <summary>
        /// Create a 'to string'.
        /// </summary>
        public override string ToString()
        {
            //
            StringBuilder _return = new StringBuilder("record:[");
            _return.AppendFormat("Id: {0}, ", Id.ToString());
            _return.AppendFormat("Date: {0}, ", Date.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            _return.AppendFormat("Application: {0}, ", Application);
            _return.AppendFormat("Method: {0}, ", Method);
            _return.AppendFormat("LogLevel: {0}, ", LogLevel);
            _return.AppendFormat("Level: {0}, ", Level);
            _return.AppendFormat("UserAccount: {0}, ", UserAccount);
            _return.AppendFormat("Message: {0}, ", Message);
            _return.AppendFormat("Exception: {0}]", Exception);
            //
            return _return.ToString();
        }
    }
}
// ===========================================================================
