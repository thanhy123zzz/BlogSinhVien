using BlogSinhVien.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace BlogSinhVien.Controllers
{
    public class MessagesController : Controller
    {
        [Route("messages")]
        public IActionResult Index()
        {
            BlogSinhVienContext context = new BlogSinhVienContext();
            ViewBag.Conversations = context.Conversation.ToList();
            return View();
        }

        [HttpPost("load-messagebox")]
        public IActionResult Load_MessageBox(int MaC)
        {
            BlogSinhVienContext context = new BlogSinhVienContext();
            var messages = context.Message.Where(x => x.MaC == MaC).ToList();
            ViewBag.messages = messages;
            ViewBag.MaC = MaC;
            return PartialView("_MessageBox");
        }
    }
}
