using BlogSinhVien.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BlogSinhVien.Controllers
{
    [Authorize(Roles = "QL")]
    [Route("managerment")]
	public class Managerment : Controller
	{
		[Route("")]
		public IActionResult Index()
		{
            BlogSinhVienContext context = new BlogSinhVienContext();

            List<BaiDang> listBD = context.BaiDang.Where(x => x.TrangThai == false).OrderByDescending(x => x.NgayDang).Take(5).ToList();

            ViewBag.ListBD = listBD;
            return View("QuanLyBaiViet");
        }

        [Route("xoa/{MaBD:int}")]
        public IActionResult XoaBD(int MaBD)
        {
            BlogSinhVienContext context = new BlogSinhVienContext();
            context.Database.ExecuteSqlRaw("delete ChiTietBaiDang where MaBaiDang = " + MaBD);
            BaiDang bd = context.BaiDang.Find(MaBD);
            context.Remove(bd);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        [Route("duyet/{MaBD:int}")]
        public IActionResult Duyet(int MaBD)
        {
            BlogSinhVienContext context = new BlogSinhVienContext();
            BaiDang bd = context.BaiDang.Find(MaBD);

            bd.TrangThai = true;

            context.Update(bd);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
