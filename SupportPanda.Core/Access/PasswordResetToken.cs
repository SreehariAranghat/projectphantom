using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportPanda.Core
{
    public class PasswordResetToken : PandaObject
    {
        public User User        { get; set; }
        public string Token     { get; set; }
        public bool IsActive    { get; set; }

        public static PasswordResetToken Create(User user)
        {
            PasswordResetToken resetToken = new PasswordResetToken();

            resetToken.User               = user;
            resetToken.Token              = Guid.NewGuid().ToString();
            resetToken.IsActive           = true;

            return resetToken;
        }
    }
}
