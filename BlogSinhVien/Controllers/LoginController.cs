using Microsoft.AspNetCore.Mvc;

namespace BlogSinhVien.Controllers
{
    [Route("login")]
    public class LoginController : Controller
    {
        [Route("")]
        public IActionResult Index()
        {
            return View("login");
        }
    }
}
