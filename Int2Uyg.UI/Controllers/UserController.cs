using Microsoft.AspNetCore.Mvc;

namespace Int2Uyg.UI.Controllers
{
    // ✅ User-facing pages - accessible to all logged-in users
    [Route("user")]
    public class UserController : Controller
    {
        private readonly IConfiguration _configuration;

        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET /user  → User dashboard / home
        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            ViewBag.ApiBaseUrl = _configuration["ApiBaseUrl"];
            return View();
        }

        // GET /user/browse  → Browse all surveys by category
        [Route("browse")]
        public IActionResult Browse()
        {
            ViewBag.ApiBaseUrl = _configuration["ApiBaseUrl"];
            return View();
        }

        // GET /user/survey/{id}  → Take a specific survey
        [Route("survey/{id}")]
        public IActionResult Survey(int id)
        {
            ViewBag.ApiBaseUrl = _configuration["ApiBaseUrl"];
            ViewBag.SurveyId = id;
            return View();
        }

        // GET /user/profile  → User profile page
        [Route("profile")]
        public IActionResult Profile()
        {
            ViewBag.ApiBaseUrl = _configuration["ApiBaseUrl"];
            return View();
        }
    }
}