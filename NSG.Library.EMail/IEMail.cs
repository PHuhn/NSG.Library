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
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Net.Http;
using System.Threading.Tasks;
using System.Reflection;
using SendGrid.Helpers.Mail;
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
    /// * production,
    /// * Email:Enabled
    /// * Email:TestEmailName,
    /// * Email:UseService,
    /// * Email:ApiKey,
    /// * Email:MailgunDomain.
    /// </summary>
    // 
    // <summary>
    // Interface for implementing the fluent design.  This implementaion
    // revolves around the SMTP MailMessage class.
    // </summary>
    public interface IEMail
    {
        //
        /// <summary>
        /// Instantiate the SMTP MailMessage class using the four parameters.
        /// </summary>
        /// <param name="fromAddress">From email address</param>
        /// <param name="toAddress">To email address</param>
        /// <param name="subject">Email subject line</param>
        /// <param name="message">Email body</param>
        /// <returns>IEMail to allow fluent design.</returns>
        IEMail NewMailMessage( string fromAddress, string toAddress, string subject, string message );
        /// <summary>
        /// Instantiate the SMTP MailMessage class without parameters.
        /// </summary>
        /// <returns>IEMail to allow fluent design.</returns>
        IEMail NewMailMessage( );
        //
        /// <summary>
        /// Load a SendGridMessage mail message into this message
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
        /// <param name="sgm">a SendGridMessage mail message</param>
        /// <returns>IEMail to allow fluent design.</returns>
        IEMail NewMailMessage(SendGridMessage sgm);
        /// <summary>
        /// Get the current MailMessage
        /// </summary>
        /// <returns>the current MailMessage</returns>
        MailMessage GetMailMessage( );
        //
        /// <summary>
        /// Set the single from email address
        /// </summary>
        /// <param name="fromAddress">an email address</param>
        /// <returns>itself, IEMail to allow fluent design.</returns>
        IEMail From( string fromAddress );
        /// <summary>
        /// Set the single from email address
        /// </summary>
        /// <param name="fromAddress">an email address</param>
        /// <param name="name">display name of the email address</param>
        /// <returns>itself, IEMail to allow fluent design.</returns>
        IEMail From( string fromAddress, string name );
        /// <summary>
        /// Set the single from email address
        /// </summary>
        /// <param name="fromAddress">A MailAddress, including an email address and the name</param>
        /// <returns>itself, IEMail to allow fluent design.</returns>
        IEMail From( MailAddress fromAddress );
        //
        /// <summary>
        /// Add a to email address.
        /// </summary>
        /// <param name="toAddress">an email address</param>
        /// <returns>itself, IEMail to allow fluent design.</returns>
        IEMail To( string toAddress );
        /// <summary>
        /// Add a to email address.
        /// </summary>
        /// <param name="toAddress">an email address</param>
        /// <param name="name">display name of the email address</param>
        /// <returns>itself, IEMail to allow fluent design.</returns>
        IEMail To( string toAddress, string name );
        /// <summary>
        /// Add a to email address.
        /// </summary>
        /// <param name="toAddress">A MailAddress, including an email address and the name</param>
        /// <returns>itself, IEMail to allow fluent design.</returns>
        IEMail To( MailAddress toAddress );
        //
        /// <summary>
        /// Add a carbon copy (cc) email address
        /// </summary>
        /// <param name="ccAddress">an email address</param>
        /// <returns>itself, IEMail to allow fluent design.</returns>
        IEMail CC( string ccAddress );
        /// <summary>
        /// Add a carbon copy (cc) email address
        /// </summary>
        /// <param name="ccAddress">an email address</param>
        /// <param name="name">display name of the email address</param>
        /// <returns>itself, IEMail to allow fluent design.</returns>
        IEMail CC( string ccAddress, string name );
        /// <summary>
        /// Add a carbon copy (cc) email address
        /// </summary>
        /// <param name="ccAddress">A MailAddress, including an email address and the name</param>
        /// <returns>itself, IEMail to allow fluent design.</returns>
        IEMail CC( MailAddress ccAddress );
        //
        /// <summary>
        /// Add a blind carbon copy (bcc) email address
        /// </summary>
        /// <param name="bccAddress">an email address</param>
        /// <returns>itself, IEMail to allow fluent design.</returns>
        IEMail BCC( string bccAddress );
        /// <summary>
        /// Add a blind carbon copy (bcc) email address
        /// </summary>
        /// <param name="bccAddress">an email address</param>
        /// <param name="name">display name of the email address</param>
        /// <returns>itself, IEMail to allow fluent design.</returns>
        IEMail BCC( string bccAddress, string name );
        /// <summary>
        /// Add a blind carbon copy (bcc) email address
        /// </summary>
        /// <param name="bccAddress">A MailAddress, including an email address and the name</param>
        /// <returns>itself, IEMail to allow fluent design.</returns>
        IEMail BCC( MailAddress bccAddress );
        //
        /// <summary>
        /// Set the email subject line
        /// </summary>
        /// <param name="subject">Email subject line</param>
        /// <returns>itself, IEMail to allow fluent design.</returns>
        IEMail Subject( string subject );
        //
        /// <summary>
        /// Set the email body/text.
        /// </summary>
        /// <param name="body">Email body</param>
        /// <returns>itself, IEMail to allow fluent design.</returns>
        IEMail Body( string body );
        //
        /// <summary>
        /// Is the mail message body Html
        /// </summary>
        /// <param name="isBodyHtml">true ot false, is body Html</param>
        /// <returns>itself, IEMail to allow fluent design.</returns>
        IEMail Html(bool isBodyHtml);
        //
        /// <summary>
        /// Set an email attachment.
        /// </summary>
        /// <param name="buffer">byte array of the file to attach</param>
        /// <param name="displayName">file name of the attachment</param>
        /// <param name="mimeType">mime type of the attachment</param>
        /// <returns>itself, IEMail to allow fluent design.</returns>
        IEMail Attachment( byte[] buffer, string displayName, string mimeType );
        //
        /// <summary>
        /// Send the email and any attachments, via the means defined
        /// in the appSetting:
        /// * Email:UseService.
        /// </summary>
        /// <returns>itself, IEMail to allow fluent design.</returns>
        IEMail Send();
        // 
        /// <summary>
        /// Use MMPA.Library.EMailing to send out the message/attachments.
        /// After sending clean-up the attachments and the streams and 
        /// finally the mail-message.
        /// </summary>
        /// <returns>return its self</returns>
        IEMail SendAsync();
        //
        // Smtp
        //
        /// <summary>
        /// Send the email and any attachments, via the SMTP.
        /// </summary>
        /// <returns>itself, IEMail to allow fluent design.</returns>
        IEMail SendSmtp( );
        /// <summary>
        /// Send the email and any attachments, via the SMTP async task.
        /// </summary>
        /// <returns>itself, IEMail to allow fluent design.</returns>
        IEMail SendSmtpAsync( );
        //
        // Send-Grid
        //
        /// <summary>
        /// Translate from MailMessage to SendGridMessage.
        /// </summary>
        /// <returns>SendGridMessage</returns>
        SendGrid.Helpers.Mail.SendGridMessage ToSendGrid( );
        /// <summary>
        /// Send the email and any attachments, via the SendGrid async task.
        /// </summary>
        /// <param name="apiKey">The SendGrid api key</param>
        /// <returns>itself, IEMail to allow fluent design.</returns>
        IEMail SendSendGridAsync( string apiKey );
        //
        // Mail-gun
        //
        /// <summary>
        /// Property to override the default MailGun URL.
        /// </summary>
        string MailgunUrl { get; set; }
        /// <summary>
        /// Translate from MailMessage to MultipartFormDataContent.
        /// </summary>
        /// <returns>MultipartFormDataContent (see unit test)</returns>
        MultipartFormDataContent ToMailGun( );
        /// <summary>
        /// Send the email and any attachments, via the MailGun async task.
        /// Note: this was not tested (did not have an account).
        /// </summary>
        /// <param name="apiKey">The MailGun api key.</param>
        /// <param name="domainName">Your domain defined with MailGun.</param>
        /// <returns>itself, IEMail to allow fluent design.</returns>
        IEMail SendMailGunAsync( string apiKey, string domainName );
        //
        /// <summary>
        /// retrurn the last few log messages...
        /// </summary>
        /// <param name="lastCount">Count of last log records to return</param>
        /// <returns>list of strings</returns>
        /// <remarks>Output from ILogger</remarks>
        List<string> Logs(int lastCount);
        //
        /// <summary>
        /// Output a list of string of the last # of emails sent
        /// </summary>
        /// <returns>list of strings</returns>
        List<string> Emails();
        //
    }
    // 
}
//
