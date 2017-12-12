using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportPanda.Core
{
    public class AccessController : ControllerBase, IAccessController
    {
        IEmailService emailService;
        IClientController clientController;

        public AccessController(IEmailService emailService
                               ,IClientController clientController)
        {
            this.emailService       = emailService;
            this.clientController   = clientController;
        }

        public bool CreateEmailVerificationRequest(User user)
        {
            Debug.WriteLine("Creating Verification Token of user {0}", user.Id);

            VerificationToken token = new VerificationToken();

            token.IsVerified = false;
            token.Token      = Guid.NewGuid().ToString();
            token.User       = user;

            Repository.Save<VerificationToken>(token);

            Debug.WriteLine("Created verification token", token.ToString());
            Debug.WriteLine("Sending verification email for verification token {0}", token.Id);

            string subject = "Welcome to " + Application.GetSettings(SettingType.GENERAL).ApplicationName.ToString();
            emailService.Sent("WelcomeEmail", subject, user.Email, new
            {
                User    = user,
                Token   = token.Token,
                Domain  = Application.GetSettings(SettingType.GENERAL).RootDomain.ToString(),
                AppName = Application.GetSettings(SettingType.GENERAL).ApplicationName.ToString(),
            });

            return true;
        }

        public User GetUser(int id)
        {
            User user       = null;
            var expression  = PredicateBuilder.Create<User>(d => d.Id == id && d.Client.Id == Util.CurrentIdentity.Client.Id);
            user            = Repository.Get<User>(expression);

            return user;
        }

        public List<User> GetUsers()
        {
            List<User> users = new List<User>();

            var expression   = PredicateBuilder.Create<User>(d => d.Client.Id == Util.CurrentIdentity.Client.Id);
            users            = Repository.GetItems<User>(expression); 

            return users;
        }


        public bool RequestPasswordReset(string email)
        {
            bool isPasswordResetLinkSent = false;
            Util.SetAnonymousCredentials();

            var emailExp = PredicateBuilder.Create<User>(d => d.IsActive == true
                                         && d.DeletedDate == null
                                         && d.DeletedUser == null
                                         && d.Email.ToLower() == email.ToLower());

            var user = Repository.Get<User>(emailExp);

            if (user != null)
            {
                PasswordResetToken token = PasswordResetToken.Create(user);
                token = Repository.Save<PasswordResetToken>(token);

                //Sent confirmation mail to the suer
                string subject = "Action required: Reset your password";

                emailService.Sent("PasswordReset", subject, user.Email, new
                {
                    User    = user,
                    Token   = token,
                    Domain  = Application.GetSettings(SettingType.GENERAL).RootDomain.ToString()                           ,
                    AppName = Application.GetSettings(SettingType.GENERAL).ApplicationName.ToString()
                });

                isPasswordResetLinkSent = true;

            }
            else
            {
                throw new Exception("Sorry, But we could not find any records for the given email address.");
            }

            return isPasswordResetLinkSent;
        }


        public User Register(RegistrationRequest registrationRequest)
        {
            User user  = null;
            var val    = ValidationHelper.ValidateEntity<RegistrationRequest>(registrationRequest);

            Util.SetAnonymousCredentials();

            if(!val.HasError)
            {
                //Will check of the email  is already registered
                var exp = PredicateBuilder.Create<User>(d => d.Email     == registrationRequest.Email 
                                                        && d.DeletedDate == null 
                                                        && d.DeletedUser == null);

                if (!Repository.Any<User>(exp))
                {
                    Client client   = new Client();
                    client.Name     = registrationRequest.Client;
                    client.UniqueId = Guid.NewGuid().ToString();
                    client.Name     = registrationRequest.Client;

                    client          = clientController.Create(client);

                    user            = new User();
                    user.UserName   = registrationRequest.Email;
                    user.FirstName  = registrationRequest.FirstName;
                    user.LastName   = registrationRequest.LastName;
                    user.Salt       = Guid.NewGuid().ToString().Substring(0, 5);
                    user.Password   = HashProvider.CreateHash(user.Salt + registrationRequest.Password);
                    user.PhotoPath  = "../img/profile_small.jpg";
                    user.Email      = registrationRequest.Email;
                    user.Client     = client;
                    user.UniqueId   = Guid.NewGuid().ToString();
                    user.IsActive   = true;

                    //Make the user an administrator
                    Role adminRole  = Repository.Get<Role>(1);
                    user.Roles.Add(adminRole);

                    user            = Repository.Save<User>(user);

                    if (user != null)
                    {
                        Log.Info(String.Format("USER CREATED : ID:{0}", user.Id));
                        Debug.WriteLine(String.Format("New user created {0}", user.Id));

                        RegistrationCompletedMessage registrationMessage = new RegistrationCompletedMessage(this, user);
                        MessageHub.Publish<RegistrationCompletedMessage>(registrationMessage);

                        Util.SetCredentials(user);

                        //Sent User Email Verification
                        CreateEmailVerificationRequest(user);
                    }
                }
                else
                    throw new Exception("User with the same email address already exist");
            }
            else
            {
                throw new ValidationException("Error while validating registration request",val.Errors);
            }

            return user;
        }

        public bool VerifyUserEmail(EmailVerificationRequest verificationRequest)
        {
            bool isVerified = false;

            if (verificationRequest != null)
            {
                Util.SetAnonymousCredentials();
                var requestExpression   = PredicateBuilder.Create<VerificationToken>(t => t.Token == verificationRequest.Token
                                                                                 && t.IsVerified == false);

                VerificationToken token = Repository.Get<VerificationToken>(requestExpression);

                if (token != null)
                {
                    token.IsVerified    = true;
                    token.VerifiedDate  = DateTime.UtcNow;

                    Repository.Save<VerificationToken>(token);
                    isVerified          = true;
                }
                else
                    throw new Exception("Verification token is invalid or expired");
            }

            return isVerified;
        }

        public bool ResetPasswordFromToken(PasswordResetRequest resetRequest)
        {
            bool isPasswordReset = false;

            Debug.WriteLine("Attempt to reset password for token {0}", resetRequest.Token);

            if(resetRequest.IsValid())
            {
                Debug.WriteLine("Token is verified to be valid {0}", resetRequest.Token);

                var exp = PredicateBuilder.Create<PasswordResetToken>(t => t.Token.ToLower() == resetRequest.Token.ToLower() 
                                                                      && t.IsActive == true);


                PasswordResetToken token = Repository.Get<PasswordResetToken>(exp);

                if(token != null)
                {
                    if (DateTime.UtcNow.Subtract(token.CreatedDate).TotalHours <= 24)
                    {
                        
                        User user       = token.User;
                        user.Salt       = Guid.NewGuid().ToString().Substring(0, 5);
                        user.Password   = HashProvider.CreateHash(user.Salt + resetRequest.NewPassword);

                        Util.SetCredentials(user);

                        Repository.Save<User>(user);

                        //Make the token inactive to prevent repate usage
                        token.IsActive = false;
                        Repository.Save<PasswordResetToken>(token);

                        Log.Info(String.Format("Password reset successfully for token {0}/userid:{1}", token.Token, user.Id));
                    }
                    else
                    {
                        Log.Error(String.Format("Attempt to reset password using expired token {0}", resetRequest.Token));
                        throw new Exception("Expired reset token");
                    }
                    
                }
                else
                {
                    var excp = new Exception("Invalid token or token has already expired");
                    Log.Fatal("Invalid Password Reset Token", excp);
                    throw excp;
                }
                    
            }

            return isPasswordReset;
        }

        public bool IsPasswordRequestTokenValid(string token)
        {
            bool isPasswordRequestValid = false;

            var exp = PredicateBuilder.Create<PasswordResetToken>(t => t.Token.ToLower() == token.ToLower()
                                                                      && t.IsActive == true);


            PasswordResetToken resetToken = Repository.Get<PasswordResetToken>(exp);

            if (resetToken != null)
            {
                if (DateTime.UtcNow.Subtract(resetToken.CreatedDate).TotalHours <= 24)
                {
                    isPasswordRequestValid = true;
                }
            }

            return isPasswordRequestValid;
        }
    }
}
