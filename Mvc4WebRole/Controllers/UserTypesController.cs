using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Dal;
using Otomotivist.Domain.UnitOfWork;
using Otomotivist.Domain.Repository;

namespace Mvc4WebRole.Controllers
{
    public class UserTypesController : BaseController
    {

        private readonly IUnitOfWork _uow;
        private readonly IGRepository<UserTypes> _userTypes;

        public UserTypesController(IUnitOfWork uow)
        {
            _uow = uow;
            _userTypes = _uow.GetRepository<UserTypes>();
        }

        public ViewResult Index()
        {
            return View(_userTypes.All().ToList());
        }

        public ViewResult Details(int id)
        {
            var usertype = _userTypes.Where(x => x.Id == id).FirstOrDefault();
            return View(usertype);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(UserTypes usertype)
        {
            if (ModelState.IsValid)
            {
                _userTypes.Insert(usertype);
                _uow.SavaChange();
                return RedirectToAction("Index");
            }

            return View(usertype);
        }
        public ActionResult Edit(int id)
        {
            var usertype = _userTypes.Where(x => x.Id == id).FirstOrDefault();
            return View(usertype);
        }

        [HttpPost]
        public ActionResult Edit(UserTypes usertype)
        {
            //if (ModelState.IsValid)
            //{
            //    context.Entry(usertype).State = EntityState.Modified;
            //    context.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            return View(usertype);
        }

        public ActionResult Delete(int id)
        {
            //var usertype = context.UserTypes.Single(x => x.Id == id);
            return View();
        }

        [HttpPost,
        ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            //var usertype = context.UserTypes.Single(x => x.Id == id);
            //context.UserTypes.Remove(usertype);
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
