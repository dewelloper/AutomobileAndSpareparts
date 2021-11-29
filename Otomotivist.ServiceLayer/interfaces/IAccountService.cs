using Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otomotivist.Service.interfaces
{
    public interface IAccountService
    {
        UserProfile GetUserProfileById(int userid);
        UserProfile GetUserProfileByName(string username);
        UserProfile GetUserProfileByEmail(string email);
        bool GetWebMembershipUserByIdAndToken(int id, string token);
        UserProfile GetUserProfileByNameOrEmail(string email, string Name);
        bool InsertUserAddress(UserAdresses adr);
        bool UserExists(string userName);

        UserProfile GetUserProfileByNameAndPwd(string userName, string password);
        List<string> GetUserRoles(int userId);
        List<Roles> GetAllRoles(int userId);

        bool AddUserProfileWithUserRole(UserProfile up);
        bool CretateTokenToUser(int userId, string token);
        bool SetUserByToken(string token, string password);
    }
}
