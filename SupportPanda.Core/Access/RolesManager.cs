using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportPanda.Core
{
    public class RolesManager : ControllerBase, IRolesManager
    {
        public void Delete(int id)
        {
            Role role = GetRole(id);

            if (role != null)
            {
                Repository.Delete<Role>(role);

                //Need to remove this role from all users;
                var sql = "DELETE FROM UserRoles WHERE RoleId = :RoleId";
                Dictionary<string, object> p = new Dictionary<string, object>();
                p.Add("RoleId", id);

                Repository.ExecuteUpdate(sql, p);
            }
            else
                throw new Exception("Role does not exist");
        }

        public List<Permission> GetPermissions()
        {
            return Repository.GetItems<Permission>();
        }

        public Role GetRole(int Id)
        {
            var expression      = PredicateBuilder.Create<Role>(d => ((d.IsSystemRole == true) || (d.Client.Id == Util.CurrentIdentity.Client.Id)) 
                                                                && d.Id == Id);
            Role r              = Repository.Get<Role>(expression);

            return r;
        }

        public List<Role> GetRoles()
        {
            List<Role> roles = null;
            var exp          = PredicateBuilder.Create<Role>(r => r.IsSystemRole == true || r.Client.Id == Util.CurrentIdentity.Client.Id);
            roles            = Repository.GetItems<Role>(exp);

            return roles;
        }

        public Role Save(Role role)
        {
            using (var benchmark = new Benchmark("rolemanager/save"))
            {
                Debug.WriteLine(string.Format("Attempting to same role {0}/{1}", role.Id, role.Name));

                if (role.Permissions.Count <= 0)
                    throw new Exception("Role requires atleast one permission");

                if (role.Id > 0)
                {
                    var curRole = GetRole(role.Id);

                    if (curRole == null)
                        throw new Exception("Invalid Role");

                    if (curRole.IsSystemRole)
                        throw new Exception("Invalid Role");
                    else
                        if (curRole.Client.Id != Util.CurrentIdentity.Client.Id)
                        throw new Exception("Invalid Role");


                    role.CreatedUser = curRole.CreatedUser;
                    role.CreatedDate = curRole.CreatedDate;
                }

                role.Client    = Util.CurrentIdentity.Client;
                var expression = PredicateBuilder.Create<Role>(d => d.Name.ToLower() == role.Name.ToLower()
                                                             && d.Id != role.Id
                                                             && (d.Client.Id == Util.CurrentIdentity.Client.Id || d.IsSystemRole == true));


                if (Repository.Any<Role>(expression))
                    throw new Exception("Role with the same name already exists");

                role = Repository.Save<Role>(role);

                Debug.WriteLine(string.Format("Role saved successfully {0}/{1}", role.Id, role.Name));
                Trace.WriteLine(string.Format("Role saved successfully {0}/{1}", role.Id, role.Name));
                Log.Info(string.Format("Role saved successfully {0}/{1}", role.Id, role.Name));

                return role;
            }
        }
    }
}
