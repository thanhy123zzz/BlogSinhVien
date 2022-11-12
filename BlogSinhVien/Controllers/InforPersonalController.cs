using BlogSinhVien.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Data;
using System.Linq;

namespace BlogSinhVien.Controllers
{
    public class InforPersonalController : Controller
    {
        [HttpGet]
        [Route("/inforpersonal")]
        [Authorize(Roles = "QL,SV")]
        public IActionResult Details()
        {
            /*var student = await _context.Students
                .FirstOrDefaultAsync(m => m.Id == id); */
            BlogSinhVienContext _context = new BlogSinhVienContext();
            //string tk = "xuandieu123";
            string MaSV = User.FindFirst("MaSV").Value;
            // string MaQL = User.FindFirst("MaQL").Value;
            SinhVien sv = _context.SinhVien.FirstOrDefault(x => x.MaSv == MaSV);
            // QuanLy quanLy = _context.QuanLy.FirstOrDefault(x => x.MaQl == MaQL);
            if (sv == null)
            {
                return NotFound();
            }
            // if (sv != null)
            // {
            //     return View(sv);
            // }
            // else if (quanLy != null)
            // {
            //     return View(quanLy);

            // }
            return View(sv);
        }
        [HttpGet]
        [Route("/change-password")]
        [Authorize(Roles = "QL,SV")]
        public IActionResult ChanggePass()
        {
            return View();
        }

        [HttpPost]
        [Route("/confirm-change-password")]
        [Authorize(Roles = "QL,SV")]
        public IActionResult CofirmChangePass(IFormCollection form)
        {
            string oldpass = form["oldpass"];
            string newpass = form["newpass"];
            string confirmpass = form["confirmpass"];

            BlogSinhVienContext _context = new BlogSinhVienContext();
            string MaSV = User.FindFirst("MaSV").Value;
            SinhVien sv = _context.SinhVien.FirstOrDefault(x => x.MaSv == MaSV);
            Accounts ac = _context.Accounts.FirstOrDefault(x => x.TaiKhoan == sv.TaiKhoan);
            if (String.Equals(ac.MatKhau.Trim(), oldpass))
            {
                // oldpass.CompareTo(ac.MatKhau) == 0 
                //String.Equals(ac.MatKhau, oldpass)
                // ac.TaiKhoan.StartsWith(oldpass) 
                ac.MatKhau = newpass;
                _context.Accounts.Update(ac);
                _context.SaveChanges();
                TempData["messUpdateSuccess"] = $"Đổi mật khẩu thành công!";
                return RedirectToAction("Details");
            }
            else
            {
                TempData["messPass"] = $"Mật khẩu cũ không chính xác!";
                // TempData["messPassx"] = "Mật khẩu cũ không chính xác!";
                return RedirectToAction("ChanggePass");
            }
        }



        [HttpGet]
        [Authorize(Roles = "QL,SV")]
        [Route("/change-information")]
        public IActionResult ChangeInfor()
        {
            BlogSinhVienContext _context = new BlogSinhVienContext();
            //string tk = "xuandieu123";
            string MaSV = User.FindFirst("MaSV").Value;
            SinhVien sv = _context.SinhVien.FirstOrDefault(x => x.MaSv == MaSV);
            if (sv == null)
            {
                return NotFound();
            }
            return View(sv);
        }
        [HttpPost]
        [Authorize(Roles = "QL,SV")]
        [Route("/confirm-change-information")]
        public IActionResult SaveChangeInfor(SinhVien sv)
        {
            var _context = new BlogSinhVienContext();
            string MaSV = User.FindFirst("MaSV").Value;
            var data = _context.SinhVien.FirstOrDefault(x => x.MaSv == sv.MaSv);
            if (data == null)
            {
                return View();
            }
            else
            {
                data.Ho = sv.Ho.Trim();
                data.Ten = sv.Ten.Trim();
                data.DiaChi = sv.DiaChi.Trim();
                data.EmailEdu = sv.EmailEdu.Trim();
                data.EmailPrivate = sv.EmailPrivate.Trim();
                data.Cccd = sv.Cccd.Trim();
                data.GioiTinh = sv.GioiTinh;
                data.Phone = sv.Phone.Trim();
                _context.SinhVien.Update(data);
                _context.SaveChanges();
                TempData["messUpdateSuccess"] = $"Cập nhật thông tin cá nhân thành công!";
                return RedirectToAction("Details", "InforPersonal");
            }
        }
    }
}
