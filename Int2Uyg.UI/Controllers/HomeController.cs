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
            // ✅ FIX: Check if user is already logged in via JWT in localStorage
            // If authenticated, redirect based on role; else show landing page
            return View();
        }

        [Route("login")]
        public IActionResult Login()
        {
            ViewBag.ApiBaseUrl = _configuration["ApiBaseUrl"];
            return View();
        }

        [Route("register")]
        public IActionResult Register()
        {
            ViewBag.ApiBaseUrl = _configuration["ApiBaseUrl"];
            return View();
        }

        [Route("forgot-password")]
        public IActionResult ForgotPassword()
        {
            ViewBag.ApiBaseUrl = _configuration["ApiBaseUrl"];
            return View();
        }
    }
}   