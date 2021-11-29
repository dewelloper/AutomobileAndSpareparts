using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.OleDb;
using System.Data.SqlClient;
using EntityState = System.Data.Entity.EntityState;
using Dal;
using Otomotivist.Domain.UnitOfWork;
using Otomotivist.Domain.Repository;

namespace Mvc4WebRole.Controllers
{
    public class ProductsController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IGRepository<Products> _products;
        private readonly IGRepository<UserProfile> _userProfile;
        private readonly IGRepository<Marks> _marks;
        private readonly IGRepository<Denominations> _denominations;

        public ProductsController(IUnitOfWork uow)
        {
            _uow = uow;
            _products = _uow.GetRepository<Products>();
            _userProfile = _uow.GetRepository<UserProfile>();
            _marks = _uow.GetRepository<Marks>();
            _denominations = _uow.GetRepository<Denominations>();
        }

        public ActionResult ImportAndUpdateFromExcel()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            return View();
        }

        public ActionResult enterOrUpdateProducts(HttpPostedFileBase ExcelFile)
        {
            var msg = "";
            var dtMaterials = new DataTable();
            if (ExcelFile != null)
            {
                var path = string.Concat(Server.MapPath("~/File/" + ExcelFile.FileName));
                ExcelFile.SaveAs(path);

                var connExcelString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";", path);

                using (var OledbConnection = new OleDbConnection(connExcelString))
                {
                    var cmd_GetExcelData = new OleDbCommand("Select PartCode,PartName,PartPrice,PartCurrency,PartCompMarkId FROM [Sheet1$]", OledbConnection);
                    OledbConnection.Open();

                    // Create DbDataReader to Data Worksheet
                    using (var daMaterials = new OleDbDataAdapter(cmd_GetExcelData))
                    {
                        daMaterials.Fill(dtMaterials);
                    }
                    OledbConnection.Close();
                }

                //Here is the code to update table:-

                #region try transaction 
                //Set up the bulk copy object inside the transaction. 
                //var connSql = @"Data Source=.\sql2012; Database=Mvc4WebRole.Models.Mvc4WebRoleContext; User Id=sa; Password=sa1234;";
                var connSql = @"Data Source=.; Database=Mvc4WebRole.Models.Mvc4WebRoleContext; User Id=sa; Password=Dimple943;";
                using (var destinationConnection = new SqlConnection(connSql))
                {
                    destinationConnection.Open();

                    using (var transaction = destinationConnection.BeginTransaction())
                    {
                        using (var bulkCopy = new SqlBulkCopy(destinationConnection, SqlBulkCopyOptions.Default, transaction))
                        {
                            var map1 = new SqlBulkCopyColumnMapping("PartCompMarkId", "CategoryId");
                            var map2 = new SqlBulkCopyColumnMapping("PartCode", "Code");
                            var map3 = new SqlBulkCopyColumnMapping("PartName", "Name");
                            var map4 = new SqlBulkCopyColumnMapping("PartPrice", "CurrentPrice");
                            var map5 = new SqlBulkCopyColumnMapping("PartCurrency", "PriceCurrencyId");

                            bulkCopy.ColumnMappings.Add(map1);
                            bulkCopy.ColumnMappings.Add(map2);
                            bulkCopy.ColumnMappings.Add(map3);
                            bulkCopy.ColumnMappings.Add(map4);
                            bulkCopy.ColumnMappings.Add(map5);

                            bulkCopy.DestinationTableName = "TempExcelImport";
                            try
                            {
                                // add a new column with empty rows to match with intTrnID column in table materials  
                                var dcTrnID = new DataColumn("Id", Type.GetType("System.Int64") );
                                var RecordDate = new DataColumn("RecordDate", Type.GetType("System.DateTime"));
                                var RegistererId = new DataColumn("RegistererId", Type.GetType("System.Int32"));

                                var userName = User.Identity.Name;
                                var userId = _userProfile.Where(k => k.UserName == userName).FirstOrDefault().UserId;

                                RegistererId.DefaultValue = userId;
                                dcTrnID.AutoIncrement = true;

                                dcTrnID.AutoIncrementSeed = 1;

                                dcTrnID.AutoIncrementStep = 1;

                                dtMaterials.Columns.Add(dcTrnID);
                                dtMaterials.Columns.Add(RecordDate);
                                dtMaterials.Columns.Add(RegistererId);

                                bulkCopy.WriteToServer(dtMaterials);

                                var callStrdProcImpExc = new SqlCommand("EXEC spExcelImport", destinationConnection, transaction);
                                callStrdProcImpExc.ExecuteNonQuery();

                                var editRegisterer = new SqlCommand("UPDATE Products SET RegistererId=" + userId + " WHERE RegistererId IS NULL", destinationConnection, transaction);
                                editRegisterer.ExecuteNonQuery();

                                //bool Failed = Convert.ToBoolean( ThreeWayMatching.UpdateExchangeRates()); 
                                transaction.Commit();
                                /*if (Failed == false)
                {
                lblMsg.Text = "Materials Code Updated Successfully"
                lblErrMsg.Text = "";
                }
                else
                {
                lblErrMsg.Text = "Update Not Successful";
                lblMsg.Text = "";
                }*/
                            }
                            catch (Exception ex)
                            {
                                msg = "Hata ! " + ex.Message;

                                transaction.Rollback();
                            }
                        }
                    }
                    destinationConnection.Close();
                }
                #endregion                
            }
            else
            {
                msg = "Dosya yüklenirken hata oluştu!";
            }

            ViewBag.ExcelFileUpload = msg;
            return RedirectToAction("ImportAndUpdateFromExcel");
        }

        //
        // GET: /Products/

        public ViewResult Index()
        {
            return View(_products.All());
        }

        //
        // GET: /Products/Details/5

        public ViewResult Details(long id)
        {
            var product = _products.All().Select(x => x.Id == id);
            return View(product);
        }

        //
        // GET: /Products/Create

        public ActionResult Create()
        {
            ViewBag.PossibleMarks = _marks.All();
            ViewBag.PossibleDenominations = _denominations.All();
            return View();
        }

        //
        // POST: /Products/Create

        [HttpPost]
        public ActionResult Create(Products product)
        {
            if (ModelState.IsValid)
            {
                _products.Insert(product);
                _uow.SavaChange();
                return RedirectToAction("Index");
            }

            ViewBag.PossibleMarks = _marks.All();
            ViewBag.PossibleDenominations = _denominations.All();
            return View(product);
        }
        //
        // GET: /Products/Edit/5
        public ActionResult Edit(long id)
        {
            var product = _products.Where(x => x.Id == id).FirstOrDefault();
            ViewBag.PossibleMarks = _marks.All();
            ViewBag.PossibleDenominations = _denominations.All();
            return View(product);
        }

        //
        // POST: /Products/Edit/5

        [HttpPost]
        public ActionResult Edit(Products product)
        {
            //if (ModelState.IsValid)
            //{
            //    context.Entry(product).State = EntityState.Modified;
            //    context.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            //ViewBag.PossibleMarks = context.Marks;
            //ViewBag.PossibleDenominations = context.Denominations;
            return View(product);
        }

        //
        // GET: /Products/Delete/5
        public ActionResult Delete(long id)
        {
            //var product = context.Products.Single(x => x.Id == id);
            return View();
        }

        //
        // POST: /Products/Delete/5

        [HttpPost,
        ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            //var product = context.Products.Single(x => x.Id == id);
            //context.Products.Remove(product);
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
