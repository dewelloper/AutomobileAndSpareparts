using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Dal;
using Otomotivist.Domain.UnitOfWork;
using Otomotivist.Domain.Repository;

namespace Mvc4WebRole.Controllers
{
    public class ProductGroupsController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IGRepository<ProductGroups> _productGroups;

        public ProductGroupsController(IUnitOfWork uow)
        {
            _uow = uow;
            _productGroups = _uow.GetRepository<ProductGroups>();
        }

        public ActionResult Index()
        {
            return View(_productGroups.All().ToList());
        }

        public ActionResult Details(int id = 0)
        {
            var productgroup = _productGroups.Where(k=> k.Id == id).FirstOrDefault();
            if (productgroup == null)
            {
                return HttpNotFound();
            }
            return View(productgroup);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductGroups productgroup)
        {
            if (ModelState.IsValid)
            {
                _productGroups.Insert(productgroup);
                _uow.SavaChange();
                return RedirectToAction("Index");
            }

            return View(productgroup);
        }

        public ActionResult Edit(int id = 0)
        {
            var productgroup = _productGroups.Where(k=> k.Id == id).FirstOrDefault();
            if (productgroup == null)
            {
                return HttpNotFound();
            }
            return View(productgroup);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductGroups productgroup)
        {
            if (ModelState.IsValid)
            {
                //context.Entry(productgroup).State = EntityState.Modified;
                //context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(productgroup);
        }

        public ActionResult Delete(int id = 0)
        {
            //var productgroup = _productGroups.Find(id);
            //if (productgroup == null)
            //{
            //    return HttpNotFound();
            //}
            return View();
        }

        [HttpPost,
        ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //var productgroup = context.ProductGroups.Find(id);
            //context.ProductGroups.Remove(productgroup);
            //context.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            //context.Dispose();
            base.Dispose(disposing);
        }
    }
}
