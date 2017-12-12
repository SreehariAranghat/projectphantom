using SupportPanda.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SupportPanda.Web.Controllers
{
    [Authorize(Roles = Permissions.PER_MANAGE_SETTINGS)]
    public class SettingsController : BaseController
    {
        public SettingsController() : base() { }
        // GET: Settings
        public ActionResult Index()
        {
            return View();
        }

    }
}