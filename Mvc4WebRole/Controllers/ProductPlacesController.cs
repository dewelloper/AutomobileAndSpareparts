using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Dal;
using Otomotivist.Domain.UnitOfWork;
using Otomotivist.Domain.Repository;

namespace Mvc4WebRole.Controllers
{
    public class ProductPlacesController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IGRepository<ProductPlaces> _productPlaces;

        public ProductPlacesController(IUnitOfWork uow)
        {
            _uow = uow;
            _productPlaces = _uow.GetRepository<ProductPlaces>();
        }

        public ViewResult Index()
        {
            return View(_productPlaces.All().Select(productplace => productplace.Id).ToList());
        }

        public ViewResult Details(int id)
        {
            var productplace = _productPlaces.Where(x => x.Id == id).FirstOrDefault();
            return View(productplace);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(ProductPlaces productplace)
        {
            if (ModelState.IsValid)
            {
                _productPlaces.Insert(productplace);
                _uow.SavaChange();
                return RedirectToAction("Index");
            }

            return View(productplace);
        }
        public ActionResult Edit(int id)
        {
            var productplace = _productPlaces.Where(x => x.Id == id).FirstOrDefault();
            return View(productplace);
        }

        [HttpPost]
        public ActionResult Edit(ProductPlaces productplace)
        {
            //if (ModelState.IsValid)
            //{
                //context.Entry(productplace).State = EntityState.Modified;
                //context.SaveChanges();
                //return RedirectToAction("Index");
            //}
            return View(productplace);
        }

        public ActionResult Delete(int id)
        {
            //var productplace = context.ProductPlaces.Single(x => x.Id == id);
            return View();
        }

        [HttpPost,
        ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            //var productplace = context.ProductPlaces.Single(x => x.Id == id);
            //context.ProductPlaces.Remove(productplace);
            //context.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            //if (disposing)
            //{
            //    context.Dispose();
            //}
            base.Dispose(disposing);
        }
    }
}
