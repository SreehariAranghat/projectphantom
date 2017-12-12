using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportPanda.Core
{
    public class EmailService : ControllerBase, IEmailService
    {
        ITemplateService templateService;

        public EmailService(ITemplateService templateService)
        {
            this.templateService = templateService;
        }

        public void Sent(string templatename, string subject, string to, object data)
        {
            string content = templateService.FromTemplate(templatename, data);

            if (!string.IsNullOrEmpty(content))
            {
                Sent(subject, content, to, string.Empty, string.Empty);
            }
        }

        public void Sent(string subject, string body, string to, string cc, string bcc)
        {
            dynamic smtpsettings = Application.GetSettings(SettingType.SMTP);

            RestClient client     = new RestClient();
            client.BaseUrl        = new Uri("https://api.mailgun.net/v3");
            client.Authenticator  = new HttpBasicAuthenticator("api", "key-e8e2ab5f62eed90a65c7e4aeff9aea1b");

            RestRequest request   = new RestRequest();
            request.AddParameter("domain", "supportpanda.net", ParameterType.UrlSegment);
            request.Resource      = "{domain}/messages";

            request.AddParameter("from", smtpsettings.FRIENDLYNAME.ToString() + "<" + smtpsettings.FROMADDRESS + ">");
            request.AddParameter("to", to);

            if(!string.IsNullOrEmpty(bcc))
                request.AddParameter("bcc", bcc);

            if (!string.IsNullOrEmpty(cc))
                request.AddParameter("cc", cc);

            request.AddParameter("subject", subject);
            request.AddParameter("html", body);

            request.AddParameter("o:tracking", false);

            request.Method       = Method.POST;
            client.ExecuteAsync(request, ((a) => { }));
        }
    }
}
