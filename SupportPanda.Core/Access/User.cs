using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportPanda.Core
{
    public class User : PandaObject
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Mobile      { get; set; }
        public string PhoneNumber { get; set; }  

        public string PhotoPath   { get; set; }

        public string UniqueId    { get; set; }

        internal string Password  { get; set; }
        internal string Salt      { get; set; }

        public bool IsActive { get; set; }
        public DateTime? LastLoginTime { get; set; }

        public bool IsEmailVerified { get; set; }

        public Client Client { get; set; }

        ISet<Role> roles = new Iesi.Collections.Generic.LinkedHashSet<Role>();

        public string EmailSignature { get; set; }
   
        [JsonIgnore]
        public ISet<Role> Roles
        {
            get { return roles; }
            set { roles = value; }
        }

        List<string> permissions;

        public List<string> Permissions
        {
            get
            {
                if (permissions == null)
                {
                    permissions = new List<string>();
                    foreach (Role r in roles)
                    {
                        foreach (Permission p in r.Permissions)
                        {
                            if (!permissions.Contains(p.Name))
                                permissions.Add(p.Name);
                        }
                    }
                }

                return permissions;

            }
                 
        }

        public void SetTemporaryPassword()
        {
            this.Salt       = Guid.NewGuid().ToString().Substring(0, 5);
            this.Password  = HashProvider.CreateHash(this.Salt + Guid.NewGuid().ToString().Substring(0,5));
        }
    }
}
