using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportPanda.Core
{
    public class PasswordResetRequest
    {
        public string Token { get; set; }
        public string NewPassword { get; set; }

        public bool IsValid()
        {
            if (!(string.IsNullOrEmpty(Token)))
            {
                if (!string.IsNullOrEmpty(NewPassword))
                {
                    if (NewPassword.Length <= 6)
                        throw new Exception("Password must be atleast 6 characters.");
                }
                else
                    throw new Exception("Password cannot be blank");
            }
            else
                throw new Exception("Invalid password reset request");
                

            return true;
        }
    }
}
