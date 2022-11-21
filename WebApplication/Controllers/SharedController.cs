using databatdongsan.library;
using System.Web.Mvc;
using WebApplication.Helpers.Extensions;
using WebApplication.Helpers.Sercurity;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class SharedController : Controller
    {
        //private readonly UserSessionVM _currentUser = SessionHelper.Get<UserSessionVM>(ConstantHelper.UserSession);

        [ChildActionOnly]
        public PartialViewResult _PartialHeader()
        {
            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            HeaderVM model = new HeaderVM
            {
                //UserSession = _currentUser,
                ActionsList = Actions.Static_GetByUser(myPrincipal.UserName, myPrincipal.UserId)
            };

            return PartialView(model);
        }
    }
}