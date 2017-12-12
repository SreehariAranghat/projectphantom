using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportPanda.Core
{
    public class Permission
    {
        public int Id               { get; set; }
        public string GroupName         { get; set; }
        public string Name          { get; set; }
        public string Title         { get; set; }
        public string Description   { get; set; }
    }
}
