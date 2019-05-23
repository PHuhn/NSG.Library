using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;
//
using SendGrid;
using SendGrid.Helpers.Mail;
//
using NSG.Library.EMail;
using NSG.Library.Helpers;
//
namespace NSG.Library_Tests
{
    [TestClass]
    public class NSG_Library_EMail_Tests
    {
        //
        private string _path = @"App_Data\";
        private string _textMimeType = "text/plain";
        private string _rtfMimeType = "application/rtf";
        // private string _pdfMimeType = "application/pdf";
        // private string _excelMimeType = "application/vnd.ms-excel"; // 2003 excel
        MailAddress _from = new MailAddress("testfrom@example.com", "Example From User");
        MailAddress _to = new MailAddress("testto@example.com", "Example To User");
        string _subject = "Sending with SendGrid is Fun";
        string _text = "This is a test of the national broadcasting system's, mail service";
        //
        [TestMethod]
        public void EMail_MailMessage_Test()
        {
            //
            IEMail _mail = EMail_CreateTestMessage();
            MailMessage _mailMessage = _mail.GetMailMessage();
			//
            Assert.AreEqual(_from.Address, _mailMessage.From.Address);
            Assert.AreEqual(_to.Address, _mailMessage.To[0].Address);
            Assert.AreEqual(_subject, _mailMessage.Subject);
            Assert.AreEqual(_text, _mailMessage.Body);
            //
        }
        //
        [TestMethod]
        public void EMail_Smtp_Basic_Test()
        {
            //
            string _subject = "ZZZZZZZZZZZZZZZZZZZZZZZZ";
            IEMail _mail = EMail_CreateTestMessage().Subject(_subject);
            // this uses the config   <system.net><mailSettings><smtp ...
            _mail.SendSmtp();
            //
            List<string> _errors = _mail.Logs(5);
            if (_errors.Count > 0)
                Console.WriteLine(_errors[0]);
            Assert.AreEqual(0, _errors.Count);
            List<string> _emails = _mail.Emails();
            if (_emails.Count > 0)
            {
                string _email = _emails[_emails.Count - 1];
                Console.WriteLine(_email);
                Assert.IsTrue(_email.Contains(_subject));
            }
            else
                Assert.Fail();
            //
        }
        //
        [TestMethod]
        public void EMail_SmtpAsync_Basic_Test()
        {
            //
            IEMail _mail = EMail_CreateTestMessage();
            // this uses the config   <system.net><mailSettings><smtp ...
            _mail.SendSmtpAsync();
            //
            List<string> _errors = _mail.Logs(5);
            if (_errors.Count > 0)
                Console.WriteLine(_errors[0]);
            Assert.AreEqual(0, _errors.Count);
            List<string> _emails = _mail.Emails();
            if (_emails.Count > 0)
                Console.WriteLine(_emails[0]);
            //
        }
        //
        [TestMethod]
        [DeploymentItem("App_Data", "App_Data")]
        public void EMail_SmtpAsync_Attachment_Test()
        {
            //
            string _textFile = @"TextFile.txt";
            string _textPath = _path + _textFile;
            if (File.Exists(_textPath))
                System.Diagnostics.Debug.WriteLine("Library_EMail_Attachment_Test: " + _textFile + " Found");
            else
                Assert.Fail("Library_EMail_Attachment_Test: " + _textFile + " NOT Found ...");
            byte[] _textBuffer = Files.FileToByteArray(_textPath);
            //
            IEMail _mail = EMail_CreateTestMessage()
                .Attachment(_textBuffer, _textFile, _textMimeType);
            MailMessage _mm = _mail.GetMailMessage();
            // this uses the config   <system.net><mailSettings><smtp ...
            _mail.SendSmtpAsync();
            //
            List<string> _errors = _mail.Logs(5);
            if (_errors.Count > 0)
                Console.WriteLine(_errors[0]);
            Assert.AreEqual(0, _errors.Count);
            List<string> _emails = _mail.Emails();
            if (_emails.Count > 0)
                Console.WriteLine(_emails[0]);
            //
        }
        //
        [TestMethod]
        public void EMail_SendGrid_ToSendGridMessage_Test()
        {
            // https://sendgrid.com/docs/API_Reference/Web_API_v3/Mail/index.html
            //
            IEMail _mail = EMail_CreateTestMessage();
            SendGridMessage _sgMessage = _mail.ToSendGrid();
            //
            Assert.AreEqual(_from.Address, _sgMessage.From.Email);
            Assert.AreEqual(_to.Address, _sgMessage.Personalizations[0].Tos[0].Email);
            Assert.AreEqual(_subject, _sgMessage.Personalizations[0].Subject);
            Assert.AreEqual(_text, _sgMessage.PlainTextContent);
            //
        }
        //
        [TestMethod]
        [DeploymentItem("App_Data", "App_Data")]
        public void EMail_SendGrid_ToSendGridMessage_Attachment_Test()
        {
            // https://sendgrid.com/docs/API_Reference/Web_API_v3/Mail/index.html
            //
            string _textFile = @"TextFile.txt";
            string _textPath = _path + _textFile;
            if (File.Exists(_textPath))
                System.Diagnostics.Debug.WriteLine("EMail_SendGrid_ToSendGridMessage_Attachment_Test: " + _textFile + " Found");
            else
                Assert.Fail("EMail_SendGrid_ToSendGridMessage_Attachment_Test: " + _textFile + " NOT Found ...");
            byte[] _textBuffer = Files.FileToByteArray(_textPath);
            IEMail _mail = EMail_CreateTestMessage()
                .Attachment(_textBuffer, _textFile, _textMimeType);
            SendGridMessage _sgMessage = _mail.ToSendGrid();
            //
            Assert.AreEqual(_from.Address, _sgMessage.From.Email);
            Assert.AreEqual(_to.Address, _sgMessage.Personalizations[0].Tos[0].Email);
            Assert.AreEqual(_subject, _sgMessage.Personalizations[0].Subject);
            Assert.AreEqual(_text, _sgMessage.PlainTextContent);
            string _content = _sgMessage.Attachments[0].Content;
            Assert.AreEqual(Encoding.Default.GetString(_textBuffer),
                Encoding.Default.GetString(Convert.FromBase64String(_content)));
            //
        }
        //
        [TestMethod]
        public void EMail_SendGrid_Basic_Test()
        {
            //
            string _apiKey = ConfigurationManager.AppSettings["Email:ApiKey"].ToString();
            //
            IEMail _mail = EMail_CreateTestMessage();
            //
            _mail.SendSendGridAsync(_apiKey);
            //
            List<string> _errors = _mail.Logs(5);
            if (_errors.Count > 0)
                Console.WriteLine(_errors[0]);
            Assert.AreEqual(0, _errors.Count);
            List<string> _emails = _mail.Emails();
            if (_emails.Count > 0)
                Console.WriteLine(_emails[0]);
            //
        }
        //
        [TestMethod]
        [DeploymentItem("App_Data", "App_Data")]
        public void EMail_SendGrid_Attachment_Test()
        {
            //
            string _apiKey = ConfigurationManager.AppSettings["Email:ApiKey"].ToString();
            //
            string _textFile = @"TextFile.txt";
            string _textPath = _path + _textFile;
            string _rtfFile = @"Test_Rtf.rtf";
            string _rtfPath = _path + _rtfFile;
            if (File.Exists(_textPath))
                System.Diagnostics.Debug.WriteLine("Library_EMail_Attachment_Test: " + _textFile + " Found");
            else
                Assert.Fail("Library_EMail_Attachment_Test: " + _rtfFile + " NOT Found ...");
            if (File.Exists(_rtfPath))
                System.Diagnostics.Debug.WriteLine("Library_EMail_Attachment_Test: " + _rtfFile + " Found");
            else
                Assert.Fail("Library_EMail_Attachment_Test: " + _textFile + " NOT Found ...");
            byte[] _textBuffer = Files.FileToByteArray(_textPath);
            Console.WriteLine(Encoding.Default.GetString(_textBuffer));
            byte[] _rtfBuffer = Files.FileToByteArray(_rtfPath);
            //
            IEMail _mail = EMail_CreateTestMessage()
                .Attachment(_rtfBuffer, _rtfFile, _rtfMimeType);
            //    .Attachment(_textBuffer, _textFile, _textMimeType);
            //
            _mail.SendSendGridAsync(_apiKey);
            //
            List<string> _errors = _mail.Logs(5);
            if (_errors.Count > 0)
                Console.WriteLine(_errors[0]);
            Assert.AreEqual(0, _errors.Count);
            //
        }
        //
        [TestMethod]
        public void EMail_MailGun_ToMailGunMessage_Test()
        {
            //
            string _textFile = @"TextFile.txt";
            string _textPath = _path + _textFile;
            if (File.Exists(_textPath))
                System.Diagnostics.Debug.WriteLine("Library_EMail_Attachment_Test: " + _textFile + " Found");
            else
                Assert.Fail("Library_EMail_Attachment_Test: " + _textFile + " NOT Found ...");
            byte[] _textBuffer = Files.FileToByteArray(_textPath);
			//
            IEMail _mail = EMail_CreateTestMessage()
                .Attachment(_textBuffer, _textFile, _textMimeType);
            MultipartFormDataContent _mgMessage = _mail.ToMailGun();
            //
            Task.Run(async () =>
            {
                string _multipartString = await _mgMessage.ReadAsStringAsync();
                Console.WriteLine( _multipartString );
            }).GetAwaiter().GetResult();

			//Assert.AreEqual(_from.Address, _mgMessage..From.Email);
            //Assert.AreEqual(_to.Address, _mgMessage.To[0].Email);
            //Assert.AreEqual(_subject, _mgMessage.Subject);
            //Assert.AreEqual(_text, _mgMessage.Text);
            //
        }
		//
        [TestMethod]
        public void EMail_Base64_Test()
        {
            string _text = @"This is a test of the Emergency Broadcast System.

The broadcasters of your area in voluntary cooperation with the FCC
and federal, state and local authorities have developed this system
to keep you informed in the event of an emergency.

This is a test of the mail service.
";
            var _bytes = Encoding.ASCII.GetBytes(_text.ToCharArray());
            var _buffer = Convert.ToBase64String(Encoding.ASCII.GetBytes(_text.ToCharArray()));
            string _text64 = Encoding.Default.GetString(Convert.FromBase64String(_buffer));
            Console.WriteLine(_text);
            Console.WriteLine(_text64);
            Console.WriteLine(_buffer);
            Assert.AreEqual(_text, _text64);
        }
        //
        [TestMethod]
        public void EMail_SendGrid_NewMailMessage_Test()
        {
            //
            string _textFile = @"TextFile.txt";
            string _textPath = _path + _textFile;
            if (File.Exists(_textPath))
                System.Diagnostics.Debug.WriteLine("Library_EMail_Attachment_Test: " + _textFile + " Found");
            else
                Assert.Fail("Library_EMail_Attachment_Test: " + _textFile + " NOT Found ...");
            byte[] _textBuffer = Files.FileToByteArray(_textPath);
            IEMail _mail = EMail_CreateTestMessage()
                .Attachment(_textBuffer, _textFile, _textMimeType);
            SendGridMessage _sgMessage = _mail.ToSendGrid();
            _mail.NewMailMessage( _sgMessage );
            MailMessage _mm = _mail.GetMailMessage( );
            //
            Assert.AreEqual(_from.Address, _mm.From.Address);
            Assert.AreEqual(_from.DisplayName, _mm.From.DisplayName);
            Assert.AreEqual(_to.Address, _mm.To[0].Address);
            Assert.AreEqual(_to.DisplayName, _mm.To[0].DisplayName);
            Assert.AreEqual(_subject, _mm.Subject);
            Assert.AreEqual(_text, _mm.Body);
            // attachment
            byte[] _actualContent = null;
            _mm.Attachments[0].ContentStream.Position = 0;
            int _len = (int)_mm.Attachments[0].ContentStream.Length;
            _actualContent = new byte[_len - 1 + 1];
            _mm.Attachments[0].ContentStream.Read(_actualContent, 0, _len);
            Assert.AreEqual(Encoding.Default.GetString(_textBuffer),
                Encoding.Default.GetString(_actualContent));
            //
        }
        //
        [TestMethod]
        public void EMail_SendGrid_NewMailMessage_Deserialize_Test()
        {
            string sgEmail = "{\"contents\":[{\"value\":\"Hi\",\"type\":\"text/plain\"}],\"personalizations\":[{\"tos\":[{\"email\":\"abuse@internap.com\"}],\"ccs\":[],\"bccs\":[],\"subject\":\"Denial-of-service attack from 63.251.98.12\"}],\"from\":{\"email\":\"PhilHuhn@yahoo.com\",\"name\":\"Phil Huhn\"},\"subject\":\"Denial-of-service attack from 63.251.98.12\",\"plainTextContent\":\"\"}";
            SendGridMessage _sgm = JsonConvert.DeserializeObject<SendGridMessage>(sgEmail);
            IEMail _email = new EMail().NewMailMessage(_sgm);
            Assert.AreEqual("Hi", ((MailMessage)_email.GetMailMessage()).Body);
        }
        //
        [TestMethod]
        public void EMail_GMail_NewMailMessage_Test()
        {
            string _fromAddress = "Phil.N.Huhn@gmail.com";
            IEMail _mail = new EMail().From(_fromAddress, "Phil N. Huhn").To("PhilHuhn@yahoo.com")
                .Subject("Test Message from Yourself").Body("This is a test Message from yourself. Phil");
            try
            {
                using (var _client = new SmtpClient("smtp.gmail.com", 587))
                {
                    _client.EnableSsl = false;
                    _client.Credentials = new System.Net.NetworkCredential(_fromAddress, "p@ssW0rd8");
                    _client.Send(_mail.GetMailMessage());
                }
            }
            catch (Exception _ex)
            {
                System.Diagnostics.Debug.WriteLine(_ex.Message);
                var _exp = _ex.GetBaseException();
                System.Diagnostics.Debug.WriteLine(_exp.Message);
            }
        }
        //
        //  Support
        //
        public IEMail EMail_CreateTestMessage()
        {
            //
            IEMail _mail = new EMail().From(_from).To(_to).Subject(_subject).Body(_text);
            //
            return _mail;
        }
    }
}
//
