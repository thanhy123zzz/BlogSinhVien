using BlogSinhVien.Models.EntitiesNew;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Linq;

namespace BlogSinhVien.Controllers
{
    public class MessagesController : Controller
    {
        [Route("messages")]
        [Authorize]
        public IActionResult Index()
        {
            ViewData["Title"] = "Trò chuyện";
            BlogSinhVienNewContext context = new BlogSinhVienNewContext();
            int? idU = int.Parse(User.Identity.Name);
            if (TempData["IdUser"] != null)
            {
                int? Id = int.Parse(TempData["IdUser"].ToString());
                Conversation c = context.Conversation
                    .Include(x => x.Iduser2Navigation)
                    .Include(x => x.Iduser1Navigation)
                    .Where(x => (x.Iduser1 == Id && x.Iduser2 == idU) || (x.Iduser2 == Id && x.Iduser1 == idU)).FirstOrDefault();
                if (c == null)
                {
                    c = new Conversation();
                    c.Iduser1 = Id;
                    c.Iduser2 = idU;
                    c.NgayTao = DateTime.Now;
                    context.Conversation.Add(c);
                    context.SaveChanges();
                    c.Iduser1Navigation = context.Users.Find(Id);
                    c.Iduser2Navigation = context.Users.Find(idU);
                }
                var messages = context.Message
                .Include(x => x.IduserSendNavigation)
                .Where(x => x.Idc == c.Id)
                .OrderByDescending(x => x.SendTime)
                .Take(25)
                .ToList();
                ViewBag.messages = messages.OrderBy(x => x.SendTime);
                return View(c);
            }
            return View(null);
        }

        [HttpPost("/load-messagebox")]
        public IActionResult Load_MessageBox(int MaC)
        {
            BlogSinhVienNewContext context = new BlogSinhVienNewContext();
            var messages = context.Message
                .Include(x => x.IduserSendNavigation)
                .Where(x => x.Idc == MaC)
                .OrderByDescending(x => x.SendTime)
                .Take(25)
                .ToList();
            ViewBag.MaC = MaC;
            foreach (Message m in messages)
            {
                if (m.TrangThai == false)
                {
                    m.TrangThai = true;
                }
            }
            Conversation c = context.Conversation.Find(MaC);
            c.TrangThai = true;
            context.Conversation.Update(c);

            context.Message.UpdateRange(messages);
            context.SaveChanges();
            ViewBag.messages = messages.OrderBy(x => x.SendTime);
            return PartialView("loadMesBox");
        }
        [HttpPost("load-nameSV")]
        public IActionResult Load_nameSV(int MaC)
        {
            BlogSinhVienNewContext context = new BlogSinhVienNewContext();
            /*if (context.Conversation.Find(MaC).MaSinhVien1 == User.FindFirst("MaSV").Value)
            {
                ViewBag.MaSV = context.Conversation.Find(MaC).MaSinhVien2;
                return PartialView("_HeaderMessage");
            }
            else
            {
                ViewBag.MaSV = context.Conversation.Find(MaC).MaSinhVien1;
                return PartialView("_HeaderMessage");
            }*/
            return PartialView("_HeaderMessage");
        }

        [HttpPost("/load-more-message")]
        public string loadMoreMessage(int sl, int MaC)
        {
            BlogSinhVienNewContext context = new BlogSinhVienNewContext();
            var M = context.Message
                .Include(x => x.IduserSendNavigation)
                .Where(x => x.Idc == MaC)
                .OrderByDescending(x => x.SendTime)
                .Take(sl)
                .ToList();
            Message mLast = M.OrderBy(x => x.SendTime).FirstOrDefault();
            var Listmessages = context.Message
                .Include(x => x.IduserSendNavigation)
                .Where(x => x.Idc == MaC && x.SendTime <= mLast.SendTime)
                .OrderByDescending(x => x.SendTime)
                .Take(sl + 25)
                .ToList();
            string html = "";
            int idUser = Int32.Parse(User.Identity.Name);
            if (mLast.Id != Listmessages.LastOrDefault().Id)
            {

                foreach (Message m in Listmessages.Take(25).OrderBy(x => x.SendTime).SkipLast(1))
                {
                    if (m.IduserSend == idUser)
                    {
                        html += "<li class='me'><figure></figure><p>" + m.Content + "</p></li>";
                    }
                    else
                    {
                        html += "<li class='you'><figure><img src='/images/avts/" + m.IduserSendNavigation.HinhAnh + "' alt='' width='35px' style='max-height: 35px; max-width:35px;object-fit: cover;'></figure><p>" + m.Content + "</p></li>";
                    }
                }
            }
            return html;
        }
    }
}
