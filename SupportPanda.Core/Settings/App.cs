using Newtonsoft.Json;
using Microsoft.Practices.Unity;
using Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportPanda.Core
{
    public class App
    {
        private static App instance             = null;
        private static readonly object padlock  = new object();

        static List<Setting> settings = null;
        static IRepository repository;
        static ILogger logger;


        App()
        {
            logger      = UnityEntityContainer.Container.Resolve<ILogger>();
            repository  = UnityEntityContainer.Container.Resolve<IRepository>();

            if (logger != null && repository != null)
            {
                settings = repository.Get<Setting>();
            }
        }

        public static App Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new App();
                    }
                    return instance;
                }
            }
        }

        public dynamic GetSettings(SettingType type)
        {
            dynamic data = default(dynamic);

            if (settings != null)
            {
                Setting s = settings.FirstOrDefault(d => d.Name == type.ToString());
                data      = JsonConvert.DeserializeObject(s?.Value);
            }

            return data;
        }
    }
}
