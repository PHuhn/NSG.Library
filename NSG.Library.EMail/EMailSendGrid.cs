//
using System;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;
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
        /// llamda method to move MailAddress email address to a 
        /// new SendGrid EmailAddress address.
        /// </summary>
        /// <param name="address">a MailAddress email address</param>
        /// <returns>a new SendGrid EmailAddress address</returns>
        private EmailAddress ConvertToEmailAddress(MailAddress address) => new EmailAddress(address.Address, address.DisplayName);
        //
        /// <summary>
        /// llamda method to move SendGrid EmailAddress email address to a 
        /// new MS SMTP MailAddress address.
        /// </summary>
        /// <param name="address">a SendGrid EmailAddress email address</param>
        /// <returns>a new MS SMTP MailAddress address</returns>
        private MailAddress ConvertToMailAddress(EmailAddress address) => new MailAddress(address.Email, address.Name);
        //
        /// <summary>
        /// Translate from MailMessage to SendGridMessage.
        /// </summary>
        /// <returns>a new SendGrid SendGridMessage message found in SendGrid.Helpers.Mail</returns>
        public SendGridMessage ToSendGrid( )
        {
            //
            var _sgm = new SendGridMessage();
            //
            _sgm.From = ConvertToEmailAddress( _mailMessage.From );
            if (_mailMessage.To.Count > 0 )
                _sgm.AddTos(_mailMessage.To.Select(ConvertToEmailAddress).ToList());
            if (_mailMessage.CC.Count > 0)
                _sgm.AddCcs(_mailMessage.CC.Select(ConvertToEmailAddress).ToList());
            if (_mailMessage.Bcc.Count > 0)
                _sgm.AddBccs(_mailMessage.Bcc.Select(ConvertToEmailAddress).ToList());
            _sgm.SetSubject(_mailMessage.Subject);
            if ( _mailMessage.IsBodyHtml )
                _sgm.HtmlContent = _mailMessage.Body;
            else
                _sgm.PlainTextContent = _mailMessage.Body;
            //
            if ( _mailMessage.Attachments.Count > 0 )
            {
                foreach (var _attachment in _mailMessage.Attachments)
                {
                    string _buffer;
                    using (var _ms = new MemoryStream())
                    {
                        _attachment.ContentStream.CopyTo(_ms);
                        _buffer = Convert.ToBase64String(_ms.ToArray());
                    }
                    // Helpers.Helpers.ByteArrayToFile(Convert.FromBase64String(_buffer), "O" + _attachment.Name);
                    //
                    string _fileName = Path.GetFileName( _attachment.Name );
                    _sgm.AddAttachment(_fileName, _buffer);
                }
            }
            //
            return _sgm;
        }
        //
        /// <summary>
        /// Convert a SendGridMessage mail message, and load it into a new message.
        /// <example>
        /// This sample shows how to call the NewMailMessage method.
        /// <code>
        ///  // translate the message from json string of SendGrid message type
        ///  JavaScriptSerializer j = new JavaScriptSerializer();
        ///  SendGridMessage _sgm = (SendGridMessage)j.Deserialize(_jsonString, typeof(SendGridMessage));
        ///  IEMail _email = new EMail(Log.Logger).NewMailMessage(_sgm).Send();
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="sgm">a SendGridMessage mail message from SendGrid.Helpers.Mail</param>
        /// <returns>itself, IEMail to allow fluent design.</returns>
        public IEMail NewMailMessage( SendGridMessage sgm )
        {
            //
            var _mailMessage = new MailMessage();
            //
            this.From( ConvertToMailAddress(sgm.From) );
            foreach( var per in sgm.Personalizations )
            {
                foreach ( var to in per.Tos )
                    this.To( ConvertToMailAddress( to ) );
                if( per.Ccs != null )
                    foreach ( var cc in per.Ccs )
                        this.CC(ConvertToMailAddress(cc));
                if (per.Bccs != null)
                    foreach ( var bcc in per.Bccs )
                        this.BCC( ConvertToMailAddress( bcc ) );
            }
            this.Subject( sgm.Subject );
            if (string.IsNullOrEmpty(_mailMessage.Subject) && sgm.Personalizations.Count > 0)
                this.Subject(sgm.Personalizations[0].Subject);
            if (sgm.Contents != null && sgm.Contents.Count > 0)
            {
                this.Body(sgm.Contents[0].Value);
                _mailMessage.IsBodyHtml = (sgm.Contents[0].Type.Contains("html") ? true : false);
            }
            else
            {
                if ( !string.IsNullOrEmpty(sgm.PlainTextContent) )
                {
                    this.Body( sgm.PlainTextContent );
                    _mailMessage.IsBodyHtml = false;
                }
                else
                {
                    this.Body( sgm.HtmlContent );
                    _mailMessage.IsBodyHtml = true;
                }
            }
            //
            if (sgm.Attachments != null && sgm.Attachments.Count > 0)
            {
                foreach (var _attachment in sgm.Attachments)
                {
                    this.Attachment(Convert.FromBase64String(_attachment.Content), _attachment.Filename, _attachment.Type);
                }
            }
            //
            return this;
        }
        //
        /// <summary>
        /// Send the email and any attachments, via the SendGrid async task.
        /// </summary>
        /// <param name="apiKey">The SendGrid api key</param>
        /// <returns>itself, IEMail to allow fluent design.</returns>
        public IEMail SendSendGridAsync( string apiKey )
        {
            //
            SendList(); // also stuff all emails into a list
            //
            var _client = new SendGridClient(apiKey);
            Task.Run(async () =>
            {
                try
                {
                    SendGridMessage _sgmm = ToSendGrid( );
                    Response _results = await _client.SendEmailAsync(_sgmm);
                    if( _results.StatusCode != System.Net.HttpStatusCode.Accepted  )
                    {
                        throw (new ApplicationException("StatusCode: "
                            + _results.StatusCode.ToString()));
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
            }).GetAwaiter().GetResult();
            return this;
        }
    }
}
//
