using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Text;
using System.Web.Mvc;
using SecurityGuard.Services;
using SecurityGuard.Interfaces;
using SecurityGuard.ViewModels;
using Mvc4WebRole.Controllers;

namespace Mvc4WebRole.Areas.SecurityGuard.Controllers
{
    [Authorize(Roles = "SecurityGuard")]
    public partial class RoleController : BaseController
    {
        private readonly IRoleService roleService;

        public RoleController()
        {
            roleService = new RoleService(roleService.Provider);
        }

        public virtual ActionResult Index()
        {
            var model = new ManageRolesViewModel();
            model.Roles = new SelectList(roleService.GetAllRoles());
            model.RoleList = roleService.GetAllRoles();

            return View(model);
        }

        [HttpGet]
        public virtual ActionResult CreateRole()
        {
            return View(new RoleViewModel());
        }

        [HttpPost]
        public virtual ActionResult CreateRole(string roleName)
        {
            var response = new JsonResponse();

            if (string.IsNullOrEmpty(roleName))
            {
                response.Success = false;
                response.Message = "You must enter a role name.";
                response.CssClass = "red";

                return Json(response);
            }

            try
            {
                roleService.CreateRole(roleName);

                if (Request.IsAjaxRequest())
                {
                    response.Success = true;
                    response.Message = "Role created successfully!";
                    response.CssClass = "green";

                    return Json(response);
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                if (Request.IsAjaxRequest())
                {
                    response.Success = false;
                    response.Message = ex.Message;
                    response.CssClass = "red";

                    return Json(response);
                }

                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// This is an Ajax method.
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual ActionResult DeleteRole(string roleName)
        {
            var response = new JsonResponse();

            if (string.IsNullOrEmpty(roleName))
            {
                response.Success = false;
                response.Message = "You must select a Role Name to delete.";
                response.CssClass = "red";

                return Json(response);
            }

            roleService.DeleteRole(roleName);

            response.Success = true;
            response.Message = roleName + " was deleted successfully!";
            response.CssClass = "green";

            return Json(response);
        }

        [HttpPost]
        public ActionResult DeleteRoles(string roles, bool throwOnPopulatedRole)
        {
            var response = new JsonResponse();
            response.Messages = new List<ResponseItem>();

            if (string.IsNullOrEmpty(roles))
            {
                response.Success = false;
                response.Message = "You must select at least one role.";
                return Json(response);
            }

            var roleNames = roles.Split(',');
            var sb = new StringBuilder();

            ResponseItem item = null;

            foreach (var role in roleNames)
            {
                if (!string.IsNullOrEmpty(role))
                {
                    try
                    {
                        roleService.DeleteRole(role, throwOnPopulatedRole);

                        item = new ResponseItem();
                        item.Success = true;
                        item.Message = "Deleted this role successfully - " + role;
                        item.CssClass = "green";
                        response.Messages.Add(item);
                    }
                    catch (ProviderException ex)
                    {
                        item = new ResponseItem();
                        item.Success = false;
                        item.Message = ex.Message;
                        item.CssClass = "yellow";
                        response.Messages.Add(item);
                    }
                }
            }

            response.Success = true;
            response.Message = sb.ToString();

            return Json(response);
        }

        /// <summary>
        /// This is an Ajax method that populates the 
        /// Roles drop down list.
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAllRoles()
        {
            var list = roleService.GetAllRoles();

            var selectList = new List<SelectObject>();

            foreach (var item in list)
            {
                selectList.Add(new SelectObject() { caption = item, value = item });
            }

            return Json(selectList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetUsersInRole(string roleName)
        {
            var list = roleService.GetUsersInRole(roleName);

            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}
