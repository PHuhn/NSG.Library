// ---------------------------------------------------------------------------
//   <system.net>
//     <mailSettings>
//       <smtp from="test@test.test">
//         <network host="smtphost" port="25" username="user" password="password" defaultCredentials="true" />
//       </smtp>
//     </mailSettings>
//   </system.net>
// https://stackoverflow.com/questions/10339877/asp-net-webapi-how-to-perform-a-multipart-post-with-file-upload-using-webapi-ht
// Attachment	Specifies that the attachment is to be displayed as a file attached to the e-mail message.
// Inline	Specifies that the attachment is to be displayed as part of the e-mail message body.
//
// Author: Phil Huhn
// Created Date: 2018/02/13
// ---------------------------------------------------------------------------
// Modified By:
// Modification Date:
// Purpose of Modification:
// ---------------------------------------------------------------------------
//
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Reflection;
//
using NSG.Library.Logger;
using NSG.Library.Helpers;
//
namespace NSG.Library.EMail
{
    /// <summary>
    /// The <see cref="NSG.Library.EMail"/> namespace contains a class
    /// for emailing via SMTP or SendGrid or Mailgun (Mailgun is untested).
    /// This requires an ILogger.  NSG.Library.Logger has the default.
    /// Also needs NSG.Library.Helpers for reading the appSettings.
    /// Finally, it needs the following appSettings (see the tests):
    ///  <list type="bullet">
    ///   <item><description>Email:Enabled</description></item>
    ///   <item><description>Email:TestEmailName</description></item>
    ///   <item><description>Email:UseService</description></item>
    ///   <item><description>Email:ApiKey</description></item>
    ///   <item><description>Email:MailgunDomain</description></item>
    ///  </list>
    /// </summary>
    [System.Runtime.CompilerServices.CompilerGenerated]
    class NamespaceDoc
    {
    }
    // 
    /// <summary>
    /// A fluent interface for smtp mail message
    /// </summary>
    /// <remarks>Example: new EMail( from, to, "Subject", "Body").Send()</remarks>
    public partial class EMail : IEMail
    {
        //
        /// <summary>
        /// This is an implementation of ILogging.  Errors are loggged
        /// to the ILogger during the sending of email.
        /// </summary>
        public ILogger Logging = null;
        //
        //  Smtp mail message
        //
        private static MailMessage _mailMessage = null;
        private static List<string> _emails = null;
        private static int _max = 10;
        //
        // 4 Constructors
        //  EMail(fromAddress, toAddress, subject, message)
        //  EMail(logging, fromAddress, toAddress, subject, message)
        //  EMail( )
        //  EMail(logging)
        //
        #region "Constructors"
        //
        /// <summary>
        /// Instantiate the SMTP MailMessage class using the four parameters.
        /// <example> 
        /// This sample shows how to call this constructor.
        /// <code>
        ///  IEMail _mail = new EMail("testfrom@example.com", "testto@example.com", "Sending email is Fun", "This is a test email.").Send();
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="fromAddress">From email address</param>
        /// <param name="toAddress">To email address</param>
        /// <param name="subject">Email subject line</param>
        /// <param name="message">Email body</param>
        /// <returns>IEMail to allow fluent design.</returns>
        public EMail(string fromAddress, string toAddress, string subject, string message)
        {
            if (Logging == null)
                this.Logging = new ListLogger("EMail");
            this.NewMailMessage(fromAddress, toAddress, subject, message);
        }
        /// <summary>
        /// Instantiate the SMTP MailMessage class using the four parameters.
        /// <example> 
        /// This sample shows how to call this constructor.
        /// <code>
        ///  IEMail _mail = new EMail(Log.Logger, "testfrom@example.com", "testto@example.com", "Sending email is Fun", "This is a test email.").Send();
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="logging">This injects an implementation of ILogging from NSG.Library.Logger</param>
        /// <param name="fromAddress">From email address</param>
        /// <param name="toAddress">To email address</param>
        /// <param name="subject">Email subject line</param>
        /// <param name="message">Email body</param>
        /// <returns>IEMail to allow fluent design.</returns>
        public EMail(ILogger logging, string fromAddress, string toAddress, string subject, string message)
        {
            if (logging == null)
                this.Logging = new ListLogger("EMail");
            else
                this.Logging = logging;
            this.NewMailMessage(fromAddress, toAddress, subject, message);
        }
        /// <summary>
        /// Instantiate the SMTP MailMessage class using no parameters.
        /// <example>
        /// This sample shows how to call this constructor.
        /// <code>
        ///  MailAddress _from = new MailAddress("testfrom@example.com", "Example From User");
        ///  MailAddress _to = new MailAddress("testto@example.com", "Example To User");
        ///  string _subject = "Sending email is fun";
        ///  string _text = "This is a test email.";
        ///  IEMail _mail = new EMail().From(_from).To(_to).Subject(_subject).Body(_text).Send();
        /// </code>
        /// </example>
        /// </summary>
        /// <returns>IEMail to allow fluent design.</returns>
        public EMail()
        {
            if (Logging == null)
                this.Logging = new ListLogger("EMail");
            this.NewMailMessage();
        }
        /// <summary>
        /// Instantiate the SMTP MailMessage class using one parameter of logging.
        /// <example>
        /// This sample shows how to call this constructor.
        /// <code>
        ///  MailAddress _from = new MailAddress("testfrom@example.com", "Example From User");
        ///  MailAddress _to = new MailAddress("testto@example.com", "Example To User");
        ///  string _subject = "Sending email is fun";
        ///  string _text = "This is a test email.";
        ///  IEMail _mail = new EMail(Log.Logger).From(_from).To(_to).Subject(_subject).Body(_text).Send();
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="logging">This injects an implementation of ILogging from NSG.Library.Logger</param>
        /// <returns>IEMail to allow fluent design.</returns>
        public EMail( ILogger logging )
        {
            if (logging == null)
                this.Logging = new ListLogger("EMail");
            else
                this.Logging = logging;
            this.NewMailMessage();
        }
        //
        #endregion // Constructors
        //
        // IEMail NewMailMessage(fromAddress, toAddress, subject, message)
        // IEMail NewMailMessage( )
        //
        #region "NewMailMessage"
        //
        /// <summary>
        /// Given and EMail instance, start a new email message by
        /// instantiating a new SMTP MailMessage class without parameters.
        /// </summary>
        /// <param name="fromAddress">the from email address</param>
        /// <param name="toAddress">the from email address</param>
        /// <param name="subject">the subject line</param>
        /// <param name="message">the message body</param>
        /// <returns>return its self</returns>
        public IEMail NewMailMessage(string fromAddress, string toAddress, string subject, string message)
        {
            try
            {
                this.CleanUp();
                _mailMessage = new MailMessage(fromAddress, toAddress, subject, message);
                if (_emails == null)
                    _emails = new List<string>();
            }
            catch (Exception _ex)
            {
                System.Diagnostics.Debug.WriteLine(_ex.Message);
            }
            //
            return this;
        }
        // 
        /// <summary>
        /// Given and EMail instance, start a new email message by
        /// instantiating a new SMTP MailMessage class without parameters.
        /// </summary>
        /// <returns>return its self</returns>
        public IEMail NewMailMessage()
        {
            try
            {
                this.CleanUp();
                _mailMessage = new MailMessage();
                if (_emails == null)
                    _emails = new List<string>();
            }
            catch (Exception _ex)
            {
                System.Diagnostics.Debug.WriteLine(_ex.Message);
            }
            //
            return this;
        }
        //
        #endregion // NewMailMessage
        //
        //  IEMail From(fromAddress)
        //  IEMail From(fromAddress, name)
        //  IEMail From( MailAddress )
        //
        #region "From"
        //
        /// <summary>
        /// Set the single from address
        /// </summary>
        /// <param name="fromAddress">string of an email address</param>
        /// <returns>itself, IEMail to allow fluent design.</returns>
        public IEMail From(string fromAddress)
        {
            _mailMessage.From = new MailAddress(fromAddress);
            return this;
        }
        /// <summary>
        /// Set the single from email address
        /// </summary>
        /// <param name="fromAddress">string of an email address</param>
        /// <param name="name">display name of the email address</param>
        /// <returns>itself, IEMail to allow fluent design.</returns>
        public IEMail From(string fromAddress, string name)
        {
            _mailMessage.From = new MailAddress(fromAddress, name);
            return this;
        }
        /// <summary>
        /// Set the single from email address
        /// </summary>
        /// <param name="fromAddress">A MailAddress, including an email address and the name</param>
        /// <returns>itself, IEMail to allow fluent design.</returns>
        public IEMail From(MailAddress fromAddress)
        {
            _mailMessage.From = fromAddress;
            return this;
        }
        // 
        #endregion // From
        // 
        //  IEMail To(fromAddress)
        //  IEMail To(fromAddress, name)
        //  IEMail To( MailAddress )
        //
        #region "To"
        // 
        /// <summary>
        /// Add a To email address.
        /// </summary>
        /// <param name="toAddress">an email address</param>
        /// <returns>return its self</returns>
        public IEMail To(string toAddress)
        {
            _mailMessage.To.Add(new MailAddress(toAddress));
            return this;
        }
        /// <summary>
        /// Add a to email address.
        /// </summary>
        /// <param name="toAddress">an email address</param>
        /// <param name="name">display name of the email address</param>
        /// <returns>itself, IEMail to allow fluent design.</returns>
        public IEMail To(string toAddress, string name)
        {
            _mailMessage.To.Add(new MailAddress(toAddress, name));
            return this;
        }
        /// <summary>
        /// Add a to email address.
        /// </summary>
        /// <param name="toAddress">A MailAddress, including an email address and the name</param>
        /// <returns>itself, IEMail to allow fluent design.</returns>
        public IEMail To(MailAddress toAddress)
        {
            _mailMessage.To.Add(toAddress);
            return this;
        }
        // 
        #endregion // To
        // 
        //  IEMail CC(fromAddress)
        //  IEMail CC(fromAddress, name)
        //  IEMail CC( MailAddress )
        //
        #region "CC"
        //
        /// <summary>
        /// Add a carbon copy (cc) email address.
        /// </summary>
        /// <param name="ccAddress">an email address</param>
        /// <returns>return its self</returns>
        public IEMail CC(string ccAddress)
        {
            _mailMessage.CC.Add(new MailAddress(ccAddress));
            return this;
        }
        /// <summary>
        /// Add a carbon copy (cc) email address.
        /// </summary>
        /// <param name="ccAddress">an email address</param>
        /// <param name="name">display name of the email address</param>
        /// <returns>itself, IEMail to allow fluent design.</returns>
        public IEMail CC(string ccAddress, string name)
        {
            _mailMessage.CC.Add(new MailAddress(ccAddress, name));
            return this;
        }
        /// <summary>
        /// Add a carbon copy (cc) email address.
        /// </summary>
        /// <param name="ccAddress">A MailAddress, including an email address and the name</param>
        /// <returns>itself, IEMail to allow fluent design.</returns>
        public IEMail CC(MailAddress ccAddress)
        {
            _mailMessage.CC.Add(ccAddress);
            return this;
        }
        //
        #endregion // CC
        // 
        //  IEMail BCC(fromAddress)
        //  IEMail BCC(fromAddress, name)
        //  IEMail BCC( MailAddress )
        //
        #region "BCC"
        //
        /// <summary>
        /// Add a blind carbon copy (bcc) email address.
        /// </summary>
        /// <param name="bccAddress">an email address</param>
        /// <returns>return its self</returns>
        public IEMail BCC(string bccAddress)
        {
            _mailMessage.Bcc.Add(new MailAddress(bccAddress));
            return this;
        }
        /// <summary>
        /// Add a blind carbon copy (bcc) email address.
        /// </summary>
        /// <param name="bccAddress">an email address</param>
        /// <param name="name">display name of the email address</param>
        /// <returns>itself, IEMail to allow fluent design.</returns>
        public IEMail BCC(string bccAddress, string name)
        {
            _mailMessage.Bcc.Add(new MailAddress(bccAddress, name));
            return this;
        }
        /// <summary>
        /// Add a blind carbon copy (bcc) email address.
        /// </summary>
        /// <param name="bccAddress">A MailAddress, including an email address and the name</param>
        /// <returns>itself, IEMail to allow fluent design.</returns>
        public IEMail BCC(MailAddress bccAddress)
        {
            _mailMessage.Bcc.Add(bccAddress);
            return this;
        }
        //
        #endregion // BCC
        //
        /// <summary>
        /// Set the title of email message.
        /// </summary>
        /// <param name="subject">the subject line</param>
        /// <returns>return its self</returns>
        public IEMail Subject( string subject )
        {
            _mailMessage.Subject = subject;
            return this;
        }
        //
        /// <summary>
        /// Set the body of the email message.
        /// </summary>
        /// <param name="body">Email body</param>
        /// <returns>return its self</returns>
        public IEMail Body(string body)
        {
            _mailMessage.Body = body;
            return this;
        }
        //
        /// <summary>
        /// Is the mail message body Html
        /// </summary>
        /// <param name="isBodyHtml">true ot false, is body Html</param>
        /// <returns>itself, IEMail to allow fluent design.</returns>
        public IEMail Html(bool isBodyHtml)
        {
            _mailMessage.IsBodyHtml = isBodyHtml;
            return this;
        }
        // 
        /// <summary>
        /// Add an attachment to the mail message.
        /// <example> 
        /// This sample shows how to call this constructor.
        /// <code>
        ///  IEMail _mail = new EMail("testfrom@example.com", "testto@example.com", "Sending email is Fun", "This is a test email.")
        ///         .Attachment(_textBuffer, _textFile, _textMimeType).Send();
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="buffer">byte buffer containing the attachment</param>
        /// <param name="displayName">name placed on the attachment</param>
        /// <param name="mimeType">mime type like 'application/pdf'</param>
        /// <returns>return its self</returns>
        public IEMail Attachment(byte[] buffer, string displayName, string mimeType)
        {
            try
            {
                MemoryStream _memStream = null;
                Attachment _attach = null;
                if (buffer != null)
                {
                    System.Net.Mime.ContentType _contentType = new System.Net.Mime.ContentType(mimeType);
                    _contentType.Name = displayName;
                    _memStream = new MemoryStream(buffer);
                    _attach = new Attachment(_memStream, _contentType);
                    //    Attachment stream must be open when email is sent.
                    _mailMessage.Attachments.Add(_attach);
                }
            }
            catch (Exception _ex)
            {
                System.Diagnostics.Debug.WriteLine(_ex.Message);
            }
            //
            return this;
        }
        // 
        /// <summary>
        /// Use appropriate EMail to send out the message/attachments.
        /// After sending clean-up the attachments and the streams and 
        /// finally the mail-message.
        /// </summary>
        /// <returns>return its self</returns>
        public IEMail Send()
        {
            string _codeName = "Email.Send";
            try
            {
                // Use Smtp/SendGrid/Mailgun/List package
                string _emailService = Helpers.Config.GetStringAppSettingConfigValue("Email:UseService", "-").ToLower();
                if (_emailService == "smtp")
                {
                    SendSmtpAsync();
                    return this;
                }
                //
                string _apiKey = Helpers.Config.GetStringAppSettingConfigValue("Email:ApiKey", "-").ToLower();
                switch (_emailService)
                {
                    case "sendgrid":
                        SendSendGridAsync( _apiKey );
                        break;
                    case "mailgun":
                        string _domain = Helpers.Config.GetStringAppSettingConfigValue("Email:MailgunDomain", "-").ToLower();
                        SendMailGunAsync(_domain, _apiKey);
                        break;
                    case "list":
                        SendList();
                        break;
                    default:
                        Logging.Log( LoggingLevel.Warning, "unknown", MethodBase.GetCurrentMethod(), _codeName + " defaulting to Smtp");
                        break;
                }
            }
            catch (Exception _ex)
            {
                System.Diagnostics.Debug.WriteLine(_ex.Message);
                var _exp = _ex.GetBaseException();
                Logging.Log(LoggingLevel.Error, "unknown", MethodBase.GetCurrentMethod(), _exp.Message, _ex);
            }
            // 
            return this;
        }
        // 
        /// <summary>
        /// Use MMPA.Library.EMailing to send out the message/attachments.
        /// After sending clean-up the attachments and the streams and 
        /// finally the mail-message.
        /// </summary>
        /// <returns>return its self</returns>
        public IEMail SendAsync()
        {
            return Send();
        }
        //
        /// <summary>
        /// Get the current MailMessage
        /// </summary>
        /// <returns>the current MailMessage</returns>
        public MailMessage GetMailMessage()
        {
            return _mailMessage;
        }
        //
        /// <summary>
        /// return the last few log messages...
        /// </summary>
        /// <param name="lastCount">Count of last log records to return</param>
        /// <returns>list of strings</returns>
        public List<string> Logs( int lastCount )
        {
            return Logging.ListString( lastCount );
        }
        //
        /// <summary>
        /// Output the list of string of the last max emails sent
        /// </summary>
        /// <returns>list of strings</returns>
        public List<string> Emails()
        {
            return _emails;
        }
        //
        private void CleanUpAttachments()
        {
            if (_mailMessage != null)
            {
                //  Clean-up
                if ((_mailMessage.Attachments.Count > 0))
                {
                    for (int _idx = (_mailMessage.Attachments.Count - 1); (_idx <= 0); _idx++)
                    {
                        Attachment _attach = _mailMessage.Attachments[_idx];
                        _attach.ContentStream.Close();
                        _attach.ContentStream.Dispose();
                        _attach.Dispose();
                        _mailMessage.Attachments.RemoveAt(_idx);
                    }
                }
            }
        }
        // 
        /// <summary>
        /// Naturally and automatically dispose of mail-message resource.
        /// </summary>
        private void CleanUp()
        {
            if (_mailMessage != null)
            {
                //  Clean-up
                _mailMessage.Dispose();
                _mailMessage = null;
            }
        }
        //
        /// <summary>
        /// Format a string of the current mail message
        /// </summary>
        /// <returns>formated string of a mail message</returns>
        private static string EmailToString( )
        {
            string _attachments = string.Join("", _mailMessage.Attachments.AsEnumerable().Select(_a => ", Attachment: [" + _a.Name + " " + _a.ContentType.MediaType + "]").ToList());
            //
            return String.Format("From: {0}, To: {1}, Subject: {2}, Body: {3}{4}",
                ToEmailAddress(_mailMessage.From),
                ToEmailAddress(_mailMessage.To[0]),
                _mailMessage.Subject, _mailMessage.Body, _attachments);
        }
        //
        /// <summary>
        /// Format and SMTP MailAddress structure to a string
        /// </summary>
        /// <param name="mailAddress">SMTP MailAddress</param>
        /// <returns>string like Phil Huhn PHuhn@yahoo.com</returns>
        private static string ToEmailAddress(MailAddress mailAddress)
        {
            return string.Format("{0}{1}<{2}>",
                mailAddress.DisplayName, (mailAddress.DisplayName == ""? "": " "), mailAddress.Address);
        }
        //
        /// <summary>
        /// Write the current email to a list of string
        /// </summary>
        private static void SendList()
        {
            _emails.Add(EmailToString());
            if (_emails.Count > _max)
                _emails.Remove(_emails[0]);
        }
    }
}
