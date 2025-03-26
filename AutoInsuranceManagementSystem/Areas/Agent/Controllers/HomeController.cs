using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoInsuranceManagementSystem.Areas.Agent.Controllers
{
    [Area("Agent")]
    [Authorize("AGENT")]
    [Route("Agent/[controller]/[action]")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
