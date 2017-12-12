using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportPanda.Core
{
    public class ClientController : ControllerBase, IClientController
    {
        public ClientController()
        {

        }

        public Client Create(Client client)
        {
            Client c = null;

            if(client.IsValid())
            {
                c = Repository.Save<Client>(client);
                Debug.WriteLine("Created Client {0}", client.Id);
            }

            return c;
        }
    }
}
