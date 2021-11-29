//using System;
//using System.Diagnostics;
//using System.Globalization;
//using System.Net;
//using System.Net.Mail;
//using System.Text;
//using System.Web.Mvc;
//using System.Xml;

//namespace Mvc4WebRole.Controllers
//{
//    public class SedaController : Controller
//    {
//        private const string CompanyPosUser = "0001";
//        private const string CompanyPosPwd = "18367854";
//        private const string CompanyMemberNo = "000089094";
//        private const string CompanyPosNo = "VP008908";
//        private const string CompanyXcip = "SJU6NL7ZXM";

//        private const string SystemError = "Bankayla bağlantı kurulamadı ! Lütfen daha sonra tekrar deneyin.";

//        public bool Result { get; set; }
//        public string ErrorMessage { get; set; }
//        public string ErrorCode { get; set; }

//        public string Code { get; set; }
//        public string GroupId { get; set; }
//        public string TransId { get; set; }
//        public string RefferenceNo { get; set; }

//        private static void SendEMail(string subject, string body)
//        {
//            var client = new SmtpClient
//            {
//                DeliveryMethod = SmtpDeliveryMethod.Network,
//                EnableSsl = true,
//                Host = "smtp.gmail.com",
//                Port = 587
//            };

//            var credentials = new NetworkCredential("oto343434@gmail.com", "oto123456");
//            client.UseDefaultCredentials = false;
//            client.Credentials = credentials;

//            var msg = new MailMessage { From = new MailAddress("otomotivist@gmail.com") };
//            msg.To.Add(new MailAddress("h.sag_58@hotmail.com"));

//            msg.Subject = subject;
//            msg.IsBodyHtml = true;
//            msg.Body = body;

//            client.Send(msg);
//        }

//        public ActionResult SedaMakinaSiparis(FormCollection collection)
//        {
//            return View();
//        }

//        public ActionResult ConfirmOrder(FormCollection collection)
//        {
//            var txtManyInstallment = collection.Get("txtManyInstallment") == string.Empty ? "0" : collection.Get("txtManyInstallment");

//            var price = Convert.ToDecimal(Session["TotalPrice"]);
//            var cardNumber = Convert.ToInt64(collection.Get("txtCardNumber"));
//            var cardOwner = collection.Get("txtCardOwnerName");
//            var month = Convert.ToInt32(collection.Get("hdnSelectedMonth"));
//            var year = Convert.ToInt32(collection.Get("hdnSelectedYear"));
//            var secureCode = Convert.ToInt32(collection.Get("txtCVV2"));
//            var Price = Convert.ToDouble(price);
//            var installments = Convert.ToInt32(txtManyInstallment);

//            var pForm = new PosForm
//            {
//                CardNumber = cardNumber,
//                CardOwner = cardOwner,
//                Month = month,
//                Year = year,
//                SecureCode = secureCode,
//                Price = Price,
//                Installments = installments
//            };

//            var isPaymentSuccess = PayWithVakifBank(pForm);
//            if (isPaymentSuccess == 1 || isPaymentSuccess == 0)
//            {
//                var currentDate = DateTime.Now;

//                var orderMail =  "<table style='border:1px solid;'><tr><td> Sipariş Tarihi : </td><td>" + currentDate + "</td></tr></table>";

//                var payInf = "Ödeme başarısız!";
//                if (isPaymentSuccess == 1)
//                {
//                    payInf = "Ödeme başarılı!";
//                }

//                var subject = "Yeni Sipariş ! " + payInf;

//                var body = orderMail;

//                try
//                {
//                    SendEMail(subject, body);
//                    TempData["Message"] = "Mail Gönderildi.";
//                }
//                catch (Exception ex)
//                {
//                    TempData["Message"] = "Mail gönderilirken hata oluştu." + ex.Message;
//                }

//                TempData["ErrorMessages"] = ErrorCode + " ~ " + ErrorMessage;
//                return View("SedaMakinaSiparis");
//            }
//            TempData["ErrorMessages"] = ErrorCode + " ~ " + ErrorMessage;
//            Debug.Assert(HttpContext.Request.UrlReferrer != null, "HttpContext.Request.UrlReferrer != null");
//            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
//        }

//        public int PayWithVakifBank(PosForm pf)
//        {
//            try
//            {
//                var b = new byte[5000];
//                var encoding = Encoding.GetEncoding("ISO-8859-9");

//                var priceBonvert = pf.Price.ToString("0000000000.00").Replace(",", string.Empty);

//                var installmentsConvert = pf.Installments == -1 ? "00" : String.Format("{0:00}", pf.Installments);

//                var yearConvert = pf.Year.ToString(CultureInfo.InvariantCulture);
//                yearConvert = yearConvert.Substring(2, 2);

//                var monthConvert = string.Format("{0:00}", pf.Month);
//                var khipForIp = GetIp();
//                var provizyonMesaji = "kullanici=" + CompanyPosUser + "&sifre=" + CompanyPosPwd + "&islem=PRO&uyeno=" + CompanyMemberNo + "&posno=" + CompanyPosNo + "&kkno=" + pf.CardNumber + "&gectar=" + yearConvert + monthConvert + "&cvc=" + string.Format("{0:000}", pf.SecureCode) + "&tutar=" + priceBonvert + "&provno=000000&taksits=" + installmentsConvert + "&islemyeri=I&uyeref=" + Helper.RandomNumber() + "&vbref=6527BB1815F9AB1DE864A488E5198663002D0000&khip=" + khipForIp + "&xcip=" + CompanyXcip;

//                b.Initialize();
//                b = Encoding.ASCII.GetBytes(provizyonMesaji);

//                var h1 = WebRequest.Create("https://subesiz.vakifbank.com.tr/vpos724v3/?" + provizyonMesaji);
//                h1.Method = "GET";

//                var wr = h1.GetResponse();
//                var s2 = wr.GetResponseStream();

//                var buffer = new byte[10000];
//                var len = 0;
//                var r = 1;
//                while (r > 0)
//                {
//                    Debug.Assert(s2 != null, "s2 != null");
//                    r = s2.Read(buffer, len, 10000 - len);
//                    len += r;
//                }
//                if (s2 != null)
//                {
//                    s2.Close();
//                }
//                var result = encoding.GetString(buffer, 0, len).Replace("\r", string.Empty).Replace("\n", string.Empty);


//                var msgTemplate = new XmlDocument();
//                msgTemplate.LoadXml(result);
//                var node = msgTemplate.SelectSingleNode("//Cevap/Msg/Kod");
//                Debug.Assert(node != null, "node != null");
//                var incomingMonthCode = node.InnerText;

//                if (incomingMonthCode == "00")
//                {
//                    node = msgTemplate.SelectSingleNode("//Cevap/Msg/ProvNo");
//                    Debug.Assert(node != null, "node != null");
//                    var incomingRefCode = node.InnerText;
//                    Result = true;
//                    RefferenceNo = incomingRefCode;
//                }
//                else
//                {
//                    Result = false;
//                    ErrorMessage = string.Empty;
//                    ErrorCode = incomingMonthCode;
//                    switch (ErrorCode)
//                    {
//                        case "97":
//                            ErrorMessage = "Sunucu ip adresini Bankanızla paylaşınız...";
//                            break;
//                        case "F2":
//                            ErrorMessage = "Sanal POS inaktif durumda...";
//                            break;
//                        case "G0":
//                            ErrorMessage = "Bankanızı Arayınız veya vpos724@vakifbank.com.tr adresine hatayı iletiniz..02124736060`I ARAYINIZ..";
//                            break;
//                        case "G5":
//                            ErrorMessage = "Yapılan işlemin Sanal POS ta yetkisi yok....";
//                            break;
//                        case "78":
//                            ErrorMessage = "Ödeme denen kart ile taksitli işlem gerçekleştirilemez....";
//                            break;
//                        case "81":
//                            ErrorMessage = "CVV ya da CAVV bilgisinin gönderilmediği durumda alınır..Eksik güvenlik Bilgisi..";
//                            break;
//                        case "83":
//                            ErrorMessage = "Sistem günsonuna girmeyen işlem iade edilmek isteniyor, işlem iptal edilebilir ya da ancak ertesi gün iade edilebilir....";
//                            break;
//                        case "01":
//                            ErrorMessage = "Eski Kayıt...";
//                            break;
//                    }
//                    return 0;
//                }
//            }
//            catch (Exception e)
//            {
//                Result = false;
//                ErrorMessage = SystemError + e.Message;
//                return 0;
//            }

//            return 1;
//        }

//        public string GetIp()
//        {
//            var ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
//            if (string.IsNullOrEmpty(ip))
//            {
//                ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
//            }
//            return ip;
//        }
//    }
//}
