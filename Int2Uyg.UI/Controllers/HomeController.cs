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
            return View();
        }

        [Route("Categories")]
        public IActionResult Categories()
        {
            ViewBag.ApiBaseUrl = _configuration["ApiBaseUrl"];
            return View();
        }

        [Route("Surveys/{id?}")]
        public IActionResult Surveys(int? id)
        {
            ViewBag.ApiBaseUrl = _configuration["ApiBaseUrl"];
            ViewBag.CatId = id ?? 0;
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
            ViewBag.SurveyId = id;
            return View();
        }

        public IActionResult TakeSurvey(int id)
        {
            return View();
        }

        // ✅ NEW: Kullanıcı Yönetimi sayfası (sadece Admin erişebilir, kontrol JS tarafında yapılıyor)
        [Route("UserManagement")]
        public IActionResult UserManagement()
        {
            ViewBag.ApiBaseUrl = _configuration["ApiBaseUrl"];
            return View();
        }
    }
}