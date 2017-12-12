using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportPanda.Core
{
    public class AgentCreateRequest
    {
        public string Name            { get; set; }
        public string Email           { get; set; }
        public string Mobile          { get; set; }
        public string WorkPhone       { get; set; }
        public string Signature       { get; set; }
    }
}
