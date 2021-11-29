using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
//using System.Web.Security;
//using Microsoft.Web.WebPages.OAuth;
//using WebMatrix.WebData;
using System.Net.Mail;
using System.Net;
using System.Web.UI;
using System.Configuration;
using BusinessObjects;
using Dal;
using Otomotivist.Domain.UnitOfWork;
using Otomotivist.Domain.Repository;
using Otomotivist.Service.interfaces;
using Otomotivist.Service.services;
using Mvc4WebRole.Filters;
using Otomotivist.Auth;
using Newtonsoft.Json;
using System.Web.Security;
using Otomotivist.Auth.Secure;


namespace Mvc4WebRole.Controllers
{
    [CustomAuthorizeAttribute]
    public class AccountController : BaseController
    {

        private readonly IUnitOfWork _uow;
        private readonly IAccountService _acs;
        private readonly IOtomotivistCoreService _ocs;


        public AccountController(IUnitOfWork uow, IAccountService acs, IOtomotivistCoreService os)
        {
            _uow = uow;
            _acs = acs;
            _ocs = os;
        }

        [AllowAnonymous]
        [OutputCache(Duration = 3600, VaryByParam = "none", Location = OutputCacheLocation.Client, NoStore = true)]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }



        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [OutputCache(Duration = 3600, VaryByParam = "none", Location = OutputCacheLocation.Client, NoStore = true)]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                UserProfile user = _acs.GetUserProfileByNameAndPwd(model.UserName, model.Password);
                //var user = Context.Users.Where(u => u.Username == model.Username && u.Password == model.Password).FirstOrDefault();
                if (user != null)
                {
                    Session["UserIdInSession"] = user.UserId;
                    var roles = _acs.GetUserRoles(user.UserId);

                    CustomPrincipalSerializeModel serializeModel = new CustomPrincipalSerializeModel();
                    serializeModel.UserId = user.UserId;
                    serializeModel.FirstName = user.Name;
                    serializeModel.LastName = user.Surname;
                    serializeModel.roles = roles.ToArray();

                    string userData = JsonConvert.SerializeObject(serializeModel);
                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                             1,
                            user.eMail,
                             DateTime.Now,
                             DateTime.Now.AddMinutes(30),
                             false,
                             userData);

                    string encTicket = FormsAuthentication.Encrypt(authTicket);
                    HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                    Response.Cookies.Add(faCookie);

                    #region Attempt_to_register_the_user
                    try
                    {
                        string usernameinv = model.UserName.ToLowerInvariant().Trim();
                        var up = _acs.GetUserProfileByName(usernameinv);

                        var usrAddress = new UserAdresses()
                        {
                            Address = "",
                            UserId = _acs.GetUserProfileByName(usernameinv).UserId
                        };

                        Session["UserIdInSession"] = usrAddress.UserId;

                        HttpContext.Session["hasLocalAccount"] = true;
                        
                        var ua = _ocs.GetUserAddressesById(up.UserId).ToList();
                        _acs.InsertUserAddress(usrAddress);
                        var ai = new AccountInformation
                        {
                            UserId = up.UserId,
                            UserName = usernameinv,
                            UserRealName = up.Name,
                            UserSurname = up.Surname,
                            UserProfilePhoto = up.ProfilePhoto,
                            UserGender = up.Gender,
                            UserEducationLevel = _ocs.GetUserEducationLevelsById(up.EducationLevel) != null ? _ocs.GetUserEducationLevelsById(up.EducationLevel).FirstOrDefault().EducationLevel : "",
                            UserJob = _ocs.GetUserJobsById(up.Job).FirstOrDefault() != null ? _ocs.GetUserJobsById(up.Job).FirstOrDefault().Job : "",
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
                        var ugs = _ocs.GetGalleries(up.UserId).ToList(); //k.id yerine k.AutherId olmalı, tabloda alan eksik !!!
                        ai.UserGalleries = ugs;
                        var ords = _ocs.GetOrders(up.UserId).ToList();
                        ai.UserOrders = ords;
                        var prdcs = _ocs.GetProducts(up.UserId).OrderByDescending(m => m.Id).Take(20).ToList();
                        ai.UserProducts = prdcs;
                        var prdcGrps = _ocs.GetProductGroupsAll().ToList();
                        ai.PrdcGrpList = prdcGrps;
                        var prdcCats = _ocs.GetCategoriesAll().ToList();
                        ai.PrdcCatList = prdcCats;
                        var prdcMarks = _ocs.GetMarksAll().ToList();
                        ai.PrdcMarkList = prdcMarks;
                        var prdcCurrs = _ocs.GetCurrenciesAll().ToList();
                        ai.PrdcCurrList = prdcCurrs;
                        //List<Friend> friends = context.Friends.Where(k => k.UserId == up.UserId).ToList();
                        ////ai.Friends = friends;
                        var uaddr = new List<UserAdresses>();
                        var drs = new UserAdresses() { Address = "", Id = 1999999999 };
                        uaddr.Add(drs);
                        ai.Adress = (ua.Count == 0 || ua[0].Address == null) ? uaddr : ua;
                        if (ua != null && ua.Count > 0)
                            ai.SelectedAdress = ua[0];
                        HttpContext.Session["AccountInformation"] = ai;

                        LoginModel lm = new LoginModel()
                        {
                            UserName = usernameinv,
                            Password = model.Password
                        };
                    }
                    catch (MembershipCreateUserException e)
                    {
                        ModelState.AddModelError("userName", e.Message);
                    }
                    #endregion

                    if (roles.Contains("Admin"))
                    {
                        return RedirectToAction("UserManagement", "Admin");
                    }
                    else
                    {
                        return RedirectToAction("yeniikinciel", "otomobilvasita");
                    }

                }

                ModelState.AddModelError("", "Incorrect username and/or password");
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            HttpContext.Session.Abandon();

            return RedirectToAction("yeniikinciel", "otomobilvasita");
        }

        public ActionResult LogOffOff()
        {
            FormsAuthentication.SignOut();
            HttpContext.Session.Abandon();

            return RedirectToAction("yeniikinciel", "otomobilvasita");
        }

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(string eMail)
        {
            var User = _acs.GetUserProfileByEmail(eMail);
            //check user existance
            var user = _acs.UserExists(User.UserName);
            if (user == false)
            {
                if (string.IsNullOrEmpty(eMail))
                {
                    TempData["Message"] = "*Bu alan boş bırakılamaz.";
                }
                else
                {
                    TempData["Message"] = "*Bu mail ile kayıtlı bir kullanıcı bulunmamakta.";
                }
            }
            else
            {
                //generate password token
                var token = Guid.NewGuid();
                _acs.CretateTokenToUser(User.UserId, token.ToString());

                //create url with above token
                var resetLink = "<a href='" + Url.Action("ResetPassword", "Account", new { un = User.UserName, rt = token }, "http") + "'>Şifreni yenile</a>";
                var resetLinkString = Url.Action("ResetPassword", "Account", new { un = User.UserName, rt = token }, "http");
                //get user emailid
                var emailid = User.eMail;
                //send mail
                const string subject = "Şifre Sıfırlama Anahtarı";
                var body = "<b>Lütfen Şifre sıfırlama anahtarını bulun</b><br/>" + resetLink + "<br/> Eğer linki göremiyorsanız aşağıdaki adresi kopyalayıp adres çubuğuna yapıştırın ve gidin.<br/>" + resetLinkString;
                try
                {
                    SendEMail(emailid, subject, body);
                    TempData["Message"] = "Mail Gönderildi.";
                }
                catch (Exception ex)
                {
                    TempData["Message"] = "Mail gönderilirken hata oluştu." + ex.Message;
                }
            }

            return View();
        }

        private static string GenerateRandomPassword(int length)
        {
            const string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
            var chars = new char[length];
            var rd = new Random();
            for (var i = 0; i < length; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }
            return new string(chars);
        }

        private static void SendEMail(string emailid, string subject, string body)
        {
            SmtpClient client = new SmtpClient(ConfigurationManager.AppSettings["mailServer"].ToString(), Convert.ToInt32(587));
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["mailAccount"].ToString(), ConfigurationManager.AppSettings["mailPassword"].ToString());
            client.UseDefaultCredentials = false;
            client.EnableSsl = true;
            client.Credentials = credentials;

            try
            {
                var msg = new MailMessage { From = new MailAddress(ConfigurationManager.AppSettings["mailAccount"].ToString()) };
                msg.To.Add(new MailAddress(emailid));
                msg.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;

                msg.Subject = subject;
                msg.IsBodyHtml = true;
                msg.Body = body;

                client.Send(msg);
            }
            catch (SmtpFailedRecipientException ex)
            {
                throw ex;
            }
        }

        //[AllowAnonymous]
        //public ActionResult ResetPassword()
        //{
        //    return View();
        //}

        [AllowAnonymous]
        public ActionResult ResetPassword(ResetPassword model)
        {
            if(model.ConfirmPassword == null || model.Password != model.ConfirmPassword)
                return View();

            string token = HttpContext.Request.QueryString["rt"];
            _acs.SetUserByToken(token, model.Password);

            return RedirectToAction("yeniikinciel", "otomobilvasita");
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            //if (!WebSecurity.Initialized)
            //{
            //    WebSecurity.InitializeDatabaseConnection("EntitiesMember", "UserProfile", "UserId", "UserName", autoCreateTables: true);
            //}
            return View();
        }

        public static HttpCookie GetAuthenticationCookie(string UserName, bool persistLogin)
        {
            var guid = Guid.NewGuid();
            var userData = guid.ToString();

            var authTicket = new FormsAuthenticationTicket(
                     1,
                     UserName,
                     DateTime.Now,
                     DateTime.Now.AddMinutes(20),
                     persistLogin,
                     userData);

            var encTicket = FormsAuthentication.Encrypt(authTicket);
            return new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = _acs.GetUserProfileByNameOrEmail(model.eMail, model.UserName);
            if (user == null)
            {
                #region Attempt_to_register_the_user
                try
                {
                    string usernameinv = model.UserName.ToLowerInvariant().Trim();
                    UserProfile confirmationToken = new UserProfile()
                    {
                        eMail = model.eMail.ToLowerInvariant(),
                        GSM = model.Phone,
                        Name = model.Name,
                        Surname = model.SurName,
                        UserName = usernameinv,
                        Password = model.Password
                    };
                    _acs.AddUserProfileWithUserRole(confirmationToken);

                    var usrAddress = new UserAdresses()
                    {
                        Address = "",
                        UserId = _acs.GetUserProfileByName(usernameinv).UserId
                    };

                    Session["UserIdInSession"] = usrAddress.UserId;

                    HttpContext.Session["hasLocalAccount"] = true;

                    var up = _acs.GetUserProfileByName(usernameinv);
                    var ua = _ocs.GetUserAddressesById(up.UserId).ToList();
                    up.EducationLevel = 7; //set default education level
                    up.Job = 79; // set default job
                    _acs.InsertUserAddress(usrAddress);
                    var ai = new AccountInformation
                    {
                        UserId = up.UserId,
                        UserName = usernameinv,
                        UserRealName = up.Name,
                        UserSurname = up.Surname,
                        UserProfilePhoto = up.ProfilePhoto,
                        UserGender = up.Gender,
                        UserEducationLevel = _ocs.GetUserEducationLevelsById(up.EducationLevel) != null ? _ocs.GetUserEducationLevelsById(up.EducationLevel).FirstOrDefault().EducationLevel : "",
                        UserJob = _ocs.GetUserJobsById(up.Job).FirstOrDefault() != null ? _ocs.GetUserJobsById(up.Job).FirstOrDefault().Job : "",
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
                    var ugs = _ocs.GetGalleries(up.UserId).ToList(); //k.id yerine k.AutherId olmalı, tabloda alan eksik !!!
                    ai.UserGalleries = ugs;
                    var ords = _ocs.GetOrders(up.UserId).ToList();
                    ai.UserOrders = ords;
                    var prdcs = _ocs.GetProducts(up.UserId).OrderByDescending(m => m.Id).Take(20).ToList();
                    ai.UserProducts = prdcs;
                    var prdcGrps = _ocs.GetProductGroupsAll().ToList();
                    ai.PrdcGrpList = prdcGrps;
                    var prdcCats = _ocs.GetCategoriesAll().ToList();
                    ai.PrdcCatList = prdcCats;
                    var prdcMarks = _ocs.GetMarksAll().ToList();
                    ai.PrdcMarkList = prdcMarks;
                    var prdcCurrs = _ocs.GetCurrenciesAll().ToList();
                    ai.PrdcCurrList = prdcCurrs;
                    //List<Friend> friends = context.Friends.Where(k => k.UserId == up.UserId).ToList();
                    ////ai.Friends = friends;
                    var uaddr = new List<UserAdresses>();
                    var drs = new UserAdresses() { Address = "", Id = 1999999999 };
                    uaddr.Add(drs);
                    ai.Adress = (ua.Count == 0 || ua[0].Address == null) ? uaddr : ua;
                    if (ua != null && ua.Count > 0)
                        ai.SelectedAdress = ua[0];
                    HttpContext.Session["AccountInformation"] = ai;

                    LoginModel lm = new LoginModel() {
                        UserName = usernameinv,
                        Password = model.Password
                    };
                    return RedirectToAction("Login","Account", new {model=lm, returnUrl="" });
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("userName", e.Message);
                }
                #endregion
            }
            else
            {
                ModelState.AddModelError("eMail", "Bu mail adresi veya kullanıcı adı zaten sistemimizde kayıtlı. Şifrenizi unuttuysanız lütfen yenileyin...");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Disassociate(string provider, string providerUserId)
        {
            //var ownerAccount = OAuthWebSecurity.GetUserName(provider, providerUserId);
            //ManageMessageId? message = null;

            //// Only disassociate the account if the currently logged in user is the owner
            //if (ownerAccount == User.Identity.Name)
            //{
            //    // Use a transaction to prevent the user from deleting their last login credential
            //    using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
            //    {
            //        var hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            //        if (hasLocalAccount || OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name).Count > 1)
            //        {
            //            OAuthWebSecurity.DeleteAccount(provider, providerUserId);
            //            scope.Complete();
            //            message = ManageMessageId.RemoveLoginSuccess;
            //        }
            //    }
            //}

            return RedirectToAction("Manage", new { Message = "message" });
        }

        public ActionResult Manage(ManageMessageId? message)
        {
            //ViewBag.StatusMessage =
            //    message == ManageMessageId.ChangePasswordSuccess ? "Şifreniz değiştirildi."
            //    : message == ManageMessageId.SetPasswordSuccess ? "Şifreniz ayarlandı."
            //    : message == ManageMessageId.RemoveLoginSuccess ? "Harici giriş kaldırılmıştır."
            //    : "";
            //ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            //ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(LocalPasswordModel model)
        {
            //var hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            //ViewBag.HasLocalPassword = hasLocalAccount;
            //ViewBag.ReturnUrl = Url.Action("Manage");
            //if (hasLocalAccount)
            //{
            //    if (ModelState.IsValid)
            //    {
            //        // ChangePassword will throw an exception rather than return false in certain failure scenarios.
            //        bool changePasswordSucceeded;
            //        try
            //        {
            //            changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
            //        }
            //        catch (Exception)
            //        {
            //            changePasswordSucceeded = false;
            //        }

            //        if (changePasswordSucceeded)
            //        {
            //            return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
            //        }
            //        ModelState.AddModelError("", "Geçerli şifre yanlış ya da yeni şifre geçersiz karakter içeriyor.");
            //    }
            //}
            //else
            //{
            //    // User does not have a local password so remove any validation errors caused by a missing
            //    // OldPassword field
            //    var state = ModelState["OldPassword"];
            //    if (state != null)
            //    {
            //        state.Errors.Clear();
            //    }

            //    if (ModelState.IsValid)
            //    {
            //        try
            //        {
            //            WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
            //            return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
            //        }
            //        catch (Exception)
            //        {
            //            ModelState.AddModelError("", String.Format("Yerel hesap oluşturulamıyor. Aynı isimde \"{0}\" bir hesap zaten var olabilir.", User.Identity.Name));
            //        }
            //    }
            //}

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public ActionResult ExternalLogin(string provider, string returnUrl)
        //{
        //    return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        //}

        //[AllowAnonymous]
        //public ActionResult ExternalLoginCallback(string returnUrl)
        //{
        //    var result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        //    if (!result.IsSuccessful)
        //    {
        //        return RedirectToAction("ExternalLoginFailure");
        //    }

        //    if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, false))
        //    {
        //        return RedirectToLocal(returnUrl);
        //    }

        //    if (User.Identity.IsAuthenticated)
        //    {
        //        // If the current user is logged in add the new account
        //        OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, User.Identity.Name);
        //        return RedirectToLocal(returnUrl);
        //    }
        //    // User is new, ask for their desired membership name
        //    //var loginData = OAuthWebSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId);
        //    ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(result.Provider).DisplayName;
        //    ViewBag.ReturnUrl = returnUrl;
        //    //return View("ExternalLoginConfirmation", new RegisterExternalLoginModel { UserName = result.UserName, ExternalLoginData = loginData });

        //    ExternalLoginConfirmation(new RegisterExternalLoginModel
        //    {
        //        UserName = result.UserName,
        //        Password = result.ProviderUserId,
        //        ConfirmPassword = result.ProviderUserId,
        //        Mail = result.UserName,
        //        ExternalLoginData = result.Provider
        //    }, returnUrl);
        //    return RedirectToAction(ViewBag.ReturnAction);
        //}

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
        {
            //var provider = model.ExternalLoginData;
            //var providerUserId = model.Password;
            //model.ExternalLoginData = "";

            ////if (User.Identity.IsAuthenticated ||
            ////    !OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
            ////    return RedirectToAction("Manage");
            //if (User.Identity.IsAuthenticated)
            //    return RedirectToAction("Manage");
            //if (ModelState.IsValid)
            //{
            //    // Insert a new user into the database
            //    var user = _ocs.GetUserProfileByName(model.UserName.ToLower());
            //    // Check if user already exists
            //    if (user == null)
            //    {
            //        // Insert name into the profile table
            //        //db.UserProfiles.Add(new UserProfile {UserName = model.UserName});
            //        //db.SaveChanges();

            //        #region Attempt_to_register_the_user
            //        try
            //        {
            //            WebSecurity.CreateUserAndAccount(model.UserName, model.Password, new { eMail = model.Mail });//false value is from login model rememberMe value so it is false at register

            //            //WebSecurity.CreateAccount(model.UserName, model.Password);

            //            var usrAddress = new UserAdresses()
            //            {
            //                Address = "",
            //                UserId = _ocs.GetUserProfileByName(model.UserName).UserId
            //            };

            //            var isLogged = WebSecurity.Login(model.UserName, model.Password);
            //            HttpContext.Session["hasLocalAccount"] = isLogged;
            //            if (!isLogged)
            //                return RedirectToAction("yeniikinciel", "otomobilvasita");
            //            var up = _acs.GetUserProfileByName(model.UserName.Trim());
            //            var ua = _ocs.GetUserAddressesById(up.UserId).ToList();
            //            up.EducationLevel = 7; //set default education level
            //            up.Job = 79; // set default job
            //            _acs.InsertUserAddress(usrAddress);
            //            var ai = new AccountInformation
            //            {
            //                UserId = up.UserId,
            //                UserName = model.UserName.Trim(),
            //                UserRealName = up.Name,
            //                UserSurname = up.Surname,
            //                UserProfilePhoto = up.ProfilePhoto,
            //                UserGender = up.Gender,
            //                UserEducationLevel = _ocs.GetUserEducationLevelsById(up.EducationLevel) != null ? _ocs.GetUserEducationLevelsById(up.EducationLevel).FirstOrDefault().EducationLevel : "",
            //                UserJob = _ocs.GetUserJobsById(up.Job).FirstOrDefault() != null ? _ocs.GetUserJobsById(up.Job).FirstOrDefault().Job : "",
            //                UserGenderList = _ocs.GetUserGenders().ToList(),
            //                UserEducationLevelList = _ocs.GetUserEducationLevels().ToList(),
            //                UserJobList = _ocs.GetUserJobs().ToList(),
            //                UserCellPhone = up.GSM,
            //                UserHomePhone = up.HomePhone,
            //                UserWorkPhone = up.WorkPhone,
            //                UserFaxNumber = up.Fax,
            //                UserTcId = up.TCid,
            //                UserBirthDate = Convert.ToDateTime(up.DateOfBirth)
            //            };
            //            ai.UserProfilePhoto = up.ProfilePhoto;
            //            ai.UserEmail = up.eMail;
            //            ai.UserTypeId = Convert.ToInt32(up.UserTypeId ?? 0);
            //            var uas = _ocs.Getautoservices(up.UserId).ToList();
            //            ai.UserAutoServices = uas;
            //            var ugs = _ocs.GetGalleries(up.UserId).ToList(); //k.id yerine k.AutherId olmalı, tabloda alan eksik !!!
            //            ai.UserGalleries = ugs;
            //            var ords = _ocs.GetOrders(up.UserId).ToList();
            //            ai.UserOrders = ords;
            //            var prdcs = _ocs.GetProducts(up.UserId).OrderByDescending(m => m.Id).Take(20).ToList();
            //            ai.UserProducts = prdcs;
            //            var prdcGrps = _ocs.GetProductGroupsAll().ToList();
            //            ai.PrdcGrpList = prdcGrps;
            //            var prdcCats = _ocs.GetCategoriesAll().ToList();
            //            ai.PrdcCatList = prdcCats;
            //            var prdcMarks = _ocs.GetMarksAll().ToList();
            //            ai.PrdcMarkList = prdcMarks;
            //            var prdcCurrs = _ocs.GetCurrenciesAll().ToList();
            //            ai.PrdcCurrList = prdcCurrs;
            //            //List<Friend> friends = context.Friends.Where(k => k.UserId == up.UserId).ToList();
            //            ////ai.Friends = friends;
            //            var uaddr = new List<UserAdresses>();
            //            var drs = new UserAdresses() { Address = "", Id = 1999999999 };
            //            uaddr.Add(drs);
            //            ai.Adress = (ua.Count == 0 || ua[0].Address == null) ? uaddr : ua;
            //            if (ua != null && ua.Count > 0)
            //                ai.SelectedAdress = ua[0];
            //            HttpContext.Session["AccountInformation"] = ai;

            //            //Response.Cookies.Add(GetAuthenticationCookie(model.UserName.ToLower(), true));

            //            //return RedirectToAction("yeniikinciel", "otomobilvasita");
            //        }
            //        catch (MembershipCreateUserException)
            //        {
            //            ModelState.AddModelError("userName", "Bu kullanıcı adı zaten kayıtlı. Farklı bir kullanıcı adı seçin.");
            //        }
            //        #endregion

            //        OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, model.UserName);
            //        OAuthWebSecurity.Login(provider, providerUserId, false);

            //        return RedirectToLocal(returnUrl);
            //    }
            //    ModelState.AddModelError("UserName", "Kullanıcı adı zaten var. Farklı bir kullanıcı adı girin.");
            //}

            //ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
            ////ViewBag.ReturnUrl = returnUrl;
            //ViewBag.ReturnAction = "/Login";
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        //[AllowAnonymous]
        //[ChildActionOnly]
        //public ActionResult ExternalLoginsList(string returnUrl)
        //{
        //    ViewBag.ReturnUrl = returnUrl;
        //    return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
        //}

        //[ChildActionOnly]
        //public ActionResult RemoveExternalLogins()
        //{
        //    var accounts = OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name);
        //    var externalLogins = new List<ExternalLogin>();
        //    foreach (var account in accounts)
        //    {
        //        var clientData = OAuthWebSecurity.GetOAuthClientData(account.Provider);

        //        externalLogins.Add(new ExternalLogin
        //        {
        //            Provider = account.Provider,
        //            ProviderDisplayName = clientData.DisplayName,
        //            ProviderUserId = account.ProviderUserId
        //        });
        //    }

        //    ViewBag.ShowRemoveButton = externalLogins.Count > 1 || OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
        //    return PartialView("_RemoveExternalLoginsPartial", externalLogins);
        //}

        //[HttpPost]
        //public ActionResult DeleteAccount(FormCollection collection)
        //{
        //    var userName = WebSecurity.CurrentUserName;
        //    try
        //    {
        //        // TODO: Add delete logic here
        //        if (Roles.GetRolesForUser(userName).Any())
        //        {
        //            Roles.RemoveUserFromRoles(userName, Roles.GetRolesForUser(userName));
        //        }
        //        ((SimpleMembershipProvider)Membership.Provider).DeleteAccount(userName); // deletes record from webpages_Membership table
        //        Membership.Provider.DeleteUser(userName, true); // deletes record from UserProfile table

        //        return LogOff();
        //    }
        //    catch
        //    {
        //        return View(userName);
        //    }
        //}

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("yeniikinciel", "otomobilvasita");
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        //internal class ExternalLoginResult : ActionResult
        //{
        //    public ExternalLoginResult(string provider, string returnUrl)
        //    {
        //        Provider = provider;
        //        ReturnUrl = returnUrl;
        //    }

        //    public string Provider { get; private set; }
        //    public string ReturnUrl { get; private set; }

        //    //public override void ExecuteResult(ControllerContext context)
        //    //{
        //    //    OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
        //    //}
        //}

        //private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        //{
        //    // See http://go.microsoft.com/fwlink/?LinkID=177550 for
        //    // a full list of status codes.
        //    switch (createStatus)
        //    {
        //        case MembershipCreateStatus.DuplicateUserName:
        //            return "Kullanıcı adı zaten var. Farklı bir kullanıcı adı girin.";

        //        case MembershipCreateStatus.DuplicateEmail:
        //            return "Bu e-posta adresi için bir kullanıcı adı zaten var. Farklı bir e-posta adresi girin.";

        //        case MembershipCreateStatus.InvalidPassword:
        //            return "Girilen şifre geçersiz. Geçerli bir şifre değeri girin.";

        //        case MembershipCreateStatus.InvalidEmail:
        //            return "Girilen e-posta adresi geçersiz. Değerini kontrol edin ve tekrar deneyin.";

        //        case MembershipCreateStatus.InvalidAnswer:
        //            return "Girilen cevap geçersiz. Değerini kontrol edin ve tekrar deneyin.";

        //        case MembershipCreateStatus.InvalidQuestion:
        //            return "Girilen şifre hatırlatma sorusu geçersiz. Değerini kontrol edin ve tekrar deneyin.";

        //        case MembershipCreateStatus.InvalidUserName:
        //            return "Girilen kullanıcı adı geçersiz. Değerini kontrol edin ve tekrar deneyin.";

        //        case MembershipCreateStatus.ProviderError:
        //            return "Kimlik doğrulama sağlayıcısı bir hata döndürdü. Lütfen girişinizi doğrulayın ve yeniden deneyin. Sorun devam ederse, sistem yöneticinize başvurun.";

        //        case MembershipCreateStatus.UserRejected:
        //            return "Kullanıcı oluşturma isteği iptal edildi. Lütfen girişinizi doğrulayın ve yeniden deneyin. Sorun devam ederse, sistem yöneticinize başvurun.";

        //        default:
        //            return "Bilinmeyen bir hata oluştu. Lütfen girişinizi doğrulayın ve yeniden deneyin. Sorun devam ederse, sistem yöneticinize başvurun.";
        //    }
        //}
        #endregion
    }
}
