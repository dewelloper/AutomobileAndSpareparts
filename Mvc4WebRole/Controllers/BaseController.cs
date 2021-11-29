using System.Web.Mvc;
using System;
using System.Linq;
using System.Collections.Generic;
using Dal;
using BusinessObjects;
using Otomotivist.Domain.UnitOfWork;
using Otomotivist.Domain.Repository;

namespace Mvc4WebRole.Controllers
{
    public class BaseController : Controller
    {

        public AccountInformation GetAccountInformation()
        {
            //if (ViewData["AccountInformation"] != null)
            //{
            //    return ViewData["AccountInformation"] as AccountInformation;
            //}
            return null;
        }
    }
}
