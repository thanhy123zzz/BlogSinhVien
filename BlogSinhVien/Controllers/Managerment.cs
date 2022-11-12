using BlogSinhVien.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
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
            List<BaiDang> listBD = context.BaiDang.OrderByDescending(x => x.NgayDang).ToList();
            ViewBag.Loai = false;
            ViewBag.ListBD = listBD;
            return View("QuanLyBaiViet");
        }

        [Route("xoa/{MaBD:int}")]
        public IActionResult XoaBD(int MaBD)
        {
            BlogSinhVienContext context = new BlogSinhVienContext();

            context.Database.ExecuteSqlRaw("delete ChiTietBaiDang where MaBaiDang = " + MaBD);

            BaiDang bd = context.BaiDang.Find(MaBD);

            List<BinhLuan> bls = context.BinhLuan.Where(x => x.MaBaiDang == MaBD).ToList();

            foreach (BinhLuan bl in bls)
            {
                context.Database.ExecuteSqlRaw("delete Vote where MaCmt = " + bl.MaCmt);
            }

            context.BinhLuan.RemoveRange(context.BinhLuan.Where(x => x.MaBaiDang == MaBD));
            context.SaveChanges();
            context.BaiDang.Remove(bd);
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

        [Route("remove-cmt/{MaCmt:int}")]
        public IActionResult RemoveCmt(int MaCmt)
        {
            BlogSinhVienContext context = new BlogSinhVienContext();
            
            context.Database.ExecuteSqlRaw("delete Vote where MaCmt = " + MaCmt);

            BinhLuan bl = context.BinhLuan.Find(MaCmt);
            context.BinhLuan.Remove(bl);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost("search-manager")]
        public IActionResult SearchManager(string key)
        {
            BlogSinhVienContext context = new BlogSinhVienContext();
             List<BaiDang> listBD = context.BaiDang
                       .FromSqlRaw("select bd.MaBaiDang,bd.MaSinhVien,bd.MaQl,bd.NgayDang,bd.TrangThai,bd.Content from BaiDang bd left join SinhVien sv on bd.MaSinhVien = sv.MaSv left join QuanLy ql on bd.MaQL = ql.MaQL where concat_ws(' ',sv.Ten,sv.Ho,bd.Content,ql.Ten,ql.Ho) LIKE '%" + key+"%';").ToList();
            ViewBag.Loai = false;
            ViewBag.ListBD = listBD;
            return PartialView("_List_Bd_Manager");
        }
        [HttpPost("insert-info-sv")]
        public IActionResult insert_infor_sv()
        {
            
            return View();
        }
    }
}
