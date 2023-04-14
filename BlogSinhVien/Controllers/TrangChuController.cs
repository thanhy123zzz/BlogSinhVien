using BlogSinhVien.Models.EntitiesNew;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;

namespace BlogSinhVien.Controllers
{
    [Authorize]
    public class TrangChuController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly ILogger<TrangChuController> _logger;
        private ICompositeViewEngine _viewEngine;
        public TrangChuController(ILogger<TrangChuController> logger, IWebHostEnvironment hostEnvironment, ICompositeViewEngine viewEngine)
        {
            _logger = logger;
            webHostEnvironment = hostEnvironment;
            _viewEngine = viewEngine;
        }
        [HttpGet]
        [Route("trang-chu")]
        [Route("")]
        public IActionResult Index()
        {
            ViewData["Title"] = "Trang chủ";
            return View("TrangChu");
        }

        [HttpPost("post-new")]
        public IActionResult post_new(IFormFile[] fileImage, IFormFile[] fileDocs, IFormFile[] fileVideo, string content)
        {
            BlogSinhVienNewContext context = new BlogSinhVienNewContext();
            BaiDang baiDang = new BaiDang();
            var tran = context.Database.BeginTransaction();
            try
            {
                baiDang.Content = content;
                baiDang.TrangThai = false;
                baiDang.NgayDang = DateTime.Now;
                baiDang.Slreport = 0;
                baiDang.Iduser = int.Parse(User.Identity.Name);
                baiDang.Ghim = false;
                context.BaiDang.Add(baiDang);
                context.SaveChanges();
                string path = DateTime.Now.ToString("yyyyMMdd");
                string t = webHostEnvironment.WebRootPath + "\\BaiDang\\" + path;
                if (!Directory.Exists(t))
                {
                    Directory.CreateDirectory(t);
                }
                if (fileImage.Count() > 0)
                {
                    foreach (IFormFile file in fileImage)
                    {
                        ChiTietBaiDang ct = new ChiTietBaiDang();
                        ct.NameFile = baiDang.Id + "_" + file.FileName;
                        ct.Path = path;
                        ct.Type = file.ContentType;
                        ct.IdbaiDang = baiDang.Id;
                        context.ChiTietBaiDang.Add(ct);
                        context.SaveChanges();
                        string pathFile = webHostEnvironment.WebRootPath + "\\BaiDang\\" + path + "\\" + ct.NameFile;
                        var fileStream = new FileStream(pathFile, FileMode.Create);
                        file.CopyTo(fileStream);
                        fileStream.Close();
                    }
                }
                if (fileDocs.Count() > 0)
                {
                    foreach (IFormFile file in fileDocs)
                    {
                        ChiTietBaiDang ct = new ChiTietBaiDang();
                        ct.NameFile = baiDang.Id + "_" + file.FileName;
                        ct.Path = path;
                        ct.Type = file.ContentType;
                        ct.IdbaiDang = baiDang.Id;
                        context.ChiTietBaiDang.Add(ct);
                        context.SaveChanges();
                        string pathFile = webHostEnvironment.WebRootPath + "\\BaiDang\\" + path + "\\" + ct.NameFile;
                        var fileStream = new FileStream(pathFile, FileMode.Create);
                        file.CopyTo(fileStream);
                        fileStream.Close();
                    }
                }
                if (fileVideo.Count() > 0)
                {
                    foreach (IFormFile file in fileVideo)
                    {
                        ChiTietBaiDang ct = new ChiTietBaiDang();
                        ct.NameFile = baiDang.Id + "_" + file.FileName;
                        ct.Path = path;
                        ct.Type = file.ContentType;
                        ct.IdbaiDang = baiDang.Id;
                        context.ChiTietBaiDang.Add(ct);
                        context.SaveChanges();
                        string pathFile = webHostEnvironment.WebRootPath + "\\BaiDang\\" + path + "\\" + ct.NameFile;
                        var fileStream = new FileStream(pathFile, FileMode.Create);

                        file.CopyTo(fileStream);
                        fileStream.Close();
                    }
                }

                tran.Commit();
            }
            catch (Exception e)
            {
                tran.Rollback();
                TempData["ThongBao"] = e.Message;
                return RedirectToAction("Index");
            }
            TempData["ThongBao"] = "Đăng bài thành công, đang chờ duyệt!";
            return RedirectToAction("Index");
        }
        [AllowAnonymous]
        [Route("/download")]
        public IActionResult Download(string path, string tenFile, string type)
        {
            string pathFile = webHostEnvironment.WebRootPath + "\\BaiDang\\" + path + "\\" + tenFile;
            return File(new FileStream(pathFile, FileMode.Open), type, tenFile);
        }

        [HttpPost("load-more-cmt")]
        public IActionResult load_more_cmt(int MaBD, int sl)
        {
            BlogSinhVienNewContext context = new BlogSinhVienNewContext();
            ViewBag.ListCmt = context.BinhLuan.Where(x => x.IdbaiDang == MaBD).OrderByDescending(x => x.Sllike).Take(sl + 5).ToList();
            return PartialView("_list_cmt");
        }
        [HttpPost("load-more-bd")]
        public IActionResult load_more_bd(int slbd)
        {
            BlogSinhVienNewContext context = new BlogSinhVienNewContext();
            ViewBag.ListBaiDang = context.BaiDang.Include(x => x.IduserNavigation)
            .Include(x => x.ChiTietBaiDang)
            .Include(x => x.BinhLuan)
            .Where(x => x.TrangThai == true && x.Ghim == false)
            .OrderByDescending(x => x.NgayDang)
            .Take(slbd + 5)
            .ToList();
            return PartialView("_baiDang");
        }
        [HttpPost("/bao-cao-bai-dang")]
        public IActionResult baoCaoBaiDang(int idBD, int idUser)
        {
            BlogSinhVienNewContext context = new BlogSinhVienNewContext();
            ToCao c = context.ToCao.Where(x => x.Idbd == idBD && x.Iduser == idUser).FirstOrDefault();
            BaiDang bd = context.BaiDang.Find(idBD);
            if (c != null)
            {
                return Ok("Bạn đã báo cáo bài viết này");
            }
            else
            {
                ToCao v = new ToCao();
                v.Iduser = idUser;
                v.NgayToCao = DateTime.Now;
                v.Idbd = idBD;
                bd.Slreport += 1;
                context.BaiDang.Update(bd);
                context.Add(v);
                context.SaveChanges();
                return Ok("Báo cáo thành công");
            }
        }
        [HttpPost("/ghim-bai-dang")]
        public IActionResult Ghim(int idBD, int idUser)
        {
            BlogSinhVienNewContext context = new BlogSinhVienNewContext();
            BaiDang bd = context.BaiDang.Find(idBD);
            bd.Ghim = true;
            bd.IdUserGhim = idUser;
            bd.NgayGhim = DateTime.Now;
            context.BaiDang.Update(bd);
            context.SaveChanges();
            return Ok("Ghim thành công");
        }
        [HttpPost("/huyghim-bai-dang")]
        public IActionResult HuyGhim(int idBD, int slbd)
        {
            BlogSinhVienNewContext context = new BlogSinhVienNewContext();
            BaiDang bd = context.BaiDang.Find(idBD);
            bd.Ghim = false;
            bd.IdUserGhim = null;
            bd.NgayGhim = null;
            context.BaiDang.Update(bd);
            context.SaveChanges();
            ViewBag.ListBaiDang = context.BaiDang.Include(x => x.IduserNavigation)
            .Include(x => x.ChiTietBaiDang)
            .Include(x => x.BinhLuan)
            .Where(x => x.TrangThai == true && x.Ghim == false)
            .OrderByDescending(x => x.NgayDang)
            .Take(slbd)
            .ToList();

            PartialViewResult partialViewResult = PartialView("_baiDang");

            string viewContent = ConvertViewToString(ControllerContext, partialViewResult, _viewEngine);

            return Json(new
            {
                message = "Huỷ ghim thành công",
                view = viewContent
            }
                );
        }
        [HttpPost("remove-cmt")]
        public IActionResult RemoveCmt(int id)
        {
            BlogSinhVienNewContext context = new BlogSinhVienNewContext();

            context.Database.ExecuteSqlRaw("delete Vote where MaCmt = " + id);

            BinhLuan bl = context.BinhLuan.Find(id);
            context.BinhLuan.Remove(bl);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
        public string ConvertViewToString(ControllerContext controllerContext, PartialViewResult pvr, ICompositeViewEngine _viewEngine)
        {
            using (StringWriter writer = new StringWriter())
            {
                ViewEngineResult vResult = _viewEngine.FindView(controllerContext, pvr.ViewName, false);
                ViewContext viewContext = new ViewContext(controllerContext, vResult.View, pvr.ViewData, pvr.TempData, writer, new HtmlHelperOptions());

                vResult.View.RenderAsync(viewContext);

                return writer.GetStringBuilder().ToString();
            }
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
