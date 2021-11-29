using Mvc4WebRole.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Net;
using Dal;
using BusinessObjects;
using Otomotivist.Domain.UnitOfWork;
using Otomotivist.Domain.Repository;

namespace Mvc4WebRole.Areas.Admin.Controllers
{
    public class UserManagementController : BaseController
    {

        private readonly IUnitOfWork _uow;
        private readonly IGRepository<UserProfile> _userProfile;
        private readonly IGRepository<webpages_Membership> _webpages_Membership;

        public UserManagementController(IUnitOfWork uow)
        {
            _uow = uow;
            _userProfile = _uow.GetRepository<UserProfile>();
            _webpages_Membership = _uow.GetRepository<webpages_Membership>();
        }

        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.IsActiveParam = sortOrder == "IsActive" ? "IsActive_desc" : "IsActive";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            Int32 criteraIsId = 0;
            try
            {
                criteraIsId = Convert.ToInt32(searchString);
            }
            catch (Exception ex)
            { }
            List<Int32> userIds = _userProfile.Where(k => k.Name.Contains(searchString) || k.eMail.Contains(searchString)).Select(m => m.UserId).ToList();
            List<webpages_Membership> ms = _webpages_Membership.Where(k => k.UserId == criteraIsId || userIds.Contains(k.UserId)).ToList();

            List<UserMKnowledge> users = new List<UserMKnowledge>();

            if (searchString == null || searchString.Trim() == "")
            {
                users = (from u in _userProfile.All()
                         select new UserMKnowledge() { UserName = u.UserName, EMail = u.eMail, id = u.UserId }).ToList();
            }
            else
            { 
                users = (from u in _userProfile
                         where u.Name.Contains(searchString) || u.eMail.Contains(searchString)
                         select new UserMKnowledge() { UserName = u.UserName, EMail = u.eMail, id = u.UserId }).ToList();
            }
            foreach (UserMKnowledge uk in users)
            {
                webpages_Membership uw = _webpages_Membership.Where(k => k.UserId == uk.id).FirstOrDefault();
                uk.IsConfirmed = Convert.ToBoolean(uk.IsConfirmed);
            }


            switch (sortOrder)
            {
                case "name_desc":
                    users = users.OrderByDescending(s => s.UserName).ToList();
                    break;
                case "IsActive":
                    users = users.OrderBy(s => s.IsConfirmed).ToList();
                    break;
                case "IsActive_desc":
                    users = users.OrderByDescending(s => s.IsConfirmed).ToList();
                    break;
                default:  // Name ascending 
                    users = users.OrderBy(s => s.UserName).ToList();
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(users.ToPagedList(pageNumber, pageSize));
        }

        // GET: Student/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            webpages_Membership ms = _webpages_Membership.Where(k => k.UserId == id).FirstOrDefault();

            UserMKnowledge user = (from u in _userProfile
                                         where u.UserId == id
                                   select new UserMKnowledge() { UserName = u.UserName, EMail = u.eMail, id = u.UserId }).FirstOrDefault();
            if (user == null)
            {
                return HttpNotFound();
            }

            user.IsConfirmed = Convert.ToBoolean(ms.IsConfirmed);
            return View(user);
        }

        // POST: Student/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            webpages_Membership ms = _webpages_Membership.Where(k => k.UserId == id).FirstOrDefault();

            UserMKnowledge user = (from u in _userProfile
                                  where u.UserId == id
                                   select new UserMKnowledge() { UserName = u.UserName, EMail = u.eMail, id = u.UserId }).FirstOrDefault();
            user.IsConfirmed = Convert.ToBoolean(ms.IsConfirmed);

            if (TryUpdateModel(user, "",
               new string[] { "Name", "CategoryId", "IsActive" }))
            {
                try
                {
                    _uow.SavaChange();

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(user);
        }

    }
}
