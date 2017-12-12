using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyMessenger;

namespace SupportPanda.Core
{
    public class MessengerHub
    {
        private static MessengerHub instance = null;
        private static readonly object padlock = new object();

        TinyMessengerHub hub;

        MessengerHub()
        {
            hub = new TinyMessengerHub();
        }

        public void Publish<T>(T t) where T : TinyMessageBase
        {
            Hub.Publish<T>(t);
        }

        public static MessengerHub Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new MessengerHub();
                    }
                    return instance;
                }
            }
        }

        public TinyMessengerHub Hub { get => hub; }
    }
}
