using RazorEngine;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportPanda.Core
{
    public class TemplateService : ControllerBase, ITemplateService
    {
        public string FromTemplate(string templateName, object data)
        {
            string htmlData     = string.Empty;
            string templateFile = Application.GetSettings(SettingType.EMAILTEMPLATE).PATH.ToString() + "\\" + templateName + ".chtml";

            if (File.Exists(templateFile))
            {
                StreamReader read = new StreamReader(templateFile);
                string content    = read.ReadToEnd();

                read.Close();
                htmlData = Engine.Razor.RunCompile(content, templateName, null, data);

            }
            else
                throw new Exception("The template does not exist");

            return htmlData;
        }
    }
}
