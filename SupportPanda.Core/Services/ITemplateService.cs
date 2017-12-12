using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportPanda.Core
{
    public interface ITemplateService
    {
        string FromTemplate(string templateName, object data);
    }
}
