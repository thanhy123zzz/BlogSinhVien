using BlogSinhVien.Models.Entities;
using MailKit.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore;
using MimeKit.Text;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MailKit.Net.Smtp;

namespace BlogSinhVien.Controllers
{
    [Authorize(Roles = "QL")]
    [Route("managerment")]
	public class Managerment : Controller
	{
        private IHostingEnvironment _hostingEnv;
        public Managerment(IHostingEnvironment hostingEnv)
        {
            _hostingEnv = hostingEnv;
        }

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
        [Route("add-sv")]
        public IActionResult AddSV()
        {
            return View("add_sv");
        }
        [HttpPost("add-sv")]
        [ValidateAntiForgeryToken]
        public IActionResult InsertSV(SinhVien sv,string MatKhau)
        {
            if (!ModelState.IsValid)    
            {
                return View("add_sv");
            }
            BlogSinhVienContext context = new BlogSinhVienContext();
            context.SinhVien.Add(sv);
            Accounts accounts = new Accounts();
            accounts.TaiKhoan = sv.TaiKhoan;
            accounts.MatKhau = MatKhau;
            accounts.MaRole = "SV1";
            context.Add(accounts);
            context.SaveChanges();

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("0850080056@sv.hcmunre.edu.vn"));
            email.To.Add(MailboxAddress.Parse(sv.EmailEdu));
            email.Subject = "Thông báo Tài khoản BLOG sinh viên HCMUNRE";
            email.Body = new TextPart(TextFormat.Plain) { Text = "Tài khoản: " + sv.TaiKhoan + "\nMật khẩu: "+accounts.MatKhau };

            // send email
            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("0850080056@sv.hcmunre.edu.vn", "thanhy123");
            smtp.Send(email);
            smtp.Disconnect(true);

            TempData["ThongBao"] = "Thêm sinh viên thành công!";
            return RedirectToAction("Index");
        }

        [HttpPost("check-mssv")]
        public bool CheckMSSV(string mssv)
        {
            BlogSinhVienContext context = new BlogSinhVienContext();
            if (context.SinhVien.Find(mssv) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        [HttpPost("check-emailEdu")]
        public bool CheckEmailEdu(string emailEdu)
        {
            BlogSinhVienContext context = new BlogSinhVienContext();
            if (context.SinhVien.Where(x=>x.EmailEdu==emailEdu).FirstOrDefault() != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        [Route("trangthai-sv/{taikhoan}")]
        public IActionResult XoaSV(string taikhoan)
        {
            BlogSinhVienContext context = new BlogSinhVienContext();
            Accounts ac = context.Accounts.Find(taikhoan);
            if (ac.TrangThai == true)
            {
                ac.TrangThai = false;
                context.Accounts.Update(ac);
            }
            else
            {
                ac.TrangThai = true;
                context.Accounts.Update(ac);
            }
            TempData["ThongBao"] = "Đổi trạng thái thành công!";
            context.SaveChanges();
            return RedirectToAction("Index");

        }
        /*[HttpPost("insert-info-sv")]
        public async Task<IActionResult> insert_infor_sv(IFormFile excelFile)
        {
            //get file name
            var filename = ContentDispositionHeaderValue.Parse(excelFile.ContentDisposition).FileName.Trim('"');

            //get path
            var MainPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads");

            //create directory "Uploads" if it doesn't exists
            if (!Directory.Exists(MainPath))
            {
                Directory.CreateDirectory(MainPath);
            }

            //get file path 
            var filePath = Path.Combine(MainPath, excelFile.FileName);
            using (System.IO.Stream stream = new FileStream(filePath, FileMode.Create))
            {
                await excelFile.CopyToAsync(stream);
            }

            //get extension
            string extension = Path.GetExtension(filename);


            string conString = string.Empty;

            switch (extension)
            {
                case ".xls": //Excel 97-03.
                    conString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=YES'";
                    break;
                case ".xlsx": //Excel 07 and above.
                    conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";" + "Extended Properties='Excel 12.0;HDR=YES;'";
                    break;
            }

            DataTable dt = new DataTable();
            using (OleDbConnection connExcel = new OleDbConnection(conString))
            {
                using (OleDbCommand cmdExcel = new OleDbCommand())
                {
                    using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                    {
                        cmdExcel.Connection = connExcel;

                        //Get the name of First Sheet.
                        connExcel.Open();
                        DataTable dtExcelSchema;
                        dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                        string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                        connExcel.Close();

                        //Read Data from First Sheet.
                        connExcel.Open();
                        cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                        odaExcel.SelectCommand = cmdExcel;
                        odaExcel.Fill(dt);
                        connExcel.Close();
                    }
                }
            }
            return View();
        }*/
    }
}
