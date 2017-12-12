using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyMessenger;

namespace SupportPanda.Core
{
    public class RegistrationCompletedMessage : TinyMessageBase
    {
        User user;

        public RegistrationCompletedMessage(object sender) : base(sender)
        {

        }

        public RegistrationCompletedMessage(object sender, User user) : base(sender)
        {
            Debug.WriteLine(String.Format("A new user has regisered with the system {0}", user.Id));
        }

        public User User { get => user; }
    }
}
