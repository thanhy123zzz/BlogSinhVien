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
            return View("TrangChu");
        }
        public IActionResult Create()
        {
            return View();
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
