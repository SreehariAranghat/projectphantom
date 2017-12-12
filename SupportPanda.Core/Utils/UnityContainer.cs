using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity.Configuration;
using Unity;

namespace SupportPanda.Core
{
    public class UnityEntityContainer
    {
        private static App instance = null;
        private static readonly object padlock = new object();

        static IUnityContainer container;

        public static IUnityContainer Container
        {
            get
            {
                lock (padlock)
                {
                    if (container == null)
                        InitContainer();

                    return container;
                }    
            }
        }

        private static void InitContainer()
        {
            container = new UnityContainer();
            container.LoadConfiguration();
        }
    }
}
