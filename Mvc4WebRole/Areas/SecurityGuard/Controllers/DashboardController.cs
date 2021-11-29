using System.Web.Mvc;
using System.Web.Security;
using SecurityGuard.Services;
using SecurityGuard.Interfaces;
using SecurityGuard.ViewModels;
using Mvc4WebRole.Controllers;

namespace Mvc4WebRole.Areas.SecurityGuard.Controllers
{
    [Authorize(Roles = "SecurityGuard")]
    public partial class DashboardController : BaseController
    {
        private IMembershipService membershipService;
        private IRoleService roleService;

        public DashboardController()
        {
            roleService = new RoleService(Roles.Provider);
            membershipService = new MembershipService(Membership.Provider);
        }


        public virtual ActionResult Index()
        {
            var viewModel = new DashboardViewModel();
            int totalRecords;

            membershipService.GetAllUsers(0, 20, out totalRecords);
            viewModel.TotalUserCount = totalRecords.ToString();
            viewModel.TotalUsersOnlineCount = membershipService.GetNumberOfUsersOnline().ToString();
            viewModel.TotalRolesCount = roleService.GetAllRoles().Length.ToString();

            return View(viewModel);
        }
    }
}
