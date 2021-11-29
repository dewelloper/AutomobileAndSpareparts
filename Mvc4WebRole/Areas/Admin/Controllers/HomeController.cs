using Mvc4WebRole.Areas.Admin.Models;
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
    public class otomobilvasitaController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IGRepository<Products> _products;

        public otomobilvasitaController(IUnitOfWork uow)
        {
            _uow = uow;
            _products = _uow.GetRepository<Products>();
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
            List<Products> prds = _products.Where(k => k.Id == criteraIsId || k.Name.Contains(searchString) || k.Explanation.Contains(searchString)).ToList();

            if (searchString == null)
                prds = _products.Where(k => k.ProductType == 1).ToList();

            List<EditablePropertyOfProduct> props = prds.Select(k => new EditablePropertyOfProduct() { id = k.Id, IsActive = k.IsActive, Name = k.Name, CategoryId = k.Category_Id }).ToList();


            switch (sortOrder)
            {
                case "name_desc":
                    props = props.OrderByDescending(s => s.Name).ToList();
                    break;
                case "IsActive":
                    props = props.OrderBy(s => s.IsActive).ToList();
                    break;
                case "IsActive_desc":
                    props = props.OrderByDescending(s => s.IsActive).ToList();
                    break;
                default:  // Name ascending 
                    props = props.OrderBy(s => s.Name).ToList();
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(props.ToPagedList(pageNumber, pageSize));
        }

        // GET: Student/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products prd = _products.Where(k=> k.Id == id).FirstOrDefault();
            //EditablePropertyOfProduct props = new EditablePropertyOfProduct() { id = prd.Id, IsActive = prd.IsActive, Name = prd.Name, CategoryId = prd.Category_Id };
            if (prd == null)
            {
                return HttpNotFound();
            }
            return View(prd);
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
            var proToUpdate = _products.Where(k=> k.Id == id).FirstOrDefault();
            if (TryUpdateModel(proToUpdate, "",
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
            return View(proToUpdate);
        }

    }
}
