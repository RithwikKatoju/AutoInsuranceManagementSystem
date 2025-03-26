using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AutoInsuranceManagementSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize("ADMIN")]
    [Route("Admin/[controller]/[action]")]
    public class AdminBaseController : Controller
    {

        public class ClubBaseController : Controller
        {
            public override void OnActionExecuting(ActionExecutingContext context)
            {
                ViewData["Area"] = "Admin";
                ViewData["Layout"] = "~/Areas/Admin/Views/Shared/_AdminLayout.cshtml";
                base.OnActionExecuting(context);
            }
        }
    }
}
