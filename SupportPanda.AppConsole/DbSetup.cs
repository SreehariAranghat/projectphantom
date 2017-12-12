using NHibernate.Cfg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportPanda
{
    public class DbSetup
    {
        public static void SetUp()
        {
            Configuration cfg = new NHibernate.Cfg.Configuration();
            cfg.Configure();

            NHibernate.Tool.hbm2ddl.SchemaExport schema = new NHibernate.Tool.hbm2ddl.SchemaExport(cfg);
            //schema.Create(true, false);

            schema.Execute(false, true, false);
            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}
