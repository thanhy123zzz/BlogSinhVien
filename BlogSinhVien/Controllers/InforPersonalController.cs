using BlogSinhVien.Models.EntitiesNew;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BlogSinhVien.Controllers
{
    [Authorize]
    public class InforPersonalController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly ILogger<InforPersonalController> _logger;
        public InforPersonalController(ILogger<InforPersonalController> logger, IWebHostEnvironment hostEnvironment)
        {
            _logger = logger;
            webHostEnvironment = hostEnvironment;
        }
        [HttpGet]
        [Route("/inforpersonal")]
        public IActionResult Details()
        {

            return View();
        }
        [HttpGet]
        [Route("/change-password")]
        public IActionResult ChanggePass()
        {
            return View();
        }

        [HttpPost]
        [Route("/confirm-change-password")]
        public IActionResult CofirmChangePass(string oldpass, string newpass, string confirmpass)
        {
            BlogSinhVienNewContext context = new BlogSinhVienNewContext();
            Users u = context.Users.Include(x => x.IdtkNavigation).Where(x => x.Id == int.Parse(User.Identity.Name)).FirstOrDefault();
            if (u.IdtkNavigation.MatKhau.Equals(oldpass))
            {
                u.IdtkNavigation.MatKhau = newpass;
                context.Accounts.Update(u.IdtkNavigation);
                context.SaveChanges();
                TempData["ThongBao"] = "Đổi mật khẩu thành công!";
            }
            else
            {
                TempData["ThongBao"] = "Đổi mật khẩu thất bại!";
            }
            return RedirectToAction("Details");
        }

        [HttpPost("/change-information")]
        public IActionResult ChangeInfor(Users user)
        {
            BlogSinhVienNewContext context = new BlogSinhVienNewContext();
            Users u = context.Users.Find(int.Parse(User.Identity.Name));
            u.Ten = user.Ten;
            u.Ho = user.Ho;
            u.Phone = user.Phone;
            u.Cccd = user.Cccd;
            u.EmailEdu = user.EmailEdu;
            u.EmailPrivate = user.EmailPrivate;
            u.GioiTinh = user.GioiTinh;
            u.DiaChi = user.DiaChi;
            context.Users.Update(u);
            context.SaveChanges();
            TempData["ThongBao"] = "Sửa thành công!";
            return RedirectToAction("Details");
        }
        [HttpPost("update-avt")]
        public bool updateAvt(IList<IFormFile> files)
        {
            var context = new BlogSinhVienNewContext();
            Users u = context.Users.Find(int.Parse(User.Identity.Name));
            if (files != null)
            {
                string avt = u.HinhAnh;

                u.HinhAnh = u.MaSv + "." + files[0].FileName.Split(".")[1];
                context.Users.Update(u);
                context.SaveChanges();
                if (!avt.Equals("avt.png"))
                {
                    string path = Path.Combine(webHostEnvironment.WebRootPath, "images", "avts", avt);
                    System.IO.File.Delete(path);
                }
                string pathFile = webHostEnvironment.WebRootPath + "\\images\\avts\\" + u.HinhAnh;
                var fileStream = new FileStream(pathFile, FileMode.Create);
                files[0].CopyTo(fileStream);
                fileStream.Close();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
