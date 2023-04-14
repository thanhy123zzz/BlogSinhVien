using BlogSinhVien.Models.EntitiesNew;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;

namespace BlogSinhVien.Controllers
{
    [Authorize]
    [Route("BDCaNhan")]
    public class BDCaNhan : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public BDCaNhan(IWebHostEnvironment hostEnvironment)
        {
            webHostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            ViewData["Title"] = "Quản lý bài viết cá nhân";
            return View();
        }
        [Route("deleteBD/{MaBD:int}")]
        public IActionResult delete(int MaBD)
        {
            BlogSinhVienNewContext context = new BlogSinhVienNewContext();
            var tran = context.Database.BeginTransaction();
            try
            {
                var bd = context.BaiDang
                    .Include(x => x.BinhLuan)
                    .Include(x => x.ChiTietBaiDang)
                    .Where(x => x.Id == MaBD).FirstOrDefault();
                if (bd.BinhLuan != null)
                {
                    foreach (BinhLuan bl in bd.BinhLuan)
                    {
                        context.BinhLuan.Remove(bl);
                    }
                }
                if (bd.ChiTietBaiDang != null)
                {
                    foreach (ChiTietBaiDang ct in bd.ChiTietBaiDang)
                    {
                        string path = Path.Combine(webHostEnvironment.WebRootPath, "BaiDang", ct.Path, ct.NameFile);
                        System.IO.File.Delete(path);
                        context.ChiTietBaiDang.Remove(ct);
                    }
                }
                context.BaiDang.Remove(bd);
                context.SaveChanges();
                tran.Commit();
            }
            catch (Exception e)
            {
                tran.Rollback();
                TempData["ThongBao"] = "Xoá thất bại!";
            }
            TempData["ThongBao"] = "Xoá thành công!";
            return RedirectToAction("Index");
        }

        [HttpPost("/load-more-bd-cn")]
        public IActionResult load_more_bd(int slbd)
        {
            BlogSinhVienNewContext context = new BlogSinhVienNewContext();
            Users nguoiDung = context.Users.Find(Int32.Parse(User.Identity.Name));
            ViewBag.ListBaiDang = context.BaiDang.Include(x => x.IduserNavigation)
            .Include(x => x.ChiTietBaiDang)
            .Include(x => x.BinhLuan)
            .Include(x => x.IduserNavigation)
            .Where(x => x.Iduser == nguoiDung.Id)
            .OrderByDescending(x => x.NgayDang)
            .Take(slbd + 5)
            .ToList();
            return PartialView("loadBaiDangCN");
        }
    }
}
