using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class RegisterExternalLoginModel
    {
        [Required(ErrorMessage = "Kullanıcı adı alanı gerekli.")]
        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "E-mail alanı gerekli.")]
        [EmailAddress]
        [Display(Name = "E-mail")]
        public string Mail { get; set; }

        [Required(ErrorMessage = "Şifre alanı gerekli.")]
        [StringLength(100, ErrorMessage = "{0} en az {2} karakter uzunluğunda olmalı.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Şifreyi dogrula")]
        [Compare("Password", ErrorMessage = "Şifre ve doğrulama şifresi eşleşmiyor.")]
        public string ConfirmPassword { get; set; }

        public string ExternalLoginData { get; set; }
    }

    public class LocalPasswordModel
    {
        [Required(ErrorMessage = "Şifre alanı gerekli.")]
        [DataType(DataType.Password)]
        [Display(Name = "Mevcut şifre")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Yeni şifre alanı gerekli.")]
        [StringLength(100, ErrorMessage = "{0} en az {2} karakter uzunluğunda olmalı.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Yeni şifre")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Yeni şifreyi dogrula")]
        [Compare("NewPassword", ErrorMessage = "Yeni şifre ve doğrulama şifresi uyuşmuyor.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel
    {
        [Required(ErrorMessage = "Kullanıcı adı alanı gerekli.")]
        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Şifre alanı gerekli.")]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string Password { get; set; }

        [Display(Name = "Beni hatırla")]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        [Required(ErrorMessage = "Adı alanı gerekli.")]
        [Display(Name = "Adı")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Soyadı alanı gerekli.")]
        [Display(Name = "Soyadı")]
        public string SurName { get; set; }

        [Required(ErrorMessage = "Kullanıcı adı alanı gerekli.")]
        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "E-mail alanı gerekli.")]
        [EmailAddress]
        [Display(Name = "E-mail")]
        public string eMail { get; set; }

        [Required(ErrorMessage = "Şifre alanı gerekli.")]
        [StringLength(100, ErrorMessage = "{0} en az {2} karakter uzunluğunda olmalı.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Şifreyi dogrula")]
        [Compare("Password", ErrorMessage = "Şifre ve doğrulama şifresi eşleşmiyor.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Telefon alanı gerekli.")]
        [Phone]
        [Display(Name = "Phone")]
        public string Phone { get; set; }
    }

    public class ExternalLogin
    {
        public string Provider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderUserId { get; set; }
    }
}
