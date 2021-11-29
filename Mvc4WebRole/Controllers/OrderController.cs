using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System.Xml;
using System.Text;
using System.Net;
using Dal;
using BusinessObjects;
using Otomotivist.Domain.UnitOfWork;
using Otomotivist.Domain.Repository;
using Otomotivist.Service.interfaces;
using System.Collections.Generic;

namespace Mvc4WebRole.Controllers
{
    public class OrderController : BaseController
    {

        private string _companyPosUser = "0001";
        private string _companyPosPwd = "18367854";
        private string _companyMemberNo = "000089094";
        private string _companyPosNo = "VP008908";
        private string _companyXcip = "SJU6NL7ZXM";

        private string _systemError = "Bankayla bağlantı kurulamadı ! Lütfen daha sonra tekrar deneyin.";

        public bool _result { get; set; }
        public string _errorMessage { get; set; }
        public string _errorCode { get; set; }

        public string _code { get; set; }
        public string _groupId { get; set; }
        public string _transId { get; set; }
        public string _refferenceNo { get; set; }


        private readonly IUnitOfWork _uow;
        private readonly IOtomotivistCoreService _ocs;
        private readonly IOrderService _ios;

        public OrderController(IUnitOfWork uow, IOtomotivistCoreService ocs, IOrderService ios)
        {
            _uow = uow;
            _ocs = ocs;
            _ios = ios;
        }

        [AuthorizeActionFilterAttribute]
        public ActionResult _OrderBasketView()
        {
            var IdsOfPrdcsInBasket = (from DataRow item in (HttpContext.Session["basket"] as DataTable).Rows
                                       select Convert.ToInt64(item["id"])).ToList();

            var ListProduct = _ios.GetContainerProducts(IdsOfPrdcsInBasket).ToList();
            var userName = User.Identity.Name;
            UserProfile up = _ocs.GetUserProfileByName(userName);
            List<UserProfile> ups = new List<UserProfile>();
            ups.Add(up);
            return View(new OrderViewModel
            {
                listProduct = ListProduct,
                listUserAddress = _ocs.GetUserAddressesByUserId(up.UserId).ToList(),
                listUserProfile = ups,
                listCities = _ocs.GetCitiesAll().ToList()
            });
        }

        public JsonResult BasketSession(int Id, int value)
        {
            Debug.Assert(HttpContext.Session != null, "HttpContext.Session != null");
            ((DataTable) HttpContext.Session["basket"]).Rows[Id][2] = value;

            return Json(true);
        }

        public JsonResult GetTransportTypes()
        {
            var c2 = _ocs.GetTransportTypesAll();
            return Json(new SelectList(c2.ToArray(), "Id", "Name"), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAddresses()
        {
            var userName = User.Identity.Name;
            if (userName == string.Empty)
            {
                return Json(null);
            }
            var firstOrDefault = _ocs.GetUserProfileByName(userName);
            if (firstOrDefault == null)
            {
                return Json(null);
            }
            var userId = firstOrDefault.UserId;
            var c2 = _ocs.GetUserAddressesByUserId(userId).ToList();
            return Json(new SelectList(c2.ToArray(), "Id", "Address"), JsonRequestBehavior.AllowGet);
        }

        private static void SendEMail(string subject, string body)
        {
            var client = new SmtpClient
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = true,
                Host = "smtp.gmail.com",
                Port = 587
            };

            var credentials = new NetworkCredential("oto343434@gmail.com", "oto123456");
            client.UseDefaultCredentials = false;
            client.Credentials = credentials;

            var msg = new MailMessage { From = new MailAddress("otomotivist@gmail.com") };
            msg.To.Add(new MailAddress("h.sag_58@hotmail.com"));

            msg.Subject = subject;
            msg.IsBodyHtml = true;
            msg.Body = body;

            client.Send(msg);
        }

        public ActionResult ConfirmOrder(FormCollection collection)
        {
            Debug.Assert(HttpContext.Session != null, "HttpContext.Session != null");
            //if (!Convert.ToBoolean(HttpContext.Session["hasLocalAccount"]))
            //{
            //    TempData["Message"] = "Sipariş verebilmek için giriş yapmalısınız !";
            //    return RedirectToAction("Login", "Account");
            //}

            var txtManyInstallment = collection.Get("txtManyInstallment") == string.Empty ? "0" : collection.Get("txtManyInstallment");

            var price = Convert.ToDecimal(Session["TotalPrice"]);
            var cardNumber = Convert.ToInt64(collection.Get("txtCardNumber"));
            var cardOwner = collection.Get("txtCardOwnerName");
            var month = Convert.ToInt32(collection.Get("hdnSelectedMonth"));
            var year = Convert.ToInt32(collection.Get("hdnSelectedYear"));
            var secureCode = Convert.ToInt32(collection.Get("txtCVV2"));
            var Price = Convert.ToDouble(price);
            var installments = Convert.ToInt32(txtManyInstallment);

            var pForm = new PosForm()
            {
                CardNumber = cardNumber,
                CardOwner = cardOwner,
                Month = month,
                Year = year,
                SecureCode = secureCode,
                Price = Price,
                Installments = installments
            };

            var isPaymentSuccess = PayWithVakifBank(pForm);
            if (isPaymentSuccess == 1 || isPaymentSuccess == 0)
            {
                var currentDate = DateTime.Now;
                var explanation = collection.Get("orderExplanation");
                var transportTypeSelect = Convert.ToInt32(collection.Get("transportTypeSelect"));
                var firstOrDefault = _ocs.GetTractionTypesAll().Where(d => d.Id == transportTypeSelect).FirstOrDefault();
                if (firstOrDefault == null)
                {
                    return View();
                }
                var transportType = firstOrDefault.Name;
                var relatedPhone = collection.Get("relatedPhone");

                var ai = HttpContext.Session["AccountInformation"] as AccountInformation;

                if (collection.Get("txtAreaNewAddress") != string.Empty)
                {
                    Debug.Assert(ai != null, "ai != null");
                    ai.SelectedAdress.Address = collection.Get("txtAreaNewAddress");
                }

                var basket = HttpContext.Session["basket"] as DataTable;

                var order = new Orders()
                {
                    Count = 1,
                    Date = currentDate,
                    Discount = 0,
                    Explanation = explanation,
                    IsActive = true,
                    IsBilling = true,
                    IsTransportFree = false,
                    OrderStatusId = 1,
                    PaymentTypeId = 1,
                    Price = (decimal?)Price,
                    RequesterUserId = ai.UserId,
                    RequesterAddressId = Convert.ToInt32(ai.SelectedAdress.Id),
                    TransportTypeId = transportTypeSelect
                };
                _ios.InsertOrder(order);

                var orderMail = string.Empty;
                var orderMail2 = "<table style='border:1px solid;'><tr><td> Sipariş Tarihi : </td><td>" + currentDate
                                 + "</td></tr><tr><td> Sipariş No : </td><td>" + order.Id + "</td></tr><tr><td> Sipariş içeriği ; </td></tr>";

                for (var k = 0; k < basket.Rows.Count; k++)
                {
                    var productId = Convert.ToInt64(basket.Rows[k].ItemArray[0]);
                    var productName = basket.Rows[k].ItemArray[1].ToString();
                    var orderId = order.Id;
                    var quantity = Convert.ToInt32(basket.Rows[k].ItemArray[2]);
                    var pric = Convert.ToDecimal(basket.Rows[k].ItemArray[3]);
                    var productGroup = Convert.ToInt32(basket.Rows[k].ItemArray[4]);
                    var productGrup = basket.Rows[k].ItemArray[5].ToString();
                    var productCatg = basket.Rows[k].ItemArray[6].ToString();
                    var productMark = basket.Rows[k].ItemArray[7].ToString();

                    var oDet = new OrderDetails()
                    {
                        OrderId = orderId,
                        ProductGrupId = productGroup,
                        ProductId = productId,
                        ProductPrice = pric,
                        Quantity = quantity
                    };
                    _ios.InsertOrderDetail(oDet);
                    _uow.SavaChange();

                    orderMail = orderMail + "<tr><td>" + (k + 1) + ". parça :</td></tr><tr><td> Parça No : </td><td>" + productId + "</td><tr><td>Parça Markası :</td><td>" + productMark
                                + "</td></tr><tr><td>Parça Kategorisi :</td><td>" + productCatg
                                + "</td></tr><tr><td> Praça Grubu : </td><td>" + productGrup
                                + "</td></tr><tr><td> Parça Adı : </td><td>" + productName + "</td></tr><tr><td> Parça Sayısı : </td><td>" + quantity
                                + " adet </td></tr><tr><td> Parça Birim Fiyatı : </td><td>" + pric + " TL</td></tr><tr><td> Parça Toplam Fiyatı : </td><td>" + pric * quantity + " TL</td></tr><tr><td>" + "------------------------------------------------" + "</td></tr>";
                }

                orderMail = orderMail2 + orderMail + "<tr><td> <b>Toplam Sipariş Tutarı : </td><td>" + Price + " TL</b></td></tr><tr><td>" + "***********************************************" + "</td></tr><tr><td> Sipariş Açıklaması : </td><td>"
                            + explanation + "</td></tr><tr><td> Sipariş İrtibat Telefon Numarası : </td><td>" + relatedPhone
                            + "</td></tr><tr><td> Sipariş Gönderi Tipi : </td><td>" + transportType
                            + "</td></tr><tr><td> Sipariş Gönderilecek Adres : </td><td>" + ai.SelectedAdress.Address
                            + "</td></tr></table>";

                HttpContext.Session["basket"] = null;

                var payInf = "Ödeme başarısız!";
                if (isPaymentSuccess == 1)
                {
                    payInf = "Ödeme başarılı!";
                }

                var subject = "Yeni Sipariş ! " + payInf;

                var body = orderMail;

                try
                {
                    SendEMail(subject, body);
                    TempData["Message"] = "Mail Gönderildi.";
                }
                catch (Exception ex)
                {
                    TempData["Message"] = "Mail gönderilirken hata oluştu." + ex.Message;
                }

                var isCompany = Convert.ToInt32(collection.Get("IsCompany")) != 0;

                var bill = new Bills()
                { Address = collection.Get("address"),
                    CityId = Convert.ToInt32(collection.Get("dropDownSelectedCity")),
                    CompanyName = collection.Get("companyName"),
                    Email = collection.Get("email"),
                    FaxNumber = collection.Get("fax"),
                    GsmNumber = collection.Get("relatedPhone"),
                    IdentityNumber = collection.Get("identificationNum"),
                    IsCompany = isCompany,
                    Name = collection.Get("name"),
                    OrderId = order.Id,
                    OrderNote = collection.Get("orderExplanation"),
                    PhoneNumber = collection.Get("phone2"),
                    SubDistrictId = Convert.ToInt32(collection.Get("SubDistrictId")),
                    Surname = collection.Get("surname"),
                    TaxNumber = collection.Get("taxNum"),
                    TaxOffice = collection.Get("taxOffice"),
                    TownId = Convert.ToInt32(collection.Get("TownId"))
                };
                _ios.InsertBills(bill);

                TempData["ErrorMessages"] = _errorCode + " ~ " + _errorMessage;
                return View();
            }
            TempData["ErrorMessages"] = _errorCode + " ~ " + _errorMessage;
            Debug.Assert(HttpContext.Request.UrlReferrer != null, "HttpContext.Request.UrlReferrer != null");
            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }

        public int PayWithVakifBank(PosForm pf)
        {
            try
            {
                var b = new byte[5000];
                string _result;
                var encoding = Encoding.GetEncoding("ISO-8859-9");

                var priceBonvert = pf.Price.ToString("0000000000.00").Replace(",", string.Empty);

                var installmentsConvert = pf.Installments == -1 ? "00" : String.Format("{0:00}", pf.Installments);

                var yearConvert = pf.Year.ToString();
                yearConvert = yearConvert.Substring(2, 2);

                var monthConvert = string.Format("{0:00}", pf.Month);
                var khipForIp = GetIp();
                var provizyonMesaji = "kullanici=" + _companyPosUser + "&sifre=" + _companyPosPwd + "&islem=PRO&uyeno=" + _companyMemberNo + "&posno=" + _companyPosNo + "&kkno=" + pf.CardNumber + "&gectar=" + yearConvert + monthConvert + "&cvc=" + string.Format("{0:000}", pf.SecureCode) + "&tutar=" + priceBonvert + "&provno=000000&taksits=" + installmentsConvert + "&islemyeri=I&uyeref=" + Helper.RandomNumber() + "&vbref=6527BB1815F9AB1DE864A488E5198663002D0000&khip=" + khipForIp + "&xcip=" + _companyXcip;

                b.Initialize();
                b = Encoding.ASCII.GetBytes(provizyonMesaji);

                var h1 = WebRequest.Create("https://subesiz.vakifbank.com.tr/vpos724v3/?" + provizyonMesaji);
                h1.Method = "GET";

                var wr = h1.GetResponse();
                var s2 = wr.GetResponseStream();

                var buffer = new byte[10000];
                var len = 0;
                var r = 1;
                while (r > 0)
                {
                    Debug.Assert(s2 != null, "s2 != null");
                    r = s2.Read(buffer, len, 10000 - len);
                    len += r;
                }
                s2.Close();
                _result = encoding.GetString(buffer, 0, len).Replace("\r", string.Empty).Replace("\n", string.Empty);


                XmlNode node = null;
                var msgTemplate = new XmlDocument();
                msgTemplate.LoadXml(_result);
                node = msgTemplate.SelectSingleNode("//Cevap/Msg/Kod");
                Debug.Assert(node != null, "node != null");
                var incomingMonthCode = node.InnerText;

                if (incomingMonthCode == "00")
                {
                    node = msgTemplate.SelectSingleNode("//Cevap/Msg/ProvNo");
                    Debug.Assert(node != null, "node != null");
                    var incomingRefCode = node.InnerText;
                    this._result = true;
                    _refferenceNo = incomingRefCode;
                }
                else
                {
                    this._result = false;
                    _errorMessage = string.Empty;
                    _errorCode = incomingMonthCode;
                    switch (_errorCode)
                    {
                        case "97":
                            _errorMessage = "Sunucu ip adresini Bankanızla paylaşınız...";
                            break;
                        case "F2":
                            _errorMessage = "Sanal POS inaktif durumda...";
                            break;
                        case "G0":
                            _errorMessage = "Bankanızı Arayınız veya vpos724@vakifbank.com.tr adresine hatayı iletiniz..02124736060`I ARAYINIZ..";
                            break;
                        case "G5":
                            _errorMessage = "Yapılan işlemin Sanal POS ta yetkisi yok....";
                            break;
                        case "78":
                            _errorMessage = "Ödeme denen kart ile taksitli işlem gerçekleştirilemez....";
                            break;
                        case "81":
                            _errorMessage = "CVV ya da CAVV bilgisinin gönderilmediği durumda alınır..Eksik güvenlik Bilgisi..";
                            break;
                        case "83":
                            _errorMessage = "Sistem günsonuna girmeyen işlem iade edilmek isteniyor, işlem iptal edilebilir ya da ancak ertesi gün iade edilebilir....";
                            break;
                        case "01":
                            _errorMessage = "Eski Kayıt...";
                            break;
                    }
                    return 0;
                }
            }
            catch (Exception e)
            {
                _result = false;
                _errorMessage = e.Message;
                return 0;
            }

            return 1;
        }

        public string GetIp()
        {
            var ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ip))
            {
                ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            return ip;
        }
    }

    public class PosForm
    {
        public string CardOwner { get; set; }
        public long CardNumber { get; set; }
        public int SecureCode { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public double Price { get; set; }
        public int Installments { get; set; }
    }

    public class Helper
    {
        public static string GetIp()
        {
            if (string.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables["remote_addr"]))
            {
                return "127.0.0.1";
            }
            return HttpContext.Current.Request.ServerVariables["remote_addr"];
        }

        public static bool IsNumeric(string value)
        {
            int result;
            return int.TryParse(value, out result);
        }

        public static bool IsTcKimlik(string tcKimlikNo)
        {
            var returnvalue = false;

            if (tcKimlikNo.Length == 11)
            {
                var charlar = tcKimlikNo.ToCharArray(0, 10);
                var sayi = 0;

                foreach (var item in charlar)
                {
                    sayi += int.Parse(item.ToString());
                }
                var sayistr = sayi.ToString();

                if (sayistr.Substring(sayistr.Length - 1) == tcKimlikNo.Substring(10))
                {
                    returnvalue = true;
                }
            }
            return returnvalue;
        }

        public static string RandomNumber()
        {
            var r = new Random();
            var strRsayi = r.Next(1, 10000000) + String.Format("{0:T}", DateTime.Now).Replace(":", string.Empty);
            return strRsayi;
        }
    }
}
