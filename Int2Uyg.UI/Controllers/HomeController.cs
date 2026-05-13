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

        public IActionResult Index()
        {
            return RedirectToAction("Login");
        }

        [Route("Login")]
        public IActionResult Login()
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
    }
}