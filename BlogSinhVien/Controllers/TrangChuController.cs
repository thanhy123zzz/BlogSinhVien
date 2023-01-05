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
using SelectPdf;
using static System.Net.WebRequestMethods;
using System.Security.Policy;
using System.Drawing.Printing;
using BlogSinhVien.Models;

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
            List<BaiDang> listBD = context.BaiDang.Where(x => x.TrangThai == true).OrderByDescending(x => x.NgayDang).Take(slbd+5).ToList();
            ViewBag.ListBD = listBD;
            ViewBag.SLBD = slbd + 5;
            return PartialView("_baiDang");
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
