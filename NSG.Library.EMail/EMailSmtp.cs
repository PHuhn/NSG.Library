// ---------------------------------------------------------------------------
//
using System;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
//
using NSG.Library.Logger;
//
namespace NSG.Library.EMail
{
    // 
    /// <summary>
    /// A fluent interface for smtp mail
    /// </summary>
    /// <remarks>Example: new EMail( from, to, "Subject", "Body").Send()</remarks>
    public partial class EMail : IEMail
    {
        //
        /// <summary>
        /// Send the email and any attachments, via the SMTP.
        /// </summary>
        /// <returns>itself, IEMail to allow fluent design.</returns>
        public IEMail SendSmtp()
        {
            //
            SendList(); // also stuff all emails into a list
            //
            // Use <system.net><mailSettings> to configure SMTP, see top comments...
            var _client = new SmtpClient();
            try
            {
                _client.Send(_mailMessage);
            }
            catch (Exception _ex)
            {
                System.Diagnostics.Debug.WriteLine(_ex.Message);
                var _exp = _ex.GetBaseException();
                Logging.Log(LoggingLevel.Error, "unknown", MethodBase.GetCurrentMethod(),
                    _exp.Message + ", Email: " + EmailToString(), _ex);
            }
            finally
            {
                _client.Dispose();
                if (_mailMessage != null)
                {
                    CleanUpAttachments();
                }
                //
                this.CleanUp();
            }
            // 
            return this;
        }
        // 
        /// <summary>
        /// Send the email and any attachments, via the SMTP async task.
        /// After sending clean-up the attachments and the streams and 
        /// finally the mail-message.
        /// </summary>
        /// <returns>itself, IEMail to allow fluent design.</returns>
        public IEMail SendSmtpAsync()
        {
            //
            SendList(); // also stuff all emails into a list
            //
            EventWaitHandle _handle = new EventWaitHandle(false, EventResetMode.AutoReset);
            var _dt = DateTime.Now;
            string _token = string.Format("Token:{0}:{1}:{2}",
                _dt.Minute, _dt.Second, _dt.Millisecond);
            try
            {
                using (var _client = new SmtpClient())
                {
                    _client.SendCompleted += (s, e) => {
                        Console.WriteLine("Token: {0}, Cancel: {1}, Error: {2}",
                            (string)e.UserState, e.Cancelled, e.Error);
                        _handle.Set();
                    };
                    _client.SendAsync(_mailMessage, _token);
                    _handle.WaitOne();
                }
            }
            catch (Exception _ex)
            {
                var _exp = _ex.GetBaseException();
                Logging.Log(LoggingLevel.Error, "unknown", MethodBase.GetCurrentMethod(),
                    _exp.Message + ", Email: " + EmailToString(), _ex);
            }
            finally
            {
                if (_mailMessage != null)
                {
                    CleanUpAttachments();
                }
                //
                this.CleanUp();
            }
            //
            return this;
        }
        //
    }
}
