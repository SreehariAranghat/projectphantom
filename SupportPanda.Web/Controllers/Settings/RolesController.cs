using SupportPanda.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SupportPanda.Web.Controllers.Settings
{
    [Authorize(Roles = Permissions.PER_SECURITY_MANAGEROLES)]
    public class RolesController : BaseController
    {
        IRolesManager rolesManager;

        public RolesController(IRolesManager rolesManager) : base() {

            this.rolesManager = rolesManager;
        }

        public ActionResult Index()
        {
            List<Role> roles = rolesManager.GetRoles();
            return View(roles);
        }

        [HttpGet]
        public ActionResult New()
        {
            ViewBag.Permissions = rolesManager.GetPermissions();
            ViewBag.Groups      = rolesManager.GetPermissions().Select(d => d.GroupName).Distinct();

            return View();
        }

        [HttpPost]
        public ActionResult New(dynamic data)
        {
            Role role           = null;
            ViewBag.Permissions = rolesManager.GetPermissions();
            ViewBag.Groups      = rolesManager.GetPermissions().Select(d => d.GroupName).Distinct();

            try
            {
                string name         = Request.Form["Name"] != null ? Request.Form["Name"] : string.Empty;
                string description  = Request.Form["Description"] != null ? Request.Form["Description"] : string.Empty;
                string permissions  = Request.Form["Permission"] != null ? Request.Form["Permission"] : string.Empty;

                role                = new Role();
                role.Name           = name;
                role.Description    = description;
                role.IsSystemRole   = false;

                List<Permission> per = rolesManager.GetPermissions();
                string[] plist       = permissions.Split(',');

                if (plist.Length > 0)
                {
                    foreach (string p in plist)
                    {
                        int id;
                        if (!string.IsNullOrEmpty(p))
                        {
                            if (int.TryParse(p, out id))
                            {
                                Permission curPer = per.FirstOrDefault(d => d.Id == id);
                                if (curPer != null)
                                {
                                    role.Permissions.Add(curPer);
                                }
                            }
                        }
                    }
                }

                rolesManager.Save(role);
                return RedirectToAction("Index", new { s = role.Name });
            }
            catch(Exception excp)
            {
                ViewBag.Error = excp.Message;
            }

            return View(role);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id.HasValue)
            {
                ViewBag.Permissions = rolesManager.GetPermissions();
                ViewBag.Groups = rolesManager.GetPermissions().Select(d => d.GroupName).Distinct();

                Role role = rolesManager.GetRole(id.Value);
                if (role != null && (!role.IsSystemRole))
                    return View(role);
                else
                    return RedirectToAction("Index");
            }
            else
                return RedirectToAction("Index");
             
        }

        [HttpPost]
        public ActionResult Edit(dynamic data)
        {
            ViewBag.Permissions = rolesManager.GetPermissions();
            ViewBag.Groups      = rolesManager.GetPermissions().Select(d => d.GroupName).Distinct();
            Role role           = null;

            try
            {
                int roleid         = int.Parse(Request.Form["Id"]);
                string name        = Request.Form["Name"] != null ? Request.Form["Name"] : string.Empty;
                string description = Request.Form["Description"] != null ? Request.Form["Description"] : string.Empty;
                string permissions = Request.Form["Permission"] != null  ? Request.Form["Permission"] : string.Empty;

                role               = new Role();
                role.Id            = roleid;
                role.Name          = name;
                role.Description   = description;
                role.IsSystemRole  = false;

                List<Permission> per = rolesManager.GetPermissions();
                string[] plist       = permissions.Split(',');

                if (plist.Length > 0)
                {
                    foreach (string p in plist)
                    {
                        int id;
                        if (!string.IsNullOrEmpty(p))
                        {
                            if (int.TryParse(p, out id))
                            {
                                Permission curPer = per.FirstOrDefault(d => d.Id == id);
                                if (curPer != null)
                                {
                                    role.Permissions.Add(curPer);
                                }
                            }
                        }
                    }
                }

                rolesManager.Save(role);
                return RedirectToAction("Index", new { s = role.Name });
            }
            catch (Exception excp)
            {
                ViewBag.Error = excp.Message;
            }

            return View(role);
        }


        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            { 
                rolesManager.Delete(id);
            }
            catch(Exception excp)
            {
                return Json(new { Error = excp.Message.ToString() });
            }

            return Json(new { Message = "Role deleted successfully" });
        }
    }
}