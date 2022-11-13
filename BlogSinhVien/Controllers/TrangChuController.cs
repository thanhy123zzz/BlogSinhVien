using BlogSinhVien.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;
using SqlParameter = Microsoft.Data.SqlClient.SqlParameter;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Text;

namespace BlogSinhVien.Controllers
{

    [Authorize(Roles = "QL,SV")]
    public class TrangChuController : Controller
    {
        private readonly ILogger<TrangChuController> _logger;
        public TrangChuController(ILogger<TrangChuController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        [Route("trang-chu")]
        [Route("")]
        public IActionResult Index()
        {
            BlogSinhVienContext context = new BlogSinhVienContext();
            List<BaiDang> listBD = context.BaiDang.Where(x=>x.TrangThai==true).OrderByDescending(x => x.NgayDang).Take(5).ToList();
            ViewBag.Loai = true;
            ViewBag.ListBD = listBD;
            return View("TrangChu");
        }
        
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost("post-new")]
        public IActionResult post_new(IFormFile[] fileImage, IFormFile[] fileDocs, IFormFile[] fileVideo, string content)
        {
            BlogSinhVienContext context = new BlogSinhVienContext();
            BaiDang baiDang = new BaiDang();

            baiDang.Content = content;
            if(User.IsInRole("SV"))
            baiDang.MaSinhVien = User.FindFirst("MaSV").Value;
            else baiDang.MaQl = User.FindFirst("MaSV").Value;
            baiDang.TrangThai = false;
            baiDang.NgayDang = DateTime.Now;

            context.BaiDang.Add(baiDang);
            context.SaveChanges();

            List<ChiTietBaiDang> chiTietBaiDangs = new List<ChiTietBaiDang>();
            int maBD = context.BaiDang.OrderByDescending(x => x.MaBaiDang).FirstOrDefault().MaBaiDang;
            insertFile(fileImage, maBD);
            insertFile(fileVideo, maBD);
            insertFile(fileDocs, maBD);
            return RedirectToAction("Index");
        }

        private void insertFile(IFormFile[] files, int MaBD)
        {
            _logger.LogInformation(files.Count().ToString());
            BlogSinhVienContext context = new BlogSinhVienContext();
            if (files.Count() == 0) return;
            else
            {
                foreach (var file in files)
                {
                    MemoryStream ms = new MemoryStream();
                    file.CopyTo(ms);
                    string sql = "insert into ChiTietBaiDang values({0},{1},{2},{3})";
                    context.Database.ExecuteSqlRaw(sql, MaBD, ms.ToArray(), file.FileName, file.ContentType);
                }
            }
        }
        [AllowAnonymous]
        [Route("/download")]
        public IActionResult Download(int MaBD, string tenFile, string loaiFile)
        {
            BlogSinhVienContext context = new BlogSinhVienContext();
            ChiTietBaiDang ct = context.ChiTietBaiDang.Where(x => x.MaBaiDang == MaBD && x.NameFile == tenFile && x.Type == loaiFile).FirstOrDefault();
            return File(ct.Files,ct.Type.Trim(),ct.NameFile.Trim());
        }

        [HttpPost("load-more-cmt")]
        public IActionResult load_more_cmt(int MaBD, int sl,bool loai)
        {
            ViewBag.MaBD = MaBD;
            ViewBag.sl = sl+5;
            ViewBag.Loai = loai;
            return PartialView("_list_cmt");
        }
        [HttpPost("load-more-bd")]
        public IActionResult load_more_bd(int slbd)
        {
            BlogSinhVienContext context = new BlogSinhVienContext();
            _logger.LogInformation(slbd.ToString());
            List<BaiDang> listBD = context.BaiDang.Where(x => x.TrangThai == true).OrderByDescending(x => x.TrangThai).Take(slbd+5).ToList();
            ViewBag.ListBD = listBD;
            ViewBag.SLBD = slbd + 5;
            return PartialView("_baiDang");
        }

        public IActionResult Edit(String? id)
        {
            BlogSinhVienContext context = new BlogSinhVienContext();
            Roles role = context.Roles.Find(id);
            if (role!=null)
                return View(role);
            else
                TempData["message"] = "Không tồn tại mã role";
                return RedirectToAction("Index");
        }
        public IActionResult Details(String? id)
        {
            BlogSinhVienContext context = new BlogSinhVienContext();
            Roles role = context.Roles.Find(id);
            return View(role);
        }
        [HttpPost]
        public IActionResult insert(Roles role)
        {
            BlogSinhVienContext context = new BlogSinhVienContext();
            if (context.Roles.Find(role.MaRole) == null)
            {
                foreach (var file in Request.Form.Files)
                {
                    MemoryStream ms = new MemoryStream();
                    file.CopyTo(ms);
                    role.HinhAnh = ms.ToArray();
                }
                context.Roles.Add(role);
                context.SaveChanges();
                TempData["messageS"] = "Thêm thành công!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "Mã role đã tồn tại!";
                return RedirectToAction("Create");
            }
        }
        [HttpPost]
        public IActionResult SaveEdit(Roles role)
        {
            BlogSinhVienContext context = new BlogSinhVienContext();
            foreach (var file in Request.Form.Files)
            {
                MemoryStream ms = new MemoryStream();
                file.CopyTo(ms);
                role.HinhAnh = ms.ToArray();
            }
            context.Roles.Update(role);
            context.SaveChanges();
            TempData["messageS"] = "Sửa thành công!";
            return RedirectToAction("Index");
        }

        public IActionResult Delete(String id)
        {
            BlogSinhVienContext context = new BlogSinhVienContext();
            
            if (context.Roles.Find(id) == null)
            {
                TempData["message"] = "Lỗi, không có Mã role cần xoá!";
                return RedirectToAction("Index");
            }
            else
            {
                context.Roles.Remove(context.Roles.Find(id));
                context.SaveChanges();
                TempData["messageS"] = "Xoá thành công!";
                return RedirectToAction("Index");
            }
        }
        [Route("/search")]
        public IActionResult search(string key)
        {
            BlogSinhVienContext context = new BlogSinhVienContext();
            var list = context.Roles.FromSqlRaw("timKiem @marole = " + key)
                    .ToList();
            return View("Role", list);
        }
    }
    public static class SessionExtensions
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);

            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
