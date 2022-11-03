using BlogSinhVien.Hubs;
using Microsoft.AspNetCore.Mvc;

namespace BlogSinhVien.Controllers
{
    public class ChatController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
