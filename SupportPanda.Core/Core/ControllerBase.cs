using Microsoft.Practices.Unity;
using Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportPanda.Core
{
    abstract public class ControllerBase
    {
        ILogger                 logger;
        IRepository             repository;

        App app                 = App.Instance;
        MessengerHub hub        = MessengerHub.Instance;

        protected ILogger Log               { get => logger; }
        protected IRepository Repository    { get => repository; }
        protected App Application           { get => app; }
        protected MessengerHub MessageHub   { get => hub; }


        public ControllerBase() {

            this.logger     = UnityEntityContainer.Container.Resolve<ILogger>();
            this.repository = UnityEntityContainer.Container.Resolve<IRepository>();

        }
    }
}
