// ===========================================================================
using System;
using System.Text;
//
namespace NSG.Library.Logger
{
    /// <summary>
    /// Make the log a singleton and globally available.
    /// Early in the application 
    /// <example>
    /// <code>
    /// public void Configuration(IAppBuilder app)
    /// {
    ///     ...
    ///     // Globally configure logging and replace default List-Logger
    ///     // with SQL-Logger.
    ///     NSG.Library.Logger.Log.Logger = new WebSrv.Models.SQLLogger(
    ///         ApplicationDbContext.Create(),
    ///         WebSrv.Models.Constants.ApplicationLoggerName);
    /// }
    /// </code>
    /// </example>
    /// </summary>
    public static class Log
    {
        //
        static ILogger _logger = new ListLogger("Default");
        //
        /// <summary>
        /// The globally-shared logger.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public static ILogger Logger
        {
            get { return _logger; }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                _logger = value;
            }
        }
    }
}
// ===========================================================================
