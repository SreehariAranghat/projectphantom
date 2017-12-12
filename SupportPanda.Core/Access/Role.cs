using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportPanda.Core
{
    public class Role : PandaObject
    {

        public string Name          { get; set; }
        public string Description   { get; set; }
        public bool IsSystemRole    { get; set; }

        [JsonIgnore]
        public Client Client { get; set; }

        ISet<Permission> permissions = new Iesi.Collections.Generic.LinkedHashSet<Permission>();
        public ISet<Permission> Permissions
        {
            get { return permissions; }
            set { permissions = value; }
        }
    }
}
