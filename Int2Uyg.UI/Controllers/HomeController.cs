using Microsoft.AspNetCore.Mvc;

namespace Int2Uyg.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // / → Login'e yönlendir
        public IActionResult Index()
        {
            return RedirectToAction("Login");
        }

        [Route("Dashboard")]
        public IActionResult Dashboard()
        {
            ViewBag.ApiBaseUrl = _configuration["ApiBaseUrl"];
            return View();
        }

        [Route("Categories")]
        public IActionResult Categories()
        {
            ViewBag.ApiBaseUrl = _configuration["ApiBaseUrl"];
            return View();
        }

        [Route("Surveys")]
        public IActionResult Surveys()
        {
            ViewBag.ApiBaseUrl = _configuration["ApiBaseUrl"];
            return View();
        }

        [Route("Login")]
        public IActionResult Login()
        {
            ViewBag.ApiBaseUrl = _configuration["ApiBaseUrl"];
            return View();
        }

        [Route("Profile")]
        public IActionResult Profile()
        {
            ViewBag.ApiBaseUrl = _configuration["ApiBaseUrl"];
            return View();
        }

        [Route("Register")]
        public IActionResult Register()
        {
            ViewBag.ApiBaseUrl = _configuration["ApiBaseUrl"];
            return View();
        }

        [Route("ForgotPassword")]
        public IActionResult ForgotPassword()
        {
            ViewBag.ApiBaseUrl = _configuration["ApiBaseUrl"];
            return View();
        }

        [Route("Home/Questions/{id}")]
        public IActionResult Questions(int id)
        {
            ViewBag.ApiBaseUrl = _configuration["ApiBaseUrl"];
            ViewBag.SurveyId = id;
            return View();
        }

        [Route("Home/TakeSurvey/{id}")]
        public IActionResult TakeSurvey(int id)
        {
            ViewBag.ApiBaseUrl = _configuration["ApiBaseUrl"];
            ViewBag.SurveyId = id;
            return View();
        }

        [Route("UserManagement")]
        public IActionResult UserManagement()
        {
            ViewBag.ApiBaseUrl = _configuration["ApiBaseUrl"];
            return View();
        }
    }
}