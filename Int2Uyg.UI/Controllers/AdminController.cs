using Microsoft.AspNetCore.Mvc;

namespace Int2Uyg.UI.Controllers
{
    [Route("admin")]
    public class AdminController : Controller
    {
        private readonly IConfiguration _configuration;

        public AdminController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            ViewBag.ApiBaseUrl = _configuration["ApiBaseUrl"];
            return View();
        }

        [Route("categories")]
        public IActionResult Categories()
        {
            ViewBag.ApiBaseUrl = _configuration["ApiBaseUrl"];
            return View();
        }

        [Route("surveys")]
        public IActionResult Surveys()
        {
            ViewBag.ApiBaseUrl = _configuration["ApiBaseUrl"];
            return View();
        }

        [Route("questions/{id}")]
        public IActionResult Questions(int id)
        {
            ViewBag.ApiBaseUrl = _configuration["ApiBaseUrl"];
            ViewBag.SurveyId = id;
            return View();
        }

        [Route("users")]
        public IActionResult Users()
        {
            ViewBag.ApiBaseUrl = _configuration["ApiBaseUrl"];
            return View();
        }

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