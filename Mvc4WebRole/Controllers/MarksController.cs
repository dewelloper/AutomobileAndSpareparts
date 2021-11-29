using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using EntityState = System.Data.Entity.EntityState;
using Dal;
using Otomotivist.Domain.UnitOfWork;
using Otomotivist.Domain.Repository;

namespace Mvc4WebRole.Controllers
{
    public class MarksController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IGRepository<Marks> _marks;

        public MarksController(IUnitOfWork uow)
        {
            _uow = uow;
            _marks = _uow.GetRepository<Marks>();
        }

        public ViewResult Index()
        {
            return View(_marks.All().Select(mark => mark.Id).ToList());
        }

        public ViewResult Details(int id)
        {
            var mark = _marks.Where(x => x.Id == id).FirstOrDefault();
            return View(mark);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Marks mark)
        {
            if (ModelState.IsValid)
            {
                _marks.Insert(mark);
                _uow.SavaChange();
                return RedirectToAction("Index");
            }

            return View(mark);
        }
        public ActionResult Edit(int id)
        {
            var mark = _marks.All().Select(x => x.Id == id);
            return View(mark);
        }

        [HttpPost]
        public ActionResult Edit(Marks mark)
        {
            if (ModelState.IsValid)
            {
                //context.Entry(mark).State = EntityState.Modified;
                //context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mark);
        }

        public ActionResult Delete(int id)
        {
            //var mark = context.Marks.Single(x => x.Id == id);
            return View();
        }

        [HttpPost,
        ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            //var mark = context.Marks.Single(x => x.Id == id);
            //context.Marks.Remove(mark);
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
