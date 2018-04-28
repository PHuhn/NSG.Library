//
using System;
using System.Text;
using System.Reflection;
using System.Net.Mail;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
//
using NSG.Library.Logger;
//
namespace NSG.Library.EMail
{
    // 
    // https://stackoverflow.com/questions/10339877/asp-net-webapi-how-to-perform-a-multipart-post-with-file-upload-using-webapi-ht
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public partial class EMail : IEMail
    {
        //
        string _mailgunUrl = "https://api.mailgun.net/v3/{0}/messages";
        /// <summary>
        /// Property to override the default MailGun URL.
        /// </summary>
        public string MailgunUrl
        {
            set { _mailgunUrl = value; }
            get { return _mailgunUrl; }
        }
        //
        /// <summary>
        /// Translate from MailMessage to MultipartFormDataContent.
        /// </summary>
        /// <returns>MultipartFormDataContent (see unit test)</returns>
        public MultipartFormDataContent ToMailGun( )
        {
            string _quoteFormat = "\"{0}\"";
            //
            MultipartFormDataContent _content = new MultipartFormDataContent();
            _content.Add(new StringContent(
                ToEmailAddress(_mailMessage.From)), "\"from\"");
            if (_mailMessage.To.Count > 0)
                foreach (MailAddress _to in _mailMessage.To)
                    _content.Add(new StringContent(
                        ToEmailAddress(_to)), "\"to\"");
            if (_mailMessage.CC.Count > 0)
                foreach (MailAddress _cc in _mailMessage.To)
                    _content.Add(new StringContent(
                        ToEmailAddress(_cc)), "\"cc\"");
            if (_mailMessage.Bcc.Count > 0)
                foreach (MailAddress _bcc in _mailMessage.To)
                    _content.Add(new StringContent(
                        ToEmailAddress(_bcc)), "\"bcc\"");
            _content.Add(new StringContent(_mailMessage.Subject), "\"subject\"");
            string _tag = (_mailMessage.IsBodyHtml ? "\"html\"" : "\"text\"");
            _content.Add(new StringContent(_mailMessage.Body), _tag);
            //
            if (_mailMessage.Attachments.Count > 0)
            {
                foreach (var _attachment in _mailMessage.Attachments)
                {
                    long _len = _attachment.ContentStream.Length;
                    byte[] _buffer = new byte[_len];
                    _attachment.ContentStream.Read(_buffer, 0, (int)_len);
                    //
                    var _fileContent = new ByteArrayContent(_buffer);
                    _fileContent.Headers.ContentDisposition =
                        new ContentDispositionHeaderValue("attachment")
                    {
                        FileName = string.Format(_quoteFormat, _attachment.Name)
                    };
                    _content.Add(_fileContent);
                }
            }
            //
            return _content;
        }
        //
        /// <summary>
        /// Send the email and any attachments, via the MailGun async task.
        /// Note: this was not tested (did not have an account).
        /// </summary>
        /// <param name="apiKey">The MailGun api key.</param>
        /// <param name="domainName">Your domain defined with MailGun.</param>
        /// <returns>itself, IEMail to allow fluent design.</returns>
        public IEMail SendMailGunAsync(string apiKey, string domainName)
        {
            try
            {
                SendList(); // also stuff all emails into a list
                //
                string _url = string.Format(MailgunUrl, domainName);
                using ( var _client = new HttpClient() )
                {
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                        Convert.ToBase64String(Encoding.ASCII.GetBytes("api:" + apiKey)));
                    using (MultipartFormDataContent _content = this.ToMailGun())
                    {
                        var _result = _client.PostAsync(_url, _content).Result;
                    }
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
    }
}
//
