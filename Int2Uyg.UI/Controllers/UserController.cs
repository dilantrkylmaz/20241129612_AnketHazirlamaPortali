using Microsoft.AspNetCore.Mvc;

namespace Int2Uyg.UI.Controllers
{
    [Route("user")]
    public class UserController : Controller
    {
        private readonly IConfiguration _configuration;

        public UserController(IConfiguration configuration)
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

        [Route("browse")]
        public IActionResult Browse()
        {
            ViewBag.ApiBaseUrl = _configuration["ApiBaseUrl"];
            return View();
        }

        [Route("survey/{id}")]
        public IActionResult Survey(int id)
        {
            ViewBag.ApiBaseUrl = _configuration["ApiBaseUrl"];
            ViewBag.SurveyId = id;
            return View();
        }

        [Route("profile")]
        public IActionResult Profile()
        {
            ViewBag.ApiBaseUrl = _configuration["ApiBaseUrl"];
            return View();
        }
    }
}