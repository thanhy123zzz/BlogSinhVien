using BlogSinhVien.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BlogSinhVien.Controllers
{
    [Authorize(Roles = "QL,SV")]
    [Route("BDCaNhan")]
    public class BDCaNhan : Controller
	{
        public IActionResult Index()
		{
            BlogSinhVienContext context = new BlogSinhVienContext();
            ViewBag.ListMyPost = context.BaiDang.Where(x => x.MaSinhVien == User.FindFirst("MaSV").Value || x.MaQl == User.FindFirst("MaSV").Value);
			return View();
		}
        [Route("deleteBD/{MaBD:int}")]
        public IActionResult delete(int MaBD)
        {
            BlogSinhVienContext context = new BlogSinhVienContext();
            context.Database.ExecuteSqlRaw("delete ChiTietBaiDang where MaBaiDang = "+MaBD);

            BaiDang bd = context.BaiDang.Find(MaBD);

            List<BinhLuan> bls = context.BinhLuan.Where(x => x.MaBaiDang == MaBD).ToList();

            foreach(BinhLuan bl in bls)
            {
                context.Database.ExecuteSqlRaw("delete Vote where MaCmt = " + bl.MaCmt);
            }

            context.BinhLuan.RemoveRange(context.BinhLuan.Where(x => x.MaBaiDang == MaBD));
            context.SaveChanges();
            context.BaiDang.Remove(bd);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
	}
}
