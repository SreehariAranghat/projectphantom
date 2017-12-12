using SupportPanda.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace SupportPanda.Web.Controllers
{
    public class BaseController : Controller
    {
        public BaseController()
        {
            if (Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                User currentUser        = Util.CurrentIdentity;
                ViewBag.DisplayName     = currentUser.FirstName;
                ViewBag.Id              = currentUser.Id;
                ViewBag.Image           = currentUser.PhotoPath;
            }
        }
    }
}