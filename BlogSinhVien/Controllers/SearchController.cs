using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BlogSinhVien.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BlogSinhVien.Controllers
{
    // [Route("[search]")]
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
            String search = form["search"];
            ViewBag.PartialView = "SearchAll";
            ViewBag.search = search;
            var _context = new BlogSinhVienContext();
            
            var list  = _context.BaiDang.FromSqlRaw($"(select MaBaiDang,bd.MaSinhVien,bd.MaQL,bd.NgayDang,TrangThai, CAST(bd.Content AS NVARCHAR(MAX)) as 'content' from BaiDang bd join SinhVien sv on sv.MaSV = bd.MaSinhVien Where CONCAT_WS(' ',Content,sv.Ho,sv.Ten)  like N'%"+search+"%')"
              +"union"
              +"(select MaBaiDang,bd.MaSinhVien,bd.MaQL,bd.NgayDang,TrangThai,CAST(bd.Content AS NVARCHAR(MAX)) from BaiDang bd join QuanLy ql on ql.MaQL = bd.MaQL Where CONCAT_WS(' ',Content,ql.Ho,ql.Ten)  like N'%"+search+"%');").ToList();
            var list2 = _context.SinhVien.ToList();
            var list3 = _context.QuanLy.ToList();
            var listnewsv = new List<SinhVien>(); 
            foreach(BaiDang x in list){
                foreach(SinhVien sv in list2){
                    if(x.MaSinhVien == sv.MaSv){
                        if(listnewsv.Contains(sv) == true){
                            
                        }else{
                        listnewsv.Add(sv);
                        Console.WriteLine(listnewsv.Count);
                        }
                    }
                }
            }
                TempData["listBaiDang"] = list;
                // TempData["listSV"] = list2;
                ViewBag.listQL = list3;
                ViewBag.listSV = listnewsv;
                if(list.Count() == 0){
                    ViewBag.messSearch = $"Không tìm thấy kết quả phù hợp!";
                }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}