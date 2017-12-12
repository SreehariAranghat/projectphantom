using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportPanda.Core
{
    public class AuthenticationManager : ControllerBase, IAuthenticationManager
    {
        public User Authenticate(string username, string password)
        {
            User user = null;

            if (!(string.IsNullOrEmpty(username)
                || string.IsNullOrEmpty(password)))
            {
                var expression = PredicateBuilder.Create<User>(d => d.UserName.ToLower() == username.ToLower()
                                                                 && d.DeletedDate == null
                                                                 && d.DeletedUser == null
                                                                 && d.IsActive    == true);

                Util.SetAnonymousCredentials();

                var u = Repository.Get<User>(expression);

                if (u != null)
                {
                    string hashPass = HashProvider.CreateHash(u.Salt + password);

                    if (String.Equals(u.Password, hashPass))
                        user = u;
                    else
                    {
                        Log.Error(String.Format("Authentication Failed for {0}", username));
                    }
                }
                else
                    Log.Error(String.Format("Authentication Failed for {0}", username));

            }
            else
                throw new Exception("User Name or Password cannot be blank");

            return user;
        }
    }
}
