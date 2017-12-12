using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SupportPanda.Core
{
    public class Util
    {
        public static int CurrentIdentityId
        {
            get
            {
                int i = 0;
                string userid = Thread.CurrentPrincipal.Identity.Name;

                if (!int.TryParse(userid, out i))
                    throw new Exception("Invalid user authentication id");

                return i;
            }
        }

        public static User CurrentIdentity
        {
            get
            {
                User user = null;

                if (!Thread.CurrentPrincipal.Identity.IsAuthenticated)
                    throw new Exception("Invalid User Authentication");

                AppIdentity identity = (AppIdentity)Thread.CurrentPrincipal.Identity;
                user                 = identity.CurrentUser;

                return user;
            }
        }

        public static void SetAnonymousCredentials()
        {
            User user = new User { Id = 1 };
            AppIdentity identity        = new AppIdentity("1", user);
            IPrincipal  principal       = new GenericPrincipal(identity, null);
            Thread.CurrentPrincipal     = principal;
        }

        public static void SetCredentials(User user)
        {
            AppIdentity identity       = new AppIdentity(user.Id.ToString(),user);
            IPrincipal principal       = new GenericPrincipal(identity, null);
            Thread.CurrentPrincipal    = principal;
        }
    }
}
