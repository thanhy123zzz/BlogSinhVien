using BlogSinhVien.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace BlogSinhVien.Controllers
{
    public class InforPersonalController : Controller
    {
        private readonly ILogger<InforPersonalController> _logger;
        public InforPersonalController(ILogger<InforPersonalController> logger)
        {
            _logger = logger;
        }
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
            QuanLy quanLy = _context.QuanLy.FirstOrDefault(x => x.MaQl == MaSV);
            if (sv == null && quanLy == null)
            {
                return NotFound();
            }
            if (sv != null)
            {
                ViewBag.SinhVien = sv;
            }
            else if (quanLy != null)
            {
                ViewBag.QuanLy = quanLy;
            }
            return View();
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
            QuanLy ql = _context.QuanLy.FirstOrDefault(x => x.MaQl == MaSV);
            if(sv != null){
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
            }else{
            Accounts ac1 = _context.Accounts.FirstOrDefault(x => x.TaiKhoan == ql.TaiKhoan);
                if (String.Equals(ac1.MatKhau.Trim(), oldpass))
            {
                // oldpass.CompareTo(ac.MatKhau) == 0 
                //String.Equals(ac.MatKhau, oldpass)
                // ac.TaiKhoan.StartsWith(oldpass) 
                ac1.MatKhau = newpass;
                _context.Accounts.Update(ac1);
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
            
        }



        [HttpGet]
        [Authorize(Roles = "QL,SV")]
        [Route("/change-information")]
        public IActionResult ChangeInfor()
        {
            BlogSinhVienContext _context = new BlogSinhVienContext();
            string MaSV = User.FindFirst("MaSV").Value;
            SinhVien sv = _context.SinhVien.FirstOrDefault(x => x.MaSv == MaSV);
            QuanLy quanLy = _context.QuanLy.FirstOrDefault(x => x.MaQl == MaSV);
            if (sv == null && quanLy == null)
            {
                return NotFound();
            }
            if (sv != null)
            {
                ViewBag.SinhVien = sv;
            }
            else if (quanLy != null)
            {
                ViewBag.QuanLy = quanLy;
            }
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "QL,SV")]
        [Route("/confirm-change-information")]
        public IActionResult SaveChangeInfor(IFormCollection form)
        {
            var _context = new BlogSinhVienContext();
            string MaSV = User.FindFirst("MaSV").Value;
            var data = _context.SinhVien.FirstOrDefault(x => x.MaSv == MaSV);
            var data2 = _context.QuanLy.FirstOrDefault(x => x.MaQl == MaSV);
            if (data == null && data2 == null)
            {
                return View();
            }
            else if(data != null)
            {
                data.Ho = form["Ho"];
                data.Ten = form["ten"];
                data.DiaChi = form["DiaChi"];
                data.EmailEdu = form["EmailEdu"];
                data.EmailPrivate = form["EmailPrivate"];
                data.Cccd = form["Cccd"];
                data.GioiTinh = Boolean.Parse(form["GioiTinh"]);
                data.Phone = form["Phone"];
                _context.SinhVien.Update(data);
                _context.SaveChanges();
                TempData["messUpdateSuccess"] = $"Cập nhật thông tin cá nhân thành công!";
                return RedirectToAction("Details", "InforPersonal");
            }else if(data2 != null){
                data2.Ho = form["Ho"];
                data2.Ten = form["ten"];
                data2.DiaChi = form["DiaChi"];
                data2.EmailEdu = form["EmailEdu"];
                data2.EmailPrivate = form["EmailPrivate"];
                data2.Cccd = form["Cccd"];
                data2.GioiTinh = Boolean.Parse(form["GioiTinh"]);
                data2.Phone = form["Phone"];
                _context.QuanLy.Update(data2);
                _context.SaveChanges();
                TempData["messUpdateSuccess"] = $"Cập nhật thông tin cá nhân thành công!";
                return RedirectToAction("Details", "InforPersonal");
            }
            return View();
        }
        [Route("update-avt")]
        public bool updateAvt(IList<IFormFile> files)
        {
            var _context = new BlogSinhVienContext();
            if (files != null)
            {
                MemoryStream ms = new MemoryStream();
                files[0].CopyTo(ms);
                string sql = "update SinhVien set HinhAnh = {0} where MaSV = {1}";
                _context.Database.ExecuteSqlRaw(sql, ms.ToArray(), User.FindFirst("MaSV").Value);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
