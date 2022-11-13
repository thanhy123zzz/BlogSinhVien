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
        // {MaCmt:int}
        [HttpGet]
        [Route("/chat/{maSV}")]
        public IActionResult Chat(string maSV)
        {
            Console.WriteLine(maSV);
            string masv1 = User.FindFirst("MaSV").Value;
            var _context = new BlogSinhVienContext();
            var list = _context.Conversation.FromSqlRaw($"SELECT *  FROM [BlogSinhVien].[dbo].[Conversation] Where MaSinhVien1 = '"+masv1+"' and MaSinhVien2 = '"+maSV+"';").ToList();        
            Console.WriteLine(list.Count);
            var conversation = new Conversation();
            conversation.MaSinhVien1 = masv1;
            conversation.MaSinhVien2 = maSV;
            if(list.Count > 0){
                return RedirectToAction("Index","Messages");
            }else{
                _context.Conversation.Add(conversation);
                // _context.Conversation.FromSqlRaw($"Insert into [BlogSinhVien].[dbo].[Conversation](MaSinhVien1,MaSinhVien2) values('"+masv1+"','"+maSV+"');");
                _context.SaveChanges();
                return RedirectToAction("Index","Messages");
            }
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}