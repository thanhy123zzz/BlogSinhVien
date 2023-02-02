using BlogSinhVien.Models.EntitiesNew;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BlogSinhVien.Controllers
{
    [Authorize]
    public class SearchController : Controller
    {
        // private readonly ILogger<SearchController> _logger;

        // public SearchController(ILogger<SearchController> logger)
        // {
        //     _logger = logger;
        // }

        [HttpPost]
        [Route("/searchAll")]
        public IActionResult SearchAll(IFormCollection form)
        {
            string search = form["search"];
            ViewData["Title"] = "Tìm kiếm: '" + search + "'";
            ViewBag.PartialView = "SearchAll";
            ViewBag.search = search;
            var _context = new BlogSinhVienNewContext();

            var list = _context.BaiDang
            .Include(x => x.IduserNavigation)
            .Include(x => x.ChiTietBaiDang)
            .Include(x => x.BinhLuan)
            .Where(x => (x.IduserNavigation.Ho + " " + x.IduserNavigation.Ten + " " + x.Content)
            .ToLower().Contains(search))
            .OrderByDescending(x => x.NgayDang)
            .ToList();
            ViewBag.listBaiDang = list;
            ViewBag.listSV = _context.Users.Where(x => (x.Ho + " " + x.Ten).ToLower().Contains(search)).ToList(); ;
            if (list.Count() == 0)
            {
                ViewBag.messSearch = $"Không tìm thấy kết quả phù hợp!";
            }
            return View();
        }
        // {MaCmt:int}
        [HttpGet]
        [Route("/chat/{Id:int}")]
        public IActionResult Chat(int Id)
        {
            var context = new BlogSinhVienNewContext();
            TempData["IdUser"] = Id;
            return RedirectToAction("Index", "Messages");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }


        [HttpGet]
        [Route("/profile/{Id:int}")]
        public IActionResult Profile(int Id)
        {
            var _context = new BlogSinhVienNewContext();
            var stu = _context.Users.Find(Id);
            var list = _context.BaiDang.Include(x => x.IduserNavigation)
                            .Include(x => x.ChiTietBaiDang)
                            .Include(x => x.BinhLuan)
                            .Where(x => x.TrangThai == true && x.Iduser == Id)
                            .OrderByDescending(x => x.NgayDang)
                            .ToList();
            ViewBag.ListPost = list;
            return View(stu);
        }
    }
}