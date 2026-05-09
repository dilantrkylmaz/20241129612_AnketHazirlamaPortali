using Microsoft.AspNetCore.Mvc;

namespace Int2Uyg.UI.Controllers
{
    // ✅ Admin-only pages — role check is done in JavaScript
    // (Full server-side auth would require cookie auth, but this project uses JWT + localStorage)
    [Route("admin")]
    public class AdminController : Controller
    {
        private readonly IConfiguration _configuration;

        public AdminController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET /admin  → Admin dashboard
        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            ViewBag.ApiBaseUrl = _configuration["ApiBaseUrl"];
            return View();
        }

        // GET /admin/categories
        [Route("categories")]
        public IActionResult Categories()
        {
            ViewBag.ApiBaseUrl = _configuration["ApiBaseUrl"];
            return View();
        }

        // GET /admin/surveys
        [Route("surveys")]
        public IActionResult Surveys()
        {
            ViewBag.ApiBaseUrl = _configuration["ApiBaseUrl"];
            return View();
        }

        // GET /admin/questions/{id}
        [Route("questions/{id}")]
        public IActionResult Questions(int id)
        {
            ViewBag.ApiBaseUrl = _configuration["ApiBaseUrl"];
            ViewBag.SurveyId = id;
            return View();
        }

        // GET /admin/users
        [Route("users")]
        public IActionResult Users()
        {
            ViewBag.ApiBaseUrl = _configuration["ApiBaseUrl"];
            return View();
        }
        // GET /admin/profile
        [Route("profile")]
        public IActionResult Profile()
        {
            ViewBag.ApiBaseUrl = _configuration["ApiBaseUrl"];
            return View();
        }
        [Route("reports")]
        public IActionResult Reports()
        {
            ViewBag.ApiBaseUrl = _configuration["ApiBaseUrl"];
            return View();
        }
        [Route("userreports")]
        public IActionResult UserReports()
        {
            ViewBag.ApiBaseUrl = _configuration["ApiBaseUrl"];
            return View();
        }
    }
}