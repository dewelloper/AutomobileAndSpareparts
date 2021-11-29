using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using Bytescout.Watermarking;
using Bytescout.Watermarking.Presets;
using Color = System.Drawing.Color;
using System.Web.UI;
using System.Web.Helpers;
using System.Text.RegularExpressions;
using System.Linq.Expressions;
using Dal;
using BusinessObjects;
using Otomotivist.Domain.UnitOfWork;
using Otomotivist.Service.interfaces;
using Otomotivist.Domain.Repository;
using Mvc4WebRole.App_Start;
using Mvc4WebRole.Filters;

namespace Mvc4WebRole.Controllers
{
    public class otomobilvasitaController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IOtomotivistCoreService _ocs;


        public otomobilvasitaController(IUnitOfWork uow, IOtomotivistCoreService otomotivistCoreService)
        {
            _uow = uow;
            _ocs = otomotivistCoreService;
        }


        //[OutputCache(Duration = 3600, VaryByParam = "none", Location = OutputCacheLocation.Client, NoStore = true)]
        //[Compress]
        //[OtomotivistCacheFilter(Duration = 60)]
        public ActionResult yeniikinciel()
        {
            var prdcs = _ocs.NewAutomobilesTop30().ToList();
            prdcs.AddRange(_ocs.NewSparepartsTop30().ToList());

            Debug.Assert(HttpContext.Session != null, "HttpContext.Session != null");
            List<UserMessages> umesgs = new List<UserMessages>();
            if (HttpContext.Session["AccountInformation"] != null && (HttpContext.Session["AccountInformation"] as AccountInformation).UserMessages.Count > 0)
            {
                umesgs = (HttpContext.Session["AccountInformation"] as AccountInformation).UserMessages;
            }

            ViewBag.Title = "Otomobil, Yedek Parça, sahibinden en uygun en ergonomik otomobil";
            ViewBag.Description = "otomotivist istanbul başta olmak üzere Türkiyede otomotiv sektörünün öncüsüdür oto alım satım kira servis yedek parça ilanları sunar toptan en uygun en ucuz ürünleri son kullanıcıya taşır";
            Response.AddHeader("Expires", "Thu, 01 Dec 2014 16:00:00 GMT");

            return View(new otomobilvasitaViewModel
            {
                UserMessages = umesgs,
                //listpG = context.ProductGroups.ToList(),
                listCatG = _ocs.GetCategoriesAll().ToList(),
                //listMarks = context.Marks.ToList(),
                products = prdcs,
                //currencies = context.Currencies.ToList(),
                OrderBasket = (DataTable)HttpContext.Session["basket"],
                cities = _ocs.GetCitiesAll().ToList(),
                listFuelTypes = _ocs.GetFuelTypesAll().ToList(),
                listCaseTypes = _ocs.GetCaseTypesAll().ToList(),
                listGearTypes = _ocs.GetGearTypesAll().ToList(),
                ViewType = "yeniikinciel",
                FormName = "YeniIkinciElOtomobiller"
            });
        }

        public JsonResult ProductGroupList(int id)
        {
            var c2 = _ocs.GetProductroupById(id);
            return Json(new SelectList(c2.ToArray(), "Id", "ProductName"), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ProductGroupList2(int id)
        {
            var c2 = _ocs.GetProductroupById(id);
            return Json(new SelectList(c2.ToArray(), "Id", "ProductName"), JsonRequestBehavior.AllowGet);
        }

        public JsonResult CategoryListSpareParts(int id)
        {
            if (id == 1)
            {
                var cats = _ocs.GetCategoriesById(id);
                return Json(new SelectList(cats.ToArray(), "Id", "Name"), JsonRequestBehavior.AllowGet);
            }
            else
            {
                var c2 = _ocs.GetCategoriesByIdIfProductExist(id);
                return Json(new SelectList(c2.ToArray(), "Id", "Name"), JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult CategoryList(int id)
        {
            var categories = _ocs.GetCategoriesById(id);

            return Json(new SelectList(categories.ToArray(), "Id", "Name"), JsonRequestBehavior.AllowGet);
        }

        public JsonResult TownList(int id)
        {
            var towns = _ocs.GetTownsById(id);
            return Json(new SelectList(towns.ToArray(), "Id", "Name"), JsonRequestBehavior.AllowGet);
        }

        public JsonResult SubDistrictList(int id)
        {
            var c2 = _ocs.GetSubdistrictsById(id);
            return Json(new SelectList(c2.ToArray(), "Id", "Name"), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EntryView()
        {
            return View();
        }

        [WhitespaceFilter]
        [OtomotivistCacheFilter(Duration = 60)]
        public ActionResult ProfileView(bool isOwner = true)
        {
            var uid = 0;
            if (Session["UserIdInSession"] != null && isOwner)
            {
                uid = Convert.ToInt32(Session["UserIdInSession"]);
                UpdateUserProfileInfo(uid, true);
            }

            Debug.Assert(HttpContext.Session != null, "HttpContext.Session != null");
            if (HttpContext.Session["AccountInformation"] != null)
            {
                return View("ProfileView", HttpContext.Session["AccountInformation"]);
            }
            return RedirectToAction("Login", "Account");
        }

        public void UpdateUserProfileInfo(int userId, bool isEditible)
        {
            var up = _ocs.GetUserProfileById(userId);
            int? uuid = (int?)up.UserId;
            var ua = _ocs.GetUserAddressesByUserId(up.UserId).ToList();
            List<UserMessages> umessages = new List<UserMessages>();
            List<UserFeedbacks> ufeed = new List<UserFeedbacks>();
            if (isEditible)
            {
                umessages = _ocs.GetUserMessagesById(Convert.ToInt32(uuid)).ToList();
                ViewData["UserMessages"] = umessages;
                Session["UserMessages"] = umessages;
                ufeed = _ocs.GetUserUserFeedbacksById(Convert.ToInt32(uuid)).ToList();
                Session["UserFeedBacks"] = ufeed;
            }
            var ai = new AccountInformation
            {
                UserFeedBacks = ufeed,
                UserMessages = umessages,
                UserId = up.UserId,
                UserName = up.UserName,
                UserRealName = up.Name,
                UserSurname = up.Surname,
                UserProfilePhoto = up.ProfilePhoto,
                UserGender = up.Gender,
                UserEducationLevel = _ocs.GetUserEducationLevelsById(up.EducationLevel).FirstOrDefault().EducationLevel,
                UserJob = _ocs.GetUserJobsById(up.Job).FirstOrDefault().Job,
                UserGenderList = _ocs.GetUserGenders().ToList(),
                UserEducationLevelList = _ocs.GetUserEducationLevels().ToList(),
                UserJobList = _ocs.GetUserJobs().ToList(),
                UserCellPhone = up.GSM,
                UserHomePhone = up.HomePhone,
                UserWorkPhone = up.WorkPhone,
                UserFaxNumber = up.Fax,
                UserTcId = up.TCid,
                UserBirthDate = up.DateOfBirth != null ? Convert.ToDateTime(up.DateOfBirth) : DateTime.Now
            };
            ai.UserProfilePhoto = up.ProfilePhoto;
            ai.UserEmail = up.eMail;
            ai.UserTypeId = Convert.ToInt32(up.UserTypeId ?? 0);
            var uas = _ocs.Getautoservices(up.UserId).ToList();
            ai.UserAutoServices = uas;
            var ugs = _ocs.GetGalleries(up.UserId).ToList();
            ai.UserGalleries = ugs;
            var ords = _ocs.GetOrders(up.UserId).ToList();
            ai.UserOrders = ords;

            var prdcs = _ocs.GetProducts(Convert.ToInt32(uuid)).ToList();
            ai.UserProducts = prdcs;
            var prdcGrps = _ocs.GetProductGroups().ToList();
            ai.PrdcGrpList = prdcGrps;
            var prdcCats = _ocs.GetCategories().ToList();
            ai.PrdcCatList = prdcCats;
            var prdcMarks = _ocs.GetMarks().ToList();
            ai.PrdcMarkList = prdcMarks;
            var prdcCurrs = _ocs.GetCurrencies().ToList();
            ai.PrdcCurrList = prdcCurrs;
            ai.Adress = ua;
            if (!isEditible)
            {
                ai.UserCellPhone = "05...";
            }

            if (ua.Count == 1)
            {
                ai.SelectedAdress = ua[0];
            }
            HttpContext.Session["AccountInformation"] = ai;
        }

        [WhitespaceFilter]
        [OtomotivistCacheFilter(Duration = 60)]
        public ActionResult otomotivisthakkinda()
        {
            ViewBag.Message = "Hakkımızda.";

            return View();
        }

        [WhitespaceFilter]
        [OtomotivistCacheFilter(Duration = 60)]
        public ActionResult kullanicisozlesmesi()
        {
            ViewBag.Message = "Hakkımızda.";

            return View();
        }

        [WhitespaceFilter]
        [OtomotivistCacheFilter(Duration = 60)]
        public ActionResult gizlilik()
        {
            ViewBag.Message = "Hakkımızda.";

            return View();
        }

        [WhitespaceFilter]
        [OtomotivistCacheFilter(Duration = 60)]
        public ActionResult sozlesmehaklar()
        {
            ViewBag.Message = "Hakkımızda.";

            return View();
        }

        [WhitespaceFilter]
        [OtomotivistCacheFilter(Duration = 60)]
        public ActionResult iletisim()
        {
            ViewBag.Message = "Bizimle iletişime geçin!.";

            return View();
        }

        [WhitespaceFilter]
        [AuthorizeActionFilterAttribute]
        public ActionResult ilankategorisi()
        {
            return View();
        }

        [WhitespaceFilter]
        public ActionResult otomobilyedekparcaservisilangirisi(int callerId)
        {
            if (Request.IsAuthenticated)
            {
                Products prod = new Products();
                if (Session["pro"] != null)
                {
                    prod = Session["pro"] as Products;
                }

                return View(new otomobilyedekparcaservisilangirisiViewModel
                {
                    listpG = _ocs.GetProductGroupsAll().ToList(),
                    listCatG = _ocs.GetCategoriesAll().ToList(),
                    listMarks = _ocs.GetMarksAll().ToList(),
                    denominationList = _ocs.GetDenominationsAll().ToList(),
                    prStateList = _ocs.GetProductStatesAll().ToList(),
                    product = prod,
                    listCurrencies = _ocs.GetCurrenciesAll().ToList(),
                    productType = callerId,
                    listCities = _ocs.GetCitiesAll().ToList(),
                    listCaseTypes = _ocs.GetCaseTypesAll().ToList(),
                    listColors = _ocs.GetColorsAll().ToList(),
                    listDamageStates = _ocs.GetDamageStatesAll().ToList(),
                    listFuelTypes = _ocs.GetFuelTypesAll().ToList(),
                    listModelYears = _ocs.GetModelYearsAll().ToList(),
                    listEnginePowers = _ocs.GetEnginePowersAll().ToList(),
                    listEngineVolumes = _ocs.GetEngineVolumesAll().ToList(),
                    listGearTypes = _ocs.GetGearTypesAll().ToList(),
                    listVehicleTypes = _ocs.GetVehicleTypesAll().ToList(),
                    listPlateNationalities = _ocs.GetPlateNationalitiesAll().ToList(),
                    listTractionTypes = _ocs.GetTractionTypesAll().ToList(),
                    listGuarantySituations = _ocs.GetGuarantySituationsAll().ToList(),
                    listPublishDurations = _ocs.GetPublishDurationsAll().ToList(),
                    listProductSellers = _ocs.GetProductSellersAll().ToList()
                });
            }
            TempData["LoginMessage"] = "*ilan Vermek İçin Giriş Yapın ya da Kaydolun!";
            return RedirectToAction("Login", "Account");
        }

        [WhitespaceFilter]
        [AcceptVerbs(HttpVerbs.Post)]

        [AuthorizeActionFilterAttribute]
        public ActionResult EnterProduct(Products product, IEnumerable<HttpPostedFileBase> images)
        {
            if (!ModelState.IsValid)//redirected because doesnt know right viewname because same view changed to showing inputs
            {
                if (product != null)
                    Session["pro"] = product;

                return RedirectToAction("otomobilyedekparcaservisilangirisi", new { CallerId = product.ProductType });
            }

            if (product.Name == null)
            {
                product.Name = string.Empty;
            }
            if (product.Explanation == null)
            {
                product.Explanation = string.Empty;
            }
            if (product.MarkId == null)
            {
                product.MarkId = 1;
            }
            if (product.GroupId == null)
            {
                product.GroupId = 12;
            }
            if (product.CategoryId == null)
            {
                product.CategoryId = 10;
            }
            if (product.Quantity == null)
            {
                product.Quantity = 1;
            }

            var waterMarker = new Watermarker();

            waterMarker.InitLibrary("demo", "demo");
            var filePathOriginal = Server.MapPath("/Images/ProductImages/");

            const string path = "/Images/ProductImages/";
            var j = 0;
            foreach (var imagefile in images)
            {
                j++;
                if (imagefile == null || imagefile.FileName == null)
                {
                    continue;
                }

                //Image img = Imager.Crop(Image.FromStream(imagefile.InputStream), new Rectangle(0, 0, 960, 800));
                //Image resized = Imager.Resize(img, 960, 800, true);

                WebImage img = new WebImage(imagefile.InputStream);
                if (img.Width > 1023)
                    img.Resize(1024, 800);
                else if (img.Width > 799)
                    img.Resize(800, 640);
                else if (img.Width > 639)
                    img.Resize(640, 518);
                else
                    img.Resize(img.Width, img.Height);

                img.Save(filePathOriginal + imagefile.FileName);

                //imagefile.SaveAs(filePathOriginal + imagefile.FileName);
                var savedFileName = Path.Combine(filePathOriginal, imagefile.FileName);

                var inputFilePath = savedFileName;
                var wmFileName = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "_" + DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" + DateTime.Now.Second + "-" + DateTime.Now.Millisecond + "_" + imagefile.FileName;
                var outputFilePath = filePathOriginal + wmFileName;

                img.Resize(100, 81); //for thumbnails
                img.Save(filePathOriginal + "/thumbnails/otomobililanlari" + wmFileName);

                waterMarker.AddInputFile(inputFilePath, outputFilePath);

                var preset = new TextFitsPageDiagonal
                {
                    Text = "otomotivist.com",
                    Font = new WatermarkFont("Tahoma", FontStyle.Regular, FontSizeType.Points, 20),
                    TextColor = Color.Black,
                    Transparency = 75,
                    Orientation = DiagonalOrientation.FromBottomLeftToTopRight
                };

                waterMarker.AddWatermark(preset);

                waterMarker.OutputOptions.OutputDirectory = outputFilePath;

                waterMarker.Execute();

                if (j == 1)
                {
                    product.ImagePath0 = path + wmFileName;
                }
                if (j == 2)
                {
                    product.ImagePath1 = path + wmFileName;
                }
                if (j == 3)
                {
                    product.ImagePath2 = path + wmFileName;
                }
                if (j == 4)
                {
                    product.ImagePath3 = path + wmFileName;
                }
                if (j == 5)
                {
                    product.ImagePath4 = path + wmFileName;
                }
                if (j == 6)
                {
                    product.ImagePath5 = path + wmFileName;
                }
                if (j == 7)
                {
                    product.ImagePath6 = path + wmFileName;
                }
                if (j == 8)
                {
                    product.ImagePath7 = path + wmFileName;
                }
                if (j == 9)
                {
                    product.ImagePath8 = path + wmFileName;
                }
                if (j == 10)
                {
                    product.ImagePath9 = path + wmFileName;
                }
                if (j == 11)
                {
                    product.ImagePath10 = path + wmFileName;
                }
            }

            product.RegistererId = _ocs.GetUserProfileByName(User.Identity.Name).UserId;
            product.RecordDate = DateTime.Now;
            _ocs.InsertProduct(product);
            _uow.SavaChange();
                TempData["EnterPrdcMessage"] = string.Empty;
                return RedirectToAction("yeniikinciel");
        }

        [AuthorizeActionFilterAttribute]
        public ActionResult InsertToOrderBasket(string id, string name, string price, string quantity, string productGrupId, string productGrup, string productCatg, string productMark, string productCurr)
        {
            try
            {
                var table = new DataTable();

                Debug.Assert(HttpContext.Session != null, "HttpContext.Session != null");
                if ((HttpContext.Session["basket"] != null) &&
                    (((DataTable)HttpContext.Session["basket"]).Columns.Count > 0))
                {
                    table = (DataTable)HttpContext.Session["basket"];
                }
                else
                {
                    table.Columns.Add("id");
                    table.Columns.Add("Parça Adı");
                    table.Columns.Add("Miktarı");
                    table.Columns.Add("Birim Fiyatı");
                    table.Columns.Add("productGrupId");
                    table.Columns.Add("productGrup");
                    table.Columns.Add("productCatg");
                    table.Columns.Add("productMark");
                    table.Columns.Add("productCurr");
                }

                if (!Control(id))
                {
                    var row = table.NewRow();
                    row[0] = id;
                    row[1] = name;
                    row[2] = quantity;
                    row[3] = price;
                    row[4] = productGrupId;
                    row[5] = productGrup;
                    row[6] = productCatg;
                    row[7] = productMark;
                    row[8] = productCurr;
                    table.Rows.Add(row);
                }
                else
                {
                    Increase(id, Convert.ToInt32(quantity));
                }
                HttpContext.Session["basket"] = table;
            }

            catch (Exception e)
            {
                if (HttpContext.Session != null)
                {
                    HttpContext.Session["Error"] = e.Message;
                }
            }

            Debug.Assert(HttpContext.Session != null, "HttpContext.Session != null");
            if (HttpContext.Session["qry"] == null)
            {
                Debug.Assert(HttpContext.Request.UrlReferrer != null, "HttpContext.Request.UrlReferrer != null");
                return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
            }
            var url = HttpContext.Session["qry"].ToString();
            return Redirect(url);
        }

        private bool Control(string id)
        {
            Debug.Assert(HttpContext.Session != null, "HttpContext.Session != null");
            if (HttpContext.Session["basket"] != null)
            {
                var table = (DataTable)HttpContext.Session["basket"];
                for (var i = 0; i < table.Rows.Count; i++)
                {
                    if (table.Rows[i]["id"].ToString() == id)
                    {
                        return true;
                    }
                }
                return false;
            }
            return false;
        }

        [AuthorizeActionFilterAttribute]
        private void Increase(string id, int quantity)
        {
            try
            {
                Debug.Assert(HttpContext.Session != null, "HttpContext.Session != null");
                var table = (DataTable)HttpContext.Session["basket"];
                for (var i = 0; i < table.Rows.Count; i++)
                {
                    if (table.Rows[i]["id"].ToString() == id)
                    {
                        table.Rows[i]["Miktarı"] = (Convert.ToInt32((table.Rows[i]["Miktarı"].ToString())) + quantity);
                        HttpContext.Session["basket"] = table;
                        return;
                    }
                }
            }
            catch (Exception e)
            {
                if (HttpContext.Session != null)
                {
                    HttpContext.Session["Error"] = e.Message;
                }
            }
        }

        [AuthorizeActionFilterAttribute]
        public ActionResult DeleteBasketItem(string id)
        {
            if (HttpContext.Session != null && HttpContext.Session["basket"] == null)
            {
                return RedirectToAction("_OrderBasketView", "Order");
            }
            if (HttpContext.Session == null)
            {
                return RedirectToAction("_OrderBasketView", "Order");
            }
            var table = (DataTable)HttpContext.Session["basket"];
            for (var i = 0; i < table.Rows.Count; i++)
            {
                if (table.Rows[i]["id"].ToString() != id)
                {
                    continue;
                }
                table.Rows[i].Delete();
                HttpContext.Session["basket"] = table;
            }

            if ((table).Rows.Count == 0)
            {
                HttpContext.Session["basket"] = null;
                return RedirectToAction("yeniikinciel", "otomobilvasita");
            }
            return RedirectToAction("_OrderBasketView", "Order");
        }

        [AuthorizeActionFilterAttribute]
        public ActionResult CleanBasket()
        {
            Debug.Assert(HttpContext.Session != null, "HttpContext.Session != null");
            HttpContext.Session["basket"] = null;
            return RedirectToAction("yeniikinciel", "otomobilvasita");
        }

        [AuthorizeActionFilterAttribute]
        public ActionResult UpdateBasket()
        {
            return RedirectToAction("_OrderBasketView", "Order");
        }

        [WhitespaceFilter]
        [Compress]
        [OtomotivistCacheFilter(Duration = 60)]
        public ActionResult GeneralSearch(string searchTextIdSparepartGeneral, string searchType)
        {
            if (searchTextIdSparepartGeneral == "1")
            {
                List<Products> pros = Session["BeforeProducts"] as List<Products>;
                return View(new otomobilvasitaViewModel
                {
                    listpG = _ocs.GetProductGroupsAll().OrderBy(k => k.ProductName).ToList(),
                    listCatG = _ocs.GetCategoriesAll().ToList(),
                    listMarks = _ocs.GetMarksAll().ToList(),
                    currencies = _ocs.GetCurrenciesAll().ToList(),
                    products = pros,
                    cities = _ocs.GetCitiesAll().ToList(),
                    listFuelTypes = _ocs.GetFuelTypesAll().ToList(),
                    listCaseTypes = _ocs.GetCaseTypesAll().ToList(),
                    listGearTypes = _ocs.GetGearTypesAll().ToList(),
                    FormName = "GenelAramaSonuclari"
                });
            }

            if (searchTextIdSparepartGeneral == "İlan başlığı ya da açıklamasında")
            {
                searchTextIdSparepartGeneral = string.Empty;
            }
            var trimmedSerchText = searchTextIdSparepartGeneral.TrimStart().TrimEnd();
            var criteria = trimmedSerchText.Split(' ');

            List<Products> tempPro = _ocs.GetProductByCriteria(trimmedSerchText).ToList();

            if (tempPro.Count == 0)
            {
                ViewBag.SearchMsg = "Belirttiğiniz kriterlere uyan bir sonuç bulamadık, arama kriterlerinizi değiştirip tekrar deneyin.";
            }
            else
            {
                ViewBag.SearchMsg = tempPro.Count + " adet sonuç bulundu.";
            }

            List<Products> lpd2 = tempPro.Take(3).ToList();
            string exp = "";
            foreach (Products p in lpd2)
                exp += p.Name + ", " + p.Code;

            List<Products> prods = tempPro.OrderByDescending(m => m.Id).ToList();
            Session["BeforeProducts"] = tempPro;
            if (tempPro.Count > 0)
            {
                ViewBag.Title = exp;
                Categories catt = null;
                string catname = "";
                if (prods[0].CategoryId != null)
                {
                    Int64 cidd = Convert.ToInt64(prods[0].CategoryId);// Int64 takes direct id Int32 according parentId
                    catt = _ocs.GetCategoriesById(cidd).FirstOrDefault();
                    catname = catt.Name;
                }
                ViewBag.Description = catname + "/" + prods[0].Name;
                Cities city = null;
                string cityName = "";
                if (prods[0].City != null)
                {
                    int cityid = Convert.ToInt32(prods[0].City);
                    city = _ocs.GetCityById(cityid);
                    cityName = city.Name;
                }

                if (city != null)
                    cityName = city.Name + ",";
                ViewBag.Keywords = cityName + catname + "," + exp;
            }

            return View(new otomobilvasitaViewModel
            {
                listpG = _ocs.GetProductGroupsAll().OrderBy(k => k.ProductName).ToList(),
                listCatG = _ocs.GetCategoriesAll().ToList(),
                listMarks = _ocs.GetMarksAll().ToList(),
                currencies = _ocs.GetCurrenciesAll().ToList(),
                products = prods,
                cities = _ocs.GetCitiesAll().ToList(),
                listFuelTypes = _ocs.GetFuelTypesAll().ToList(),
                listCaseTypes = _ocs.GetCaseTypesAll().ToList(),
                listGearTypes = _ocs.GetGearTypesAll().ToList(),
                FormName = "GenelAramaSonuclari"
            });
        }

        [WhitespaceFilter]
        [Compress]
        public ActionResult SearchProduct(string catagoryId, [DefaultValue(null)] string markId, string searchType, [DefaultValue(null)] string hdnInpSearch2)
        {
            if (catagoryId == "-1")
            {
                List<Products> pros = Session["BeforeProducts"] as List<Products>;
                return View(new otomobilvasitaViewModel
                {
                    listpG = _ocs.GetProductGroupsAll().OrderBy(k => k.ProductName).ToList(),
                    listCatG = _ocs.GetCategoriesAll().ToList(),
                    listMarks = _ocs.GetMarksAll().ToList(),
                    currencies = _ocs.GetCurrenciesAll().ToList(),
                    products = pros,
                    cities = _ocs.GetCitiesAll().ToList(),
                    listFuelTypes = _ocs.GetFuelTypesAll().ToList(),
                    listCaseTypes = _ocs.GetCaseTypesAll().ToList(),
                    listGearTypes = _ocs.GetGearTypesAll().ToList(),
                    FormName = "OtomobilAramaSonuclari"
                });
            }

            var products = new List<Products>();
            if (hdnInpSearch2 == null)
            {
                hdnInpSearch2 = string.Empty;
            }
            hdnInpSearch2 = hdnInpSearch2.TrimStart().TrimEnd();

            if (hdnInpSearch2 == "Parça kodu, adı ya da açıklamasında" || hdnInpSearch2 == "İlan başlığı ya da açıklamasında" || hdnInpSearch2 == "Hizmet adı ya da açıklamasında")
            {
                hdnInpSearch2 = string.Empty;
            }

            var criteria = hdnInpSearch2.Split(' ');
            var crit = criteria[0].ToString();
            var qry = string.Empty;

            if (catagoryId != null && markId != "null" && markId != "Seçim yapınız." && crit == string.Empty)
            {
                if (HttpContext.Request.Url != null)
                {
                    qry = HttpContext.Request.Url.AbsoluteUri + "?CatagoryId=" + catagoryId + "&MarkId=" + markId + "&hdnInpSearch=" + hdnInpSearch2;
                }
                var marId = Convert.ToInt64(markId);
                var fChilds = _ocs.GetCategoriesById(marId).Select(m => m.Id).ToList();
                var types1 = fChilds.Select(fc => (int?)fc).ToList();
                var sChilds = _ocs.GetCategoriesAll().Where(k => types1.Contains(k.ParentId)).Select(m => m.Id).ToList();
                var types2 = sChilds.Select(sc => (int?)sc).ToList();
                var tChilds = _ocs.GetCategoriesAll().Where(k => types2.Contains(k.ParentId)).Select(m => m.Id).ToList();
                var types3 = tChilds.Select(tc => (int?)tc).ToList();
                types3.Add(Convert.ToInt32(marId));
                products = _ocs.GetProductsByTypeAndContainerCategory(2, types3).ToList();
            }
            else
            {
                if (crit != string.Empty)
                {
                    if (HttpContext.Request.Url != null)
                    {
                        qry = HttpContext.Request.Url.AbsoluteUri + "?CatagoryId=" + catagoryId + "&hdnInpSearch=" + hdnInpSearch2;
                    }
                    products = _ocs.GetProductsByTypeAndCriteriaCode(2,crit).ToList();
                }
                else
                {
                    if (HttpContext.Request.Url != null)
                    {
                        qry = HttpContext.Request.Url.AbsoluteUri + "?CatagoryId=" + catagoryId;
                    }
                    products = _ocs.GetProductsByType(2).ToList();
                }
            }

            Session["BeforeProducts"] = products;
            List<Products> prods = products.OrderByDescending(m => m.Id).ToList();

            List<Products> lpd2 = products.Take(3).ToList();
            string exp = "";
            foreach (Products p in lpd2)
                exp += p.Name + ", " + p.Code;

            if (products.Count > 0)
            {
                ViewBag.Title = exp;
                Categories catt = null;
                string catname = "";
                if (prods[0].CategoryId != null)
                {
                    Int64 cidd = Convert.ToInt64(prods[0].CategoryId);
                    catt = _ocs.GetCategoriesById(cidd).FirstOrDefault();
                    catname = catt.Name;
                }
                ViewBag.Description = catname + "/" + prods[0].Name;
                Cities city = null;
                string cityName = "";
                if (prods[0].City != null)
                {
                    city = _ocs.GetCityById(Convert.ToInt32(prods[0].City));
                    cityName = city.Name;
                }

                if (city != null)
                    cityName = city.Name + ",";
                ViewBag.Keywords = cityName + catname + "," + exp;
            }
            return View(new otomobilvasitaViewModel
            {
                listpG = _ocs.GetProductGroupsAll().OrderBy(k => k.ProductName).ToList(),
                listCatG = _ocs.GetCategoriesAll().ToList(),
                listMarks = _ocs.GetMarksAll().ToList(),
                currencies = _ocs.GetCurrenciesAll().ToList(),
                products = prods,
                cities = _ocs.GetCitiesAll().ToList(),
                listFuelTypes = _ocs.GetFuelTypesAll().ToList(),
                listCaseTypes = _ocs.GetCaseTypesAll().ToList(),
                listGearTypes = _ocs.GetGearTypesAll().ToList(),
                FormName = "AracAramaSonuclari"
            });
        }

        public ActionResult aracozellikleri(int prdcId, string GroupId, string CategoryId, string PriceCurrencyId, string Name, string Code)
        {
            if (HttpContext.Session["pdvm"] != null)
            {
                PrdcDetailViewModel pvm = HttpContext.Session["pdvm"] as PrdcDetailViewModel;
                HttpContext.Session["pdvm"] = null;
                return View(pvm); 
            }

            var product = _ocs.GetProductsById(prdcId);

            var owner = _ocs.GetUserProfileById(Convert.ToInt32(product.RegistererId));
            var prdcOwnerName = owner.Name +" " + owner.Surname;
            var prdcOwnerPhone = owner.GSM;

            Debug.Assert(product != null, "product != null");
            if (product.MarkId == null)
            {
                product.MarkId = 1;
            }
            TempData["currentproductGroup"] = GroupId;
            TempData["currentproductCat"] = CategoryId;
            TempData["currentproductCurr"] = PriceCurrencyId;

            var prdcCity = string.Empty;
            var prdcTown = string.Empty;
            var prdcDistrict = string.Empty;
            if (product.City != null && product.City != 0)
            {
                var firstOrDefault = _ocs.GetCityById(Convert.ToInt32(product.City));
                if (firstOrDefault != null)
                {
                    prdcCity = firstOrDefault.Name;
                }
            }
            if (product.Region != null && product.Region != 0)
            {
                var firstOrDefault = _ocs.GetTownsById(Convert.ToInt32(product.Region)).FirstOrDefault();
                if (firstOrDefault != null)
                {
                    prdcTown = firstOrDefault.Name;
                }
            }
            if (product.District != null && product.District != 0)
            {
                var firstOrDefault = _ocs.GetSubdistrictsById(Convert.ToInt32(product.District)).FirstOrDefault();
                if (firstOrDefault != null)
                {
                    prdcDistrict = firstOrDefault.Name;
                }
            }
            var prdcCaseTyp = string.Empty;
            var prdcColor = string.Empty;
            var prdcEngineCap = string.Empty;
            var prdcEnginePow = string.Empty;
            var prdcFuelTyp = string.Empty;
            var prdcGearTyp = string.Empty;
            var prdcSeller = string.Empty;
            var prdcVehicleTyp = string.Empty;
            if (product.CaseType != null)
            {
                var firstOrDefault = _ocs.GetCaseTypesById(Convert.ToInt32(product.CaseType));
                if (firstOrDefault != null)
                {
                    prdcCaseTyp = firstOrDefault.Name;
                }
            }
            if (product.Color != null)
            {
                var firstOrDefault = _ocs.GetColorsById(Convert.ToInt32(product.Color));
                if (firstOrDefault != null)
                {
                    prdcColor = firstOrDefault.Name;
                }
            }
            if (product.EngineCapacity != null)
            {
                var firstOrDefault = _ocs.GetEngineVolumesById(Convert.ToInt32(product.EngineCapacity));
                if (firstOrDefault != null)
                {
                    prdcEngineCap = firstOrDefault.Name;
                }
            }
            if (product.EnginePower != null)
            {
                prdcEnginePow = _ocs.GetEnginePowersById(Convert.ToInt32(product.EnginePower)).Name;
            }
            if (product.FuelType != null)
            {
                prdcFuelTyp = _ocs.GetFuelTypesById(Convert.ToInt32(product.FuelType)).Name;
            }
            if (product.GearType != null)
            {
                prdcGearTyp = _ocs.GetGearTypesById(Convert.ToInt32(product.GearType)).Name;
            }
            if (product.ProductSeller != null)
            {
                prdcSeller = _ocs.GetProductSellersById(Convert.ToInt32(product.ProductSeller)).Name;
            }
            if (product.VehicleType != null)
            {
                prdcVehicleTyp = _ocs.GetVehicleTypesById(Convert.ToInt32(product.VehicleType)).Name;
            }

            var prdcCatsArr = _ocs.FindCategories(product.CategoryId);
            var prdcCats = prdcCatsArr.Aggregate(string.Empty, (current, t) => current + t + " » ");
            prdcCats = prdcCats.Substring(0, prdcCats.Length - 2);

            if (product.CurrentPrice != null)
                product.CurrentPrice = Convert.ToDecimal(product.CurrentPrice.ToString().Replace(",00", "").Replace(".00", ""));
            else product.CurrentPrice = 0;

            ViewBag.Title = product.Name;
            if (product != null)
            {
                ViewBag.Title = product.Name;
                Categories catt = null;
                string catname = "";
                if (product.CategoryId != null)
                {
                    Int32 cidd = Convert.ToInt32(product.CategoryId);
                    catt = _ocs.GetCategoriesByCategoryId(cidd);
                    catname = catt.Name;
                }
                ViewBag.Description = catname + "/" + product.Name;
                Cities city = null;
                string cityName = "";
                if (product.City != null)
                {
                    city = _ocs.GetCityById(Convert.ToInt32(product.City));
                    cityName = city.Name;
                }

                if (city != null)
                    cityName = city.Name + ",";
                ViewBag.Keywords = cityName + catname + "," + product.Name;
            }


            List<string> pg = _ocs.GetProductGroupsAll().Select(k => k.ProductName).Distinct().ToList();
            Products prd = _ocs.GetProductsById(prdcId);
            Int32 pgId = Convert.ToInt32(prd.GroupId);
            ProductGroups pGroup = _ocs.GetProductroupById(pgId).FirstOrDefault();
            Categories pCat = _ocs.GetCategoriesById(Convert.ToInt64(prd.CategoryId)).FirstOrDefault();
            Currencies curr = _ocs.GetCurrenciesById(Convert.ToInt32(prd.PriceCurrencyId));
            //if (Name != Name.ToSeoUrl() || GroupId != pGroup.ProductName.ToSeoUrl() || CategoryId != pCat.Name.ToSeoUrl() || PriceCurrencyId != curr.Name.ToSeoUrl())
            //{
                PrdcDetailViewModel pdvm = new PrdcDetailViewModel
                {
                    ProductGroups = pg,
                    selectedProduct = product,
                    denominationList = _ocs.GetDenominationsAll().ToList(),
                    prdcGroup = FindGroups(product.GroupId),
                    prdcCat = prdcCats,
                    prdcMark = _ocs.GetMarksAll().Where(x => x.Id == product.MarkId).FirstOrDefault().MarkName,
                    prdcCurrency = _ocs.GetCurrenciesById(Convert.ToInt32(product.PriceCurrencyId)).Name,
                    prdcCaseTyp = prdcCaseTyp,
                    prdcColor = prdcColor,
                    prdcEngineCap = prdcEngineCap,
                    prdcEnginePow = prdcEnginePow,
                    prdcFuelTyp = prdcFuelTyp,
                    prdcGearTyp = prdcGearTyp,
                    prdcSeller = prdcSeller,
                    prdcVehicleTyp = prdcVehicleTyp,
                    prdcOwnerName = prdcOwnerName,
                    prdcOwnerPhone = prdcOwnerPhone,
                    prdcCity = prdcCity,
                    prdcTown = prdcTown,
                    prdcDistrict = prdcDistrict
                };
                HttpContext.Session["pdvm"] = pdvm;
                return RedirectToActionPermanent("aracozellikleri", new
                {
                    prdcId = prd.Id,
                    GroupId = pGroup.ProductName.ToSeoUrl(),
                    CategoryId = pCat.Name.ToSeoUrl(),
                    PriceCurrencyId = curr.Name.ToSeoUrl(),
                    Name = prd.Name.ToSeoUrl(),
                    Code = prd.Code,
                });
           // }

            return View();
        }

        [WhitespaceFilter]
        [HttpGet]
        public ActionResult aracozellikleriduzenle(int prdcId, string productCatIds, string productGroupIds)
        {
            var product = _ocs.GetProductsById(prdcId);

            var prdcCats = new int[] { };
            var prdcGrps = new int[] { };
            if (product != null)
            {
                prdcCats = FindCategoryIds(product.CategoryId);
                prdcGrps = FindGroupIds(product.GroupId);
            }

            if (product != null)
            {
                return View(new otomobilyedekparcaservisilangirisiViewModel
                {
                    product = product,
                    listCatG = _ocs.GetCategoriesAll().ToList(),
                    listCurrencies = _ocs.GetCurrenciesAll().ToList(),
                    denominationList = _ocs.GetDenominationsAll().ToList(),
                    listpG = _ocs.GetProductGroupsAll().OrderBy(k => k.ProductName).ToList(),
                    listMarks = _ocs.GetMarksAll().ToList(),
                    prStateList = _ocs.GetProductStatesAll().ToList(),
                    productType = product.ProductType,
                    listTractionTypes = _ocs.GetTractionTypesAll().ToList(),
                    listPublishDurations = _ocs.GetPublishDurationsAll().ToList(),
                    listProductSellers = _ocs.GetProductSellersAll().ToList(),
                    listPlateNationalities = _ocs.GetPlateNationalitiesAll().ToList(),
                    listVehicleTypes = _ocs.GetVehicleTypesAll().ToList(),
                    listGuarantySituations = _ocs.GetGuarantySituationsAll().ToList(),
                    listCities =_ocs.GetCitiesAll().ToList(),
                    listCaseTypes = _ocs.GetCaseTypesAll().ToList(),
                    listColors = _ocs.GetColorsAll().ToList(),
                    listDamageStates = _ocs.GetDamageStatesAll().ToList(),
                    listFuelTypes =_ocs.GetFuelTypesAll().ToList(),
                    listModelYears = _ocs.GetModelYearsAll().OrderBy(k => k.Value).ToList(),
                    listEnginePowers = _ocs.GetEnginePowersAll().ToList(),
                    listEngineVolumes = _ocs.GetEngineVolumesAll().ToList(),
                    listGearTypes = _ocs.GetGearTypesAll().ToList(),
                    PrdcCats = prdcCats,
                    ProductGroupIds = prdcGrps
                });
            }
            return null;
        }

        public ActionResult DeactiveProduct(int prdcId)
        {
            var firstOrDefault = _ocs.GetProductsById(prdcId);
            if (firstOrDefault != null)
            {
                firstOrDefault.IsActive = false;
            }
            _uow.SavaChange();

            return HttpContext.Request.UrlReferrer != null ? Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri) : null;
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Savearacozellikleriduzenle(Products product, IEnumerable<HttpPostedFileBase> images)
        {
            var productEntity = _ocs.GetProductsById(Convert.ToInt32(product.Id));

            if (productEntity.ImagePath0 != null)
            {
                product.ImagePath0 = productEntity.ImagePath0;
            }
            if (productEntity.ImagePath1 != null)
            {
                product.ImagePath1 = productEntity.ImagePath1;
            }
            if (productEntity.ImagePath2 != null)
            {
                product.ImagePath2 = productEntity.ImagePath2;
            }
            if (productEntity.ImagePath3 != null)
            {
                product.ImagePath3 = productEntity.ImagePath3;
            }
            if (productEntity.ImagePath4 != null)
            {
                product.ImagePath4 = productEntity.ImagePath4;
            }
            if (productEntity.ImagePath5 != null)
            {
                product.ImagePath5 = productEntity.ImagePath5;
            }
            if (productEntity.ImagePath6 != null)
            {
                product.ImagePath6 = productEntity.ImagePath6;
            }
            if (productEntity.ImagePath7 != null)
            {
                product.ImagePath7 = productEntity.ImagePath7;
            }
            if (productEntity.ImagePath8 != null)
            {
                product.ImagePath8 = productEntity.ImagePath8;
            }
            if (productEntity.ImagePath9 != null)
            {
                product.ImagePath9 = productEntity.ImagePath9;
            }
            if (productEntity.ImagePath10 != null)
            {
                product.ImagePath10 = productEntity.ImagePath10;
            }

            var waterMarker = new Watermarker();

            waterMarker.InitLibrary("demo", "demo");
            var filePathOriginal = Server.MapPath("/Images/ProductImages/");

            const string path = "/Images/ProductImages/";
            var j = 0;
            var prdcImages = images as HttpPostedFileBase[] ?? images.ToArray();
            foreach (var pfwm in prdcImages)
            {
                j++;
                if (pfwm == null || pfwm.FileName == null)
                {
                    continue;
                }
                pfwm.SaveAs(filePathOriginal + pfwm.FileName);
                var savedFileName = Path.Combine(filePathOriginal, pfwm.FileName);

                var inputFilePath = savedFileName;
                var wmFileName = "wm_" + DateTime.Now.Ticks + "_" + pfwm.FileName;
                var outputFilePath = filePathOriginal + wmFileName;

                waterMarker.AddInputFile(inputFilePath, outputFilePath);

                var preset = new TextFitsPageDiagonal
                {
                    Text = "otomotivist.com",
                    Font = new WatermarkFont("Tahoma", FontStyle.Regular, FontSizeType.Points, 30),
                    TextColor = Color.Black,
                    Transparency = 85,
                    Orientation = DiagonalOrientation.FromBottomLeftToTopRight
                };

                waterMarker.AddWatermark(preset);

                waterMarker.OutputOptions.OutputDirectory = outputFilePath;

                waterMarker.Execute();

                if (j == 1)
                {
                    product.ImagePath0 = path + wmFileName;
                }
                if (j == 2)
                {
                    product.ImagePath1 = path + wmFileName;
                }
                if (j == 3)
                {
                    product.ImagePath2 = path + wmFileName;
                }
                if (j == 4)
                {
                    product.ImagePath3 = path + wmFileName;
                }
                if (j == 5)
                {
                    product.ImagePath4 = path + wmFileName;
                }
                if (j == 6)
                {
                    product.ImagePath5 = path + wmFileName;
                }
                if (j == 7)
                {
                    product.ImagePath6 = path + wmFileName;
                }
                if (j == 8)
                {
                    product.ImagePath7 = path + wmFileName;
                }
                if (j == 9)
                {
                    product.ImagePath8 = path + wmFileName;
                }
                if (j == 10)
                {
                    product.ImagePath9 = path + wmFileName;
                }
                if (j == 11)
                {
                    product.ImagePath10 = path + wmFileName;
                }
            }

            if (product.Barcode == null && productEntity.Barcode != null)
            {
                product.Barcode = productEntity.Barcode;
            }
            if (product.CategoryId != productEntity.CategoryId)
            {
                product.CategoryId = productEntity.CategoryId;
            }
            if (product.Code == null && productEntity.Code != null)
            {
                product.Code = productEntity.Code;
            }
            //if (product.CurrentPrice != productEntity.CurrentPrice)
            //{
            //    product.CurrentPrice = productEntity.CurrentPrice;
            //}
            if (product.DenominationId != productEntity.DenominationId)
            {
                product.DenominationId = productEntity.DenominationId;
            }
            if (product.DenominationId != productEntity.DenominationId)
            {
                product.DenominationId = productEntity.DenominationId;
            }
            if (product.EqualId != productEntity.EqualId)
            {
                product.EqualId = productEntity.EqualId;
            }
            if (product.Explanation == null && productEntity.Explanation != null)
            {
                product.Explanation = productEntity.Explanation;
            }
            if (product.FeedCount != productEntity.FeedCount)
            {
                product.FeedCount = productEntity.FeedCount;
            }
            if (product.GroupId != productEntity.GroupId)
            {
                product.GroupId = productEntity.GroupId;
            }
            if (product.Id != productEntity.Id)
            {
                product.Id = productEntity.Id;
            }

            if (product.IsActive != productEntity.IsActive)
            {
                product.IsActive = productEntity.IsActive;
            }
            if (product.LastAccessDate != productEntity.LastAccessDate)
            {
                product.LastAccessDate = productEntity.LastAccessDate;
            }
            if (product.MarkId != productEntity.MarkId)
            {
                product.MarkId = productEntity.MarkId;
            }
            if (product.ModelYear == null && productEntity.ModelYear != null)
            {
                product.ModelYear = productEntity.ModelYear;
            }
            if (product.ModifierId != productEntity.ModifierId)
            {
                product.ModifierId = productEntity.ModifierId;
            }
            if (product.Name == null && productEntity.Name != null)
            {
                product.Name = productEntity.Name;
            }
            if (product.OfferedPrice != productEntity.OfferedPrice)
            {
                product.OfferedPrice = productEntity.OfferedPrice;
            }
            if (product.PriceCurrencyId != productEntity.PriceCurrencyId)
            {
                product.PriceCurrencyId = productEntity.PriceCurrencyId;
            }
            if (product.ProductState_Id != productEntity.ProductState_Id)
            {
                product.ProductState_Id = productEntity.ProductState_Id;
            }
            if (product.ProductType != productEntity.ProductType)
            {
                product.ProductType = productEntity.ProductType;
            }
            if (product.Quantity != productEntity.Quantity)
            {
                product.Quantity = productEntity.Quantity;
            }
            if (product.RecordDate != productEntity.RecordDate)
            {
                product.RecordDate = productEntity.RecordDate;
            }
            if (product.RegistererId != productEntity.RegistererId)
            {
                product.RegistererId = productEntity.RegistererId;
            }
            if (product.SpecialCode == null && productEntity.SpecialCode != null)
            {
                product.SpecialCode = productEntity.SpecialCode;
            }
            if (product.StatusId != productEntity.StatusId)
            {
                product.StatusId = productEntity.StatusId;
            }
            if (product.SubProductId != productEntity.SubProductId)
            {
                product.SubProductId = productEntity.SubProductId;
            }

            //context.Entry(productEntity).CurrentValues.SetValues(product);
            _uow.SavaChange();

            return RedirectToAction("aracozellikleri", "otomobilvasita", new
            {
                prdcId = product.Id,
                productGroup = TempData["currentproductGroup"],
                productCat = TempData["currentproductCat"],
                productCurr = TempData["currentproductCurr"]
            });
        }

        public ActionResult EditUserProfile(UserProfile usrProfile, string Address, HttpPostedFileBase ProfilePhoto)
        {
            var userName = User.Identity.Name;
            var userId = _ocs.GetUserProfileByName(userName).UserId;
            var userEntity = _ocs.GetUserProfileById(userId);
            var usrAddressId = _ocs.GetUserAddressesByUserId(userId).FirstOrDefault().Id;
            var userAddressEntity = _ocs.GetUserAddressesById(usrAddressId).FirstOrDefault();
            var newUserAddressEntity = userAddressEntity;
            newUserAddressEntity.Address = Address;
            usrProfile.UserName = userEntity.UserName;
            usrProfile.UserId = userId;

            if (ProfilePhoto != null)
            {
                var fullPath = Path.Combine(Server.MapPath("/Images/UserProfileImages/"), Path.GetFileName(ProfilePhoto.FileName));
                const string path2 = "/Images/UserProfileImages/";
                ProfilePhoto.SaveAs(fullPath);
                usrProfile.ProfilePhoto = path2 + ProfilePhoto.FileName;
            }
            else
            {
                usrProfile.ProfilePhoto = userEntity.ProfilePhoto;
            }

            //context.Entry(userAddressEntity).CurrentValues.SetValues(newUserAddressEntity);
            //context.Entry(userEntity).CurrentValues.SetValues(usrProfile);
            //context.SaveChanges();
            _uow.SavaChange();

            UpdateUserProfileInfo(userId, true);

            return RedirectToAction("ProfileView", "otomobilvasita");
        }


        int marId = 0;
        List<int?> types1 = new List<int?>();
        List<int?> types2 = new List<int?>();
        List<int?> types3 = new List<int?>();
        string crit = "";
        [Compress]
        [WhitespaceFilter]
        public ActionResult SearchProductVehicle(string CatagoryId, string MarkId, string ModelId, string otomobilvasitaCat6,
            string productFuelType, string productCaseType, string productGearType, string fader1, string fader2, string fader3, string fader4, string startPrice, string endPrice,
        string hdnInpSearch)
        {
            if (CatagoryId == "-1")
            {
                List<Products> prdcs = Session["BeforeProducts"] as List<Products>;
                return View(new otomobilvasitaViewModel
                {
                    listpG = _ocs.GetProductGroupsAll().OrderBy(k=> k.ProductName).ToList(),
                    listCatG = _ocs.GetCategoriesAll().OrderBy(k => k.Name).ToList(),
                    listMarks = _ocs.GetMarksAll().ToList(),
                    currencies = _ocs.GetCurrenciesAll().ToList(),
                    products = prdcs,
                    cities = _ocs.GetCitiesAll().ToList(),
                    listFuelTypes = _ocs.GetFuelTypesAll().ToList(),
                    listCaseTypes = _ocs.GetCaseTypesAll().ToList(),
                    listGearTypes = _ocs.GetGearTypesAll().ToList(),
                    FormName = "TasitAramaSonuclari"
                });
            }

            var products = new List<Products>();

            if (hdnInpSearch == null)
            {
                hdnInpSearch = string.Empty;
            }
            hdnInpSearch = hdnInpSearch.TrimStart().TrimEnd();
            if (hdnInpSearch == "Parça kodu, adı ya da açıklamasında" || hdnInpSearch == "İlan başlığı ya da açıklamasında" || hdnInpSearch == "Hizmet adı ya da açıklamasında")
            {
                hdnInpSearch = string.Empty;
            }

            var criteria = hdnInpSearch.Split(' ');

            var qry = string.Empty;

            // paged query should be ignore the other parameters
            if (Request.QueryString["page"] != null && Convert.ToInt32(Request.QueryString["page"]) > 1)
            {
                CatagoryId = "";
                MarkId = "null";
                ModelId = "null";
                otomobilvasitaCat6 = "null";
            }

            if (otomobilvasitaCat6 == null)
            {
                qry = HttpContext.Request.Url.AbsoluteUri + "?CatagoryId=" + CatagoryId + "&MarkId=" + MarkId + "&ModelId=" + ModelId + "&otomobilvasitaCat6=" + otomobilvasitaCat6 + "&hdnInpSearch=" + hdnInpSearch;
                Int32.TryParse(MarkId, out marId);
                //var marId = Convert.ToInt32(MarkId);
                var fChilds = _ocs.GetCategoriesById(marId).Select(m => m.Id).ToList();

                foreach (object obj in fChilds)
                    types1.Add(Convert.ToInt32(obj));

                var sChilds = _ocs.GetCategoriesByContainer(types1).Select(m => m.Id).ToList(); 

                foreach (object obj in sChilds)
                    types2.Add(Convert.ToInt32(obj));

                var tChilds = _ocs.GetCategoriesByContainer(types2).Select(m => m.Id).ToList();

                foreach (object obj in tChilds)
                    types3.Add(Convert.ToInt32(obj));

                types3.Add(marId);
                crit = criteria[0].ToString();
                products = _ocs.GetProductsByTypeAndContainerCategory(1, types3).Where(k => k.Name.Contains(crit) || crit == string.Empty || crit == string.Empty).ToList();
            }
            else
            {
                if (CatagoryId != "null" && MarkId != "null" && ModelId != "null" && otomobilvasitaCat6 != "null")
                {
                    Debug.Assert(HttpContext.Request.Url != null, "HttpContext.Request.Url != null");
                    qry = HttpContext.Request.Url.AbsoluteUri + "?CatagoryId=" + CatagoryId + "&MarkId=" + MarkId + "&ModelId=" + ModelId + "&otomobilvasitaCat6=" + otomobilvasitaCat6 + "&hdnInpSearch=" + hdnInpSearch;
                    var subModId = Convert.ToInt32(otomobilvasitaCat6);
                    var fChilds = _ocs.GetCategoriesById(subModId).Select(m => m.Id).ToList();
                    var types1 = fChilds.Select(fc => (int?)fc).ToList();
                    var crit = criteria[0].ToString();
                    products = _ocs.GetProductsByTypeAndCriteriaCodeAndContainer(1, crit, types1).ToList(); 
                        //_products.Where(K => K.ProductType == 1 && K.IsActive == true && types1.Contains(K.CategoryId) && (K.Name.Contains(crit) || crit == string.Empty)).Take(500).ToList();
                }
                else
                {
                    if (CatagoryId != "null" && MarkId != "null" && ModelId != "null")
                    {
                        qry = HttpContext.Request.Url.AbsoluteUri + "?CatagoryId=" + CatagoryId + "&MarkId=" + MarkId + "&ModelId=" + ModelId + "&otomobilvasitaCat6=" + otomobilvasitaCat6 + "&hdnInpSearch=" + hdnInpSearch;
                        var modId = Convert.ToInt32(ModelId);
                        var fChilds = _ocs.GetCategoriesById(modId).Select(m => m.Id).ToList();
                        var types1 = fChilds.Select(fc => (int?)fc).ToList();

                        var sChilds = _ocs.GetCategoriesByContainer(types1).Select(m => m.Id).ToList();
                        var types2 = sChilds.Select(sc => (int?)sc).ToList();
                        types2.Add(modId);
                        var crit = criteria[0].ToString();
                        products = _ocs.GetProductsByTypeAndCriteriaCodeAndContainer(1, crit, types2).ToList();
                        //_products.Where(K => K.ProductType == 1 && K.IsActive == true && types2.Contains(K.CategoryId) && (K.Name.Contains(crit) || crit == string.Empty)).Take(500).ToList();
                    }
                    else
                    {
                        if (CatagoryId != "null" && MarkId != "null" && otomobilvasitaCat6 == "null")
                        {
                            qry = HttpContext.Request.Url.AbsoluteUri + "?CatagoryId=" + CatagoryId + "&MarkId=" + MarkId + "&ModelId=" + ModelId + "&otomobilvasitaCat6=" + otomobilvasitaCat6 + "&hdnInpSearch=" + hdnInpSearch;
                            Int32.TryParse(MarkId, out marId);
                            //var marId = Convert.ToInt32(MarkId);
                            var fChilds = _ocs.GetCategoriesById(marId).Select(m => m.Id).ToList();


                            foreach (object obj in fChilds)
                                types1.Add(Convert.ToInt32(obj));

                            var sChilds = _ocs.GetCategoriesByContainer(types1).Select(m => m.Id).ToList();

                            foreach (object obj in sChilds)
                                types2.Add(Convert.ToInt32(obj));

                            var tChilds = _ocs.GetCategoriesByContainer(types2).Select(m => m.Id).ToList();

                            foreach (object obj in tChilds)
                                types3.Add(Convert.ToInt32(obj));

                            types3.Add(marId);
                            crit = criteria[0].ToString();
                            products = _ocs.GetProductsByTypeAndCriteriaCodeAndContainer(1, crit, types3).ToList();
                                //_products.Where(K => K.ProductType == 1 && K.IsActive == true && types3.Contains(K.CategoryId) && (K.Name.Contains(crit) || crit == string.Empty || crit == string.Empty)).Take(500).ToList();
                        }
                        else
                        {
                            if (CatagoryId != "null" && MarkId == "null" && ModelId == "null" && otomobilvasitaCat6 == "null")
                            {
                                qry = HttpContext.Request.Url.AbsoluteUri + "?CatagoryId=" + CatagoryId + "&MarkId=" + MarkId + "&ModelId=" + ModelId + "&otomobilvasitaCat6=" + otomobilvasitaCat6 + "&hdnInpSearch=" + hdnInpSearch;
                                var crit = criteria[0].ToString();
                                products = _ocs.GetProductsByTypeAndCriteriaCode(1, crit).ToList();
                                    //_products.Where(K => K.ProductType == 1 && K.IsActive == true && (K.Name.Contains(crit) || crit == string.Empty)).Take(500).ToList();
                            }
                            else
                            {
                                if (CatagoryId == "null" && MarkId == "null" && ModelId == "null" && otomobilvasitaCat6 == "null")
                                {
                                    qry = HttpContext.Request.Url.AbsoluteUri + "?CatagoryId=" + CatagoryId + "&hdnInpSearch=" + hdnInpSearch;

                                    if (criteria[0] != string.Empty)
                                    {
                                        var tempPro1 = new List<Products>();

                                        foreach (var sCriteria in criteria)
                                        {
                                            var criteria1 = sCriteria;
                                            tempPro1.AddRange(_ocs.GetProductsByTypeAndCriteriaCode(1, criteria1).ToList());
                                        }
                                        products = tempPro1.OrderByDescending(m => m.RecordDate).Take(500).ToList();
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (!HttpContext.Request.UrlReferrer.AbsoluteUri.Contains("SearchProduct"))
            {
                HttpContext.Session["qry"] = qry;
            }
            if (HttpContext.Request.UrlReferrer.AbsoluteUri.Contains("GeneralSearch"))
            {
                HttpContext.Session["qry"] = HttpContext.Request.UrlReferrer.AbsoluteUri;
            }
            else
            {
            }

            if (!products.Any() && CatagoryId == "null" && MarkId == "null" && ModelId == "null" && otomobilvasitaCat6 == "null")
            {
                products = _ocs.GetProductsByType(1).ToList();
                    //_products.Where(k => k.ProductType == 1 && k.IsActive == true).OrderByDescending(m => m.Id).Take(100).ToList();
            }

            if (productFuelType != null)
            {

                if (productFuelType.Trim() != "1")
                {
                    var fuelType = Convert.ToInt32(productFuelType);
                    products = products.Where(k => k.FuelType == fuelType).ToList();
                }
                if (productCaseType.Trim() != "1")
                {
                    var caseType = Convert.ToInt32(productCaseType);
                    products = products.Where(k => k.CaseType == caseType).ToList();
                }
                if (productGearType.Trim() != "1")
                {
                    var gearType = Convert.ToInt32(productGearType);
                    products = products.Where(k => k.GearType == gearType).ToList();
                }

                if (fader1.Trim() != "1960")
                {
                    var modelYearStart = Convert.ToInt32(fader1);
                    products = products.Where(k => Convert.ToInt32(k.ModelYear) >= modelYearStart).ToList();
                }
                if (fader2.Trim() != "2015")
                {
                    var modelYearEnd = Convert.ToInt32(fader2);
                    products = products.Where(k => Convert.ToInt32(k.ModelYear) <= modelYearEnd).ToList();
                }

                if (fader3.Trim() != "750")
                {
                    var engineStart = Convert.ToInt32(fader3);
                    products = products.Where(k => k.EngineCapacity >= engineStart).ToList();
                }
                if (fader4.Trim() != "10000")
                {
                    var engineEnd = Convert.ToInt32(fader4);
                    products = products.Where(k => k.EngineCapacity <= engineEnd).ToList();
                }

                if (startPrice.Trim() != string.Empty)
                {
                    var priceStart = Convert.ToInt32(startPrice);
                    products = products.Where(k => k.CurrentPrice >= priceStart).ToList();
                }
                if (endPrice != null && endPrice.Trim() != string.Empty)
                {
                    var priceEnd = Convert.ToInt32(endPrice);
                    products = products.Where(k => k.CurrentPrice <= priceEnd).ToList();
                }
            }

            List<Products> lpd2 = products.Take(3).ToList();
            string exp = "";
            foreach (Products p in lpd2)
                exp += p.Name + ", " + p.Code;

            if (products.Count > 0)
            {
                ViewBag.Title = exp;
                Categories catt = null;
                string catname = "";
                if (products[0].CategoryId != null)
                {
                    Int32 cidd = Convert.ToInt32(products[0].CategoryId);
                    catt = _ocs.GetCategoriesById(cidd).FirstOrDefault();
                    catname = catt.Name;
                }
                ViewBag.Description = catname + "/" + products[0].Name;
                Cities city = null;
                string cityName = "";
                if (products[0].City != null)
                {
                    Int32 cityidd = Convert.ToInt32(products[0].City);
                    city = _ocs.GetCitiesById(cityidd);
                    cityName = city.Name;
                }

                if (city != null)
                    cityName = city.Name + ",";
                ViewBag.Keywords = cityName + catname + "," + exp;
            }

            Session["BeforeProducts"] = products;
            List<Products> prods = products.OrderByDescending(m => m.Id).ToList();

            return View(new otomobilvasitaViewModel
            {
                listpG = _ocs.GetProductGroupsAll().OrderBy(k => k.ProductName).ToList(),
                listCatG = _ocs.GetCategoriesAll().ToList(),
                listMarks = _ocs.GetMarksAll().ToList(),
                currencies = _ocs.GetCurrenciesAll().OrderBy(k => k.Name).ToList(),
                products = prods,
                cities = _ocs.GetCitiesAll().OrderBy(k => k.Name).ToList(),
                listFuelTypes = _ocs.GetFuelTypesAll().OrderBy(k => k.Name).ToList(),
                listCaseTypes = _ocs.GetCaseTypesAll().OrderBy(k => k.Name).ToList(),
                listGearTypes = _ocs.GetGearTypesAll().OrderBy(k => k.Name).ToList(),
                ViewType = "OtomobilKategoriSonuclari"
            });
        }


        //public ActionResult _SearchProductSpareparts(string categoryId, string markId, string criteria, string id)
        //{
        //    //int catId = 0;
        //    //if (categoryId == null)
        //    //    catId = 1;
        //    //else if (categoryId != null && categoryId != "") catId = Convert.ToInt32(categoryId);

        //    //int markaId = 1;
        //    //if (markId != null)
        //    //    markaId = Convert.ToInt32(markId);

        //    VhicleModel vm = new VhicleModel();
        //    vm.listCatG = _categories.All().ToList();
        //    vm.currencies = _currencies.All().ToList();
        //    vm.cities = _cities.All().ToList();
        //    vm.listpG = _productGroups.All().OrderBy(k => k.ProductName).ToList();
        //    vm.listMarks = _marks.All().ToList();

        //    var productsPredicate = PredicateBuilder.False<Products>();

        //    //if (!String.IsNullOrEmpty(categoryId) && !String.IsNullOrEmpty(markId))
        //    //{
        //    //    List<int?> catpIds = context.Categories.Where(k => k.ParentId == markaId).Select(m => (int?)m.Id).ToList();
        //    //    List<int?> catpIdsReal = context.Categories.Where(k => catpIds.Contains(k.ParentId)).Select(m => (int?)m.Id).ToList();
        //    //    productsPredicate = productsPredicate.Or(k => catpIdsReal.Contains(k.CategoryId) && k.IsActive == true);
        //    //}

        //    if (!string.IsNullOrEmpty(criteria))
        //    {
        //        productsPredicate = productsPredicate.And(k => k.Code.Contains(criteria));
        //    }

        //    vm.products = _products.Where(k=> k.Code.Contains(criteria)).Take(200).ToList();

        //    return PartialView(vm);
        //}

        public ActionResult _SearchProductVhicle(string categoryId, string markId, string model, string altModeId, string kasaTipi, string vitesTipi, string lokasyon, string yakitTipi, string modelYearStart, string modelYearEnd, string silindirStart, string silindirEnd, string priceStart, string priceEnd, string criteria, string id)
        {
            var productsPredicate = PredicateBuilder.True<Products>();

            int cityId = 0;
            if (lokasyon != null && lokasyon.Trim() != "")
                cityId = Convert.ToInt32(lokasyon);

            int cid = Convert.ToInt32(categoryId);
            if (cid > 0)
                productsPredicate = productsPredicate.And(_ => _.CategoryId == cid);

            int markaId = 1;
            if (markId != null && markId != "null")
                markaId = Convert.ToInt32(markId);

            VhicleModel vm = new VhicleModel();
            vm.listCatG = _ocs.GetCategoriesAll().ToList();
            vm.currencies = _ocs.GetCurrenciesAll().ToList();
            vm.cities = _ocs.GetCitiesAll().ToList();

            if (!string.IsNullOrEmpty(altModeId) && altModeId != "null")
            {
                var modId = Convert.ToInt64(altModeId);
                var firstChilds = _ocs.GetCategoriesById(modId).Select(m => m.Id).ToList();
                var firstChildIds = firstChilds.Select(fc => (int?)fc).ToList();
                var secondChilds = _ocs.GetCategoriesByContainer(firstChildIds).Select(m => m.Id).ToList();
                var secondChildIds = secondChilds.Select(sc => (int?)sc).ToList();
                secondChildIds.Add(Convert.ToInt32(modId));
                //productsPredicate = productsPredicate.And(k => secondChildIds.Contains(k.CategoryId));
            }
            else
                if (!string.IsNullOrEmpty(model) && model != "null")
                {
                    var altmodId = Convert.ToInt64(model);
                    var firstChilds = _ocs.GetCategoriesById(altmodId).Select(m => m.Id).ToList();
                    var firstChildIds = firstChilds.Select(fc => (int?)fc).ToList();
                    var secondChilds = _ocs.GetCategoriesByContainer(firstChildIds).Select(m => m.Id).ToList();
                    var secondChildIds = secondChilds.Select(sc => (int?)sc).ToList();
                    //productsPredicate = productsPredicate.And(k => secondChildIds.Contains(k.CategoryId));
                }
                else
                    if (!String.IsNullOrEmpty(categoryId) && (markId != null && markId.ToString() != "null"))
                    {
                        List<int?> catpIds = _ocs.GetCategoriesById(markaId).Select(m => (int?)m.Id).ToList();
                        List<int?> catpIdsReal = _ocs.GetCategoriesByContainer(catpIds).Select(m => (int?)m.Id).ToList();
                        productsPredicate = productsPredicate.And(k => catpIdsReal.Contains(k.CategoryId) && k.IsActive == true);
                    }

            if (!string.IsNullOrEmpty(kasaTipi) && kasaTipi != "1")
            {
                int casetypeId = Convert.ToInt32(kasaTipi);
                productsPredicate = productsPredicate.And(k => k.CaseType == casetypeId);
            }
            if (!string.IsNullOrEmpty(vitesTipi) && vitesTipi != "1")
            {
                int gearTypeId = Convert.ToInt32(vitesTipi);
                productsPredicate = productsPredicate.And(k => k.GearType == gearTypeId);
            }
            if (!string.IsNullOrEmpty(yakitTipi) && yakitTipi != "1")
            {
                int fuelTypeId = Convert.ToInt32(yakitTipi);
                productsPredicate = productsPredicate.And(k => k.FuelType == fuelTypeId);
            }
            if (!string.IsNullOrEmpty(modelYearStart))
            {
                int modelYearStartInt = Convert.ToInt32(modelYearStart);
                productsPredicate = productsPredicate.And(k => k.ModelYear > modelYearStartInt);
            }
            if (!string.IsNullOrEmpty(modelYearEnd))
            {
                int modelYearEndInt = Convert.ToInt32(modelYearEnd);
                productsPredicate = productsPredicate.And(k => k.ModelYear < modelYearEndInt);
            }
            //if (!string.IsNullOrEmpty(silindirStart))
            //{
            //    int silindirStartInt = Convert.ToInt32(silindirStart);
            //    productsPredicate = productsPredicate.And(k => k.EngineCapacity > silindirStartInt);
            //}
            //if (!string.IsNullOrEmpty(silindirEnd))
            //{
            //    int silindirEndInt = Convert.ToInt32(silindirEnd);
            //    productsPredicate = productsPredicate.And(k => k.EngineCapacity < silindirEndInt);
            //}
            if (!string.IsNullOrEmpty(priceStart))
            {
                decimal priceStartdes = Convert.ToDecimal(priceStart);
                productsPredicate = productsPredicate.And(k => k.CurrentPrice > priceStartdes);
            }
            if (!string.IsNullOrEmpty(priceEnd))
            {
                decimal priceEnddes = Convert.ToDecimal(priceEnd);
                productsPredicate = productsPredicate.And(k => k.CurrentPrice < priceEnddes);
            }


            if (cityId > 0)
            {
                productsPredicate = productsPredicate.And(k => k.City == cityId);
            }

            if (Request["searchTextIdSparepartGeneral"] != null)
            {
                criteria = Request["searchTextIdSparepartGeneral"].ToString();
            }

            if (!string.IsNullOrEmpty(criteria))
            {
                productsPredicate = productsPredicate.And(k => k.Name.Contains(criteria) || k.Code.Contains(criteria));
            }

            productsPredicate = productsPredicate.And(_=> _.ProductType == 1);

            vm.products = _ocs.ProductsWhere(productsPredicate).ToList();

            return PartialView("~/Views/shared/_SearchProductVhicle.cshtml", vm);
        }

        public int[] FindCategoryIds(int? id)
        {
            var productCategory = _ocs.GetCategoriesById(Convert.ToInt32(id)).FirstOrDefault();
            if (productCategory == null)
            {
                return null;
            }
            var rootLevel = productCategory.RootLevel;
            var productCategoryIds = new int[rootLevel];
            int? parentId = id;
            if (rootLevel == 1)
            {
                productCategoryIds[1] = productCategory.Id;
            }
            else
            {
                for (var i = rootLevel; i > 0; i--)
                {
                    var prdcCat = _ocs.GetCategoriesById(Convert.ToInt32(parentId)).FirstOrDefault();
                    productCategoryIds[i - 1] = prdcCat.Id;
                    parentId = prdcCat.ParentId;
                }
            }
            return productCategoryIds;
        }

        public string FindGroups(int? id)
        {
            var productGroup = _ocs.GetProductGroupsById(Convert.ToInt32(id));
            if (productGroup == null)
            {
                return null;
            }
            var hasParent = productGroup.ParentId != null;
            var productGroups = productGroup.ProductName;
            while (hasParent)
            {
                var parentPrdcGroup = _ocs.GetProductGroupsById(Convert.ToInt32(productGroup.ParentId));
                productGroups = parentPrdcGroup.ProductName + "->" +
                            productGroups;
                hasParent = parentPrdcGroup.ParentId != null;
            }
            return productGroups;
        }

        public int[] FindGroupIds(int? id)
        {
            var productGroup = _ocs.GetProductGroupsById(Convert.ToInt32(id));
            if (productGroup == null)
            {
                return null;
            }
            var hasParent = productGroup.ParentId != null;
            var productGroupIds = new int[2];
            productGroupIds[0] = productGroup.Id;
            while (hasParent)
            {
                var parentPrdcGroup = _ocs.GetProductGroupsById(Convert.ToInt32(productGroup.ParentId));
                productGroupIds[0] = parentPrdcGroup.Id;
                productGroupIds[1] = productGroup.Id;
                hasParent = parentPrdcGroup.ParentId != null;
            }
            return productGroupIds;
        }

        public ActionResult MessageSenderPartial(int uid, int pid)
        {
            var umm = new UserMessagesModel();
            var umes = new UserMessages();
            umes.SenderUserId = uid;
            umes.Id = pid;
            umm.UserMessageses = new List<UserMessages>();
            umm.UserMessageses.Add(umes);
                umm.UserMessageses = _ocs.GetUserMessagesById(uid).OrderByDescending(m => m.Id).Take(5).ToList();
            return PartialView(umm);
        }

        [WhitespaceFilter]
        [AuthorizeActionFilterAttribute]
        public ActionResult SendMessage(string txtSubject, string textareaMessage, string accountId, string advertisementId)
        {
            if (advertisementId != "-1")
            {
                var aid = Convert.ToInt32(advertisementId);
                var suid = Convert.ToInt32(accountId);
                var product = _ocs.GetProductsById(aid);
                Int32 receiver = Convert.ToInt32(product.RegistererId);
                _ocs.InsertMessage(new UserMessages()
                {
                    Content = textareaMessage,
                    ReceiverUserId = receiver,
                    SenderUserId = suid,
                    Title = txtSubject,
                    SendingDate = DateTime.Now
                });
                List<UserMessages> mess = _ocs.GetUserMessagesById(receiver).ToList();
                foreach (UserMessages um in mess)
                {
                    um.ReceiveDate = DateTime.Now;
                    _uow.SavaChange();
                }

                _uow.SavaChange();
            }
            else
            {
                Int32 receiver = Convert.ToInt32(Session["UserIdInSession"]);
                _ocs.InsertMessage(new UserMessages()
                {
                    Content = textareaMessage,
                    ReceiverUserId = Convert.ToInt32(accountId),
                    SenderUserId = receiver,
                    Title = txtSubject,
                    SendingDate = DateTime.Now
                });
                List<UserMessages> mess = _ocs.GetUserMessagesById(receiver).ToList();
                foreach (UserMessages um in mess)
                {
                    um.ReceiveDate = DateTime.Now;
                    _uow.SavaChange();
                }
                _uow.SavaChange();
            }

            var url = this.Request.UrlReferrer.OriginalString;
            return Redirect(url);
        }

        public ActionResult AddToFavorite(int uid, int pid)
        {
            var product = _ocs.GetProductsById(pid);
            var ufeed = new UserFeedbacks()
            {
                UserId = uid,
                GalleryId = product.RegistererId,
                AutoServiceId = pid,
                PleasedCount = 0,
                UnpleasedCount = 0
            };
            _ocs.InsertFeedBacks(ufeed);
            _uow.SavaChange();

            var url = this.Request.UrlReferrer.OriginalString;
            return Redirect(url);
        }

        public ActionResult GoToTargetProfile(int pid)
        {
            var product = _ocs.GetProductsById(pid);
            UpdateUserProfileInfo(Convert.ToInt32(product.RegistererId), false);
            return RedirectToAction("ProfileView", "otomobilvasita", new { isOwner = false });
        }
        [HttpPost]
        public ActionResult PrdDelete(int id)
        {
            var product = _ocs.GetProductsById(id);
            product.IsActive = false;
            _uow.SavaChange();
            return Json(new { status = "success", message = "Ürün kaydı sistemimizde pasif edilmiştir..." });
        }
    }

    public static class StringHelpers
    {
        public static string ToSeoUrl(this string url)
        {
            // make the url lowercase
            string encodedUrl = (url ?? "").ToLower();

            // replace & with and
            encodedUrl = Regex.Replace(encodedUrl, @"\&+", "and");

            // remove characters
            encodedUrl = encodedUrl.Replace("'", "");

            // remove invalid characters
            encodedUrl = Regex.Replace(encodedUrl, @"[^a-z0-9]", "-");

            // remove duplicates
            encodedUrl = Regex.Replace(encodedUrl, @"-+", "-");

            // trim leading & trailing characters
            encodedUrl = encodedUrl.Trim('-');

            return encodedUrl;
        }

        public static string filterWords(string keywords)
        {

            keywords = Regex.Replace(keywords, "-", "%2D");

            keywords = Regex.Replace(keywords, "@", "%40");

            keywords = Regex.Replace(keywords, "\\*", "%2A");

            keywords = Regex.Replace(keywords, "\\+", "%2B");

            keywords = Regex.Replace(keywords, "\\/", "%2F");

            keywords = Regex.Replace(keywords, "\\.", "%2E");

            keywords = Regex.Replace(keywords, "%", "$");

            return keywords;

        }

        public static List<string> urlEncodedCharacters = new List<string>
        {
          "/", "\\", "<", ">", ":", "\"", "|", "?", "%" //and others, but not *
        };
        //Since this is a superset of urlEncodedCharacters, we won't be able to only use UrlEncode() - instead we'll use HexEncode
        public static List<string> specialCharactersNotAllowedInWindows = new List<string>
        {
          "/", "\\", "<", ">", ":", "\"", "|", "?", "*" //windows dissallowed character set
        };

        public static string ToEncode(this string fileName)
        {
            List<string> charactersToChange = new List<string>(specialCharactersNotAllowedInWindows);
            charactersToChange.AddRange(urlEncodedCharacters.
                Where(x => !urlEncodedCharacters.Union(specialCharactersNotAllowedInWindows).Contains(x)));   // add any non duplicates (%)

            charactersToChange.ForEach(s => fileName = fileName.Replace(s, Uri.HexEscape(s[0])));   // "?" => "%3f"

            return fileName;
        }


    }

    public static class PredicateBuilder
    {
        /// <summary>
        /// Creates a predicate that evaluates to true.
        /// </summary>
        public static Expression<Func<T, bool>> True<T>() { return param => true; }

        /// <summary>
        /// Creates a predicate that evaluates to false.
        /// </summary>
        public static Expression<Func<T, bool>> False<T>() { return param => false; }

        /// <summary>
        /// Creates a predicate expression from the specified lambda expression.
        /// </summary>
        public static Expression<Func<T, bool>> Create<T>(Expression<Func<T, bool>> predicate) { return predicate; }

        /// <summary>
        /// Combines the first predicate with the second using the logical "and".
        /// </summary>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.AndAlso);
        }

        /// <summary>
        /// Combines the first predicate with the second using the logical "or".
        /// </summary>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.OrElse);
        }

        /// <summary>
        /// Negates the predicate.
        /// </summary>
        public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> expression)
        {
            var negated = Expression.Not(expression.Body);
            return Expression.Lambda<Func<T, bool>>(negated, expression.Parameters);
        }

        /// <summary>
        /// Combines the first expression with the second using the specified merge function.
        /// </summary>
        static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
        {
            // zip parameters (map from parameters of second to parameters of first)
            var map = first.Parameters
                .Select((f, i) => new { f, s = second.Parameters[i] })
                .ToDictionary(p => p.s, p => p.f);

            // replace parameters in the second lambda expression with the parameters in the first
            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

            // create a merged lambda expression with parameters from the first expression
            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }

        class ParameterRebinder : ExpressionVisitor
        {
            readonly Dictionary<ParameterExpression, ParameterExpression> map;

            ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
            {
                this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
            }

            public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
            {
                return new ParameterRebinder(map).Visit(exp);
            }

            protected override Expression VisitParameter(ParameterExpression p)
            {
                ParameterExpression replacement;

                if (map.TryGetValue(p, out replacement))
                {
                    p = replacement;
                }

                return base.VisitParameter(p);
            }
        }
    }
}
