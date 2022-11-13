using BlogSinhVien.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BlogSinhVien.Controllers
{
    public class MessagesController : Controller
    {
        [Route("messages")]
        [Authorize(Roles = "SV")]
        public IActionResult Index()
        {
            string MaSV = User.FindFirst("MaSV").Value;
            BlogSinhVienContext context = new BlogSinhVienContext();
            List<Conversation> conversations = context.Conversation.Where(x => x.MaSinhVien1 == MaSV || x.MaSinhVien2 == MaSV).ToList();
            int MaC = conversations.FirstOrDefault().MaC;
            if (context.Conversation.Find(MaC).MaSinhVien1 == User.FindFirst("MaSV").Value)
            {
                ViewBag.MaSV = context.Conversation.Find(MaC).MaSinhVien2;
            }
            else
            {
                ViewBag.MaSV = context.Conversation.Find(MaC).MaSinhVien1;
            }
            ViewBag.MaC = MaC;
            ViewBag.Conversations = conversations;
            ViewBag.messages = context.Message.Where(x => x.MaC == MaC).OrderBy(x => x.SendTime).ToList();
            return View();
        }

        [HttpPost("load-messagebox")]
        public IActionResult Load_MessageBox(int MaC)
        {
            BlogSinhVienContext context = new BlogSinhVienContext();
            var messages = context.Message.Where(x => x.MaC == MaC).OrderBy(x => x.SendTime).ToList();
            ViewBag.messages = messages;
            ViewBag.MaC = MaC;
            return PartialView("_MessageBox");
        }   
        [HttpPost("load-nameSV")]
        public IActionResult Load_nameSV(int MaC)
        {
            BlogSinhVienContext context = new BlogSinhVienContext();
            if (context.Conversation.Find(MaC).MaSinhVien1 == User.FindFirst("MaSV").Value)
            {
                ViewBag.MaSV = context.Conversation.Find(MaC).MaSinhVien2;
                return PartialView("_HeaderMessage");
            }
            else
            {
                ViewBag.MaSV = context.Conversation.Find(MaC).MaSinhVien1;
                return PartialView("_HeaderMessage");
            }
        }
    }
}
