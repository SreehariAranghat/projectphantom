using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportPanda.Core
{
    public interface IAccessController
    {
        User Register(RegistrationRequest registrationRequest);

        bool CreateEmailVerificationRequest(User user);
        bool VerifyUserEmail(EmailVerificationRequest verificationRequest);

        bool RequestPasswordReset(string email);
        bool ResetPasswordFromToken(PasswordResetRequest resetRequest);
        bool IsPasswordRequestTokenValid(string token);

        List<User> GetUsers();
        User GetUser(int id);
    }
}
