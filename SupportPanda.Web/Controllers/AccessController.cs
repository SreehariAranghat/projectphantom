using Newtonsoft.Json;
using SupportPanda.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;

namespace SupportPanda.Web.Controllers
{
    [OutputCache(NoStore = true, Duration = 0)]
    public class AccessController : Controller
    {
        IAuthenticationManager authManager;
        IAccessController accessController;

        public AccessController(IAuthenticationManager authManager
                               , IAccessController accessController)
        {
            this.authManager = authManager;
            this.accessController = accessController;
        }

        [HttpGet]
        public ActionResult Authenticate()
        {
            SignOut();
            return View();
        }

        [HttpPost]
        public ActionResult Authenticate(string username
                                       , string password)
        {
            if (!(string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)))
            {
                Core.User user = authManager.Authenticate(username, password);

                if (user != null)
                {
                    string userData = JsonConvert.SerializeObject(user);


                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                                                                    1,
                                                                    user.Id.ToString(),  //user id
                                                                    DateTime.Now,
                                                                    DateTime.Now.AddMinutes(30),  // expiry
                                                                    false,  //do not remember
                                                                    userData,
                                                                    "/");

                    HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName,
                                                                        FormsAuthentication.Encrypt(authTicket));
                    Response.Cookies.Add(cookie);
                    return RedirectPermanent("/");
                }
                else
                {
                    ViewBag.Error = "Login Failed";
                }
            }
            else
            {
                ViewBag.Error = "Username / Password Cannot be blank";
            }

            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegistrationRequest registrationRequest)
        {
            try
            {
                Core.User user = accessController.Register(registrationRequest);
                if (user != null)
                {
                    ViewBag.IsRegistered = true;
                    Util.SetCredentials(user);

                    string userData = JsonConvert.SerializeObject(user);


                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                                                                    1,
                                                                    user.Id.ToString(),  //user id
                                                                    DateTime.Now,
                                                                    DateTime.Now.AddMinutes(30),  // expiry
                                                                    false,  //do not remember
                                                                    userData,
                                                                    "/");

                    HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName,
                                                                        FormsAuthentication.Encrypt(authTicket));
                    Response.Cookies.Add(cookie);
                }
            }
            catch (Exception excp)
            {
                ViewBag.Error = excp.Message;
            }

            return View();
        }

        [HttpGet]
        public ActionResult Logout()
        {
            SignOut();

            return RedirectPermanent("~/access/authenticate");
        }

        [HttpGet]
        public ActionResult Verify(string id)
        {
            try
            {
                EmailVerificationRequest request = new EmailVerificationRequest();
                request.Token = id;
                accessController.VerifyUserEmail(request);
            }
            catch (Exception excp)
            {
                ViewBag.Error = excp.Message;
            }

            return View();
        }

        private void SignOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();

            // clear authentication cookie
            HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie1.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie1);

            // clear session cookie (not necessary for your current problem but i would recommend you do it anyway)
            SessionStateSection sessionStateSection = (SessionStateSection)WebConfigurationManager.GetSection("system.web/sessionState");
            HttpCookie cookie2 = new HttpCookie(sessionStateSection.CookieName, "");
            cookie2.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie2);

        }

        [HttpGet]
        public ActionResult PasswordReset()
        {
            SignOut();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult PasswordReset(string email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                try
                {
                    var isRequestSent = accessController.RequestPasswordReset(email);
                    ViewBag.IsPasswordLinkSent = isRequestSent;
                }
                catch (Exception excp)
                {
                    ViewBag.Error = excp.Message;
                }
            }
            else
            {
                ViewBag.Error = "Email adress cannot be blank";
            }
            return View();
        }

        [HttpGet]
        public ActionResult ResetPassword(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                ViewBag.IsTokenValid = accessController.IsPasswordRequestTokenValid(id);
                ViewBag.Id = id;
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult ResetPassword(string id
                                         , string newpassword
                                         , string confirmpassword)
        {
            ViewBag.Id = id;
            if (!string.IsNullOrEmpty(id))
            {
                ViewBag.IsTokenValid = accessController.IsPasswordRequestTokenValid(id);
                if (!(string.IsNullOrEmpty(newpassword) || string.IsNullOrEmpty(confirmpassword)))
                {
                    if (newpassword.Equals(confirmpassword))
                    {
                        PasswordResetRequest request = new PasswordResetRequest { Token = id, NewPassword = newpassword };

                        try
                        {
                            bool isResetSuccess = accessController.ResetPasswordFromToken(request);
                            ViewBag.PasswordResetSuccess = true;
                        }
                        catch (Exception excp)
                        {
                            ViewBag.Error = excp.Message;
                        }
                    }
                    else
                    {
                        ViewBag.Error = "New password and Confirm password does not match";
                    }
                }
                else
                {
                    ViewBag.Error = "New password or confirm password cannot be blank";
                }
            }

            return View();
        }
    }
}