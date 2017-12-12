using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportPanda.Core
{
    public interface IRolesManager
    {
        List<Permission> GetPermissions();

        List<Role> GetRoles();
        Role       GetRole(int Id);

        Role Save(Role role);
        void Delete(int roleid);
    }
}
