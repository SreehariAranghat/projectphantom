using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace SupportPanda.Core
{
    public class AppIdentity : GenericIdentity
    {
        User currentUser;

        public AppIdentity(string name) : base(name)
        {
        }

        public AppIdentity(string name,User user) : base(name)
        {
            this.currentUser = user;
        }

        public User CurrentUser { get => currentUser; }
    }
}
