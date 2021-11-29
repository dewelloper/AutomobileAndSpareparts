using Dal;
using Otomotivist.Domain.Repository;
using Otomotivist.Domain.UnitOfWork;
using Otomotivist.Service.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Otomotivist.Service.services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _uow;
        private readonly IGRepository<UserProfile> _userProfile;
        private readonly IGRepository<webpages_Membership> _webpages_Membership;
        private readonly IGRepository<UserAdresses> _userAdresses;
        private readonly IGRepository<UserRole> _userRole;
        private readonly IGRepository<Roles> _roles;
        private readonly IGRepository<Tokens> _tokens;

        public AccountService(IUnitOfWork uow)
        {
            _uow = uow;
            _userProfile = _uow.GetRepository<UserProfile>();
            _webpages_Membership = _uow.GetRepository<webpages_Membership>();
            _userAdresses = _uow.GetRepository<UserAdresses>();
            _userRole = _uow.GetRepository<UserRole>();
            _roles = _uow.GetRepository<Roles>();
            _tokens = _uow.GetRepository<Tokens>();
        }

        public UserProfile GetUserProfileById(int userid)
        {
            return _userProfile.Where(k => k.UserId == userid).FirstOrDefault();
        }

        public UserProfile GetUserProfileByName(string username)
        {
            return _userProfile.Where(k => k.UserName == username || k.eMail == username).FirstOrDefault();
        }

        public UserProfile GetUserProfileByEmail(string email)
        {
            return _userProfile.Where(k => k.eMail == email).FirstOrDefault();
        }

        public UserProfile GetUserProfileByNameOrEmail(string email, string Name)
        {
            return _userProfile.Where(k => k.eMail == email || k.Name == Name).FirstOrDefault();
        }

        public bool GetWebMembershipUserByIdAndToken(int id, string token)
        {
            return _webpages_Membership.Where(k => k.UserId == id && k.PasswordVerificationToken == token).Any();
        }

        public bool InsertUserAddress(UserAdresses adr)
        {
            _userAdresses.Insert(adr);
            _uow.SavaChange();
            return true;
        }

        public bool UserExists(string userName)
        {
            return true;// WebMatrix.WebData.WebSecurity.UserExists(userName);
        }

        public UserProfile GetUserProfileByNameAndPwd(string userName, string password)
        {
            UserProfile up = _userProfile.Where(k => k.UserName == userName && k.Password == password).FirstOrDefault();
            if(up == null)
                up = _userProfile.Where(k => k.eMail == userName && k.Password == password).FirstOrDefault();
            return up;
        }

        public List<string> GetUserRoles(int userId)
        {
            var urs = _userRole.Where(k => k.UserId == userId).Select(k => k.RoleId).ToList();
            return _roles.Where(k => urs.Contains(k.RoleId)).Select(m => m.RoleName).ToList();
        }

        public List<Roles> GetAllRoles(int userId)
        {
            return _roles.Where(k => 1 == 1).ToList();
        }

        public bool AddUserProfileWithUserRole(UserProfile up)
        {
            _userProfile.Insert(up);
            UserProfile upro = _userProfile.Where(k => k.eMail == up.eMail).FirstOrDefault();
            UserRole role = new UserRole()
            {
                UserId = upro.UserId,
                RoleId = 2
            };
            _userRole.Insert(role);
            return true;
        }

        public bool CretateTokenToUser(int userId, string token)
        {
            _tokens.Insert(new Tokens()
            {
                Token = token,
                UserId = userId,
                Date = DateTime.Now
            });
            return true;
        }

        public bool SetUserByToken(string token, string password)
        {
            int? uid = _tokens.Where(k => k.Token == token).FirstOrDefault().UserId;
            UserProfile up = _userProfile.Where(k => k.UserId == uid).FirstOrDefault();
            up.Password = password;
            _userProfile.Update(up);
            return true;
        }


    }
}
