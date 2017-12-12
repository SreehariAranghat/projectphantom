using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportPanda.Core
{
    public class VerificationToken : PandaObject
    {
        public string Token { get; set; }
        public bool IsVerified { get; set; }
        public User User { get; set; }
        public DateTime? VerifiedDate { get; set; }

    }
}
