using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Dal;
using Otomotivist.Domain.UnitOfWork;
using Otomotivist.Domain.Repository;

namespace Mvc4WebRole.Controllers
{
    public class DenominationsController : BaseController
    {

        private readonly IUnitOfWork _uow;
        private readonly IGRepository<Denominations> _denominations;

        public DenominationsController(IUnitOfWork uow)
        {
            _uow = uow;
            _denominations = _uow.GetRepository<Denominations>();
        }

        public ViewResult Index()
        {
            return View(_denominations.All().Select(k=> k.Id).ToList());
        }

        public ViewResult Details(int id)
        {
            var denomination = _denominations.Where(x => x.Id == id);
            return View(denomination);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Denominations denomination)
        {
            if (ModelState.IsValid)
            {
                _denominations.Insert(denomination);
                _uow.SavaChange();
                return RedirectToAction("Index");
            }

            return View(denomination);
        }
        public ActionResult Edit(int id)
        {
            var denomination = _denominations.Where(x => x.Id == id);
            return View(denomination);
        }

        [HttpPost]
        public ActionResult Edit(Denominations denomination)
        {
            if (ModelState.IsValid)
            {
                _uow.SavaChange();
                return RedirectToAction("Index");
            }
            return View(denomination);
        }

        public ActionResult Delete(int id)
        {
            var denomination = _denominations.Where(x => x.Id == id);
            return View(denomination);
        }

        [HttpPost,
        ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            //var denomination = _denominations.Where(x => x.Id == id);
            //_denominations.Delete(denomination);
            //context.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
