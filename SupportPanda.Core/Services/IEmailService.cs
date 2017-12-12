using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportPanda.Core
{
    public interface IEmailService
    {
        void Sent(string templatename,string subject,string to, object data);
        void Sent(string subject, string body, string to, string cc, string bcc);
    }
}
