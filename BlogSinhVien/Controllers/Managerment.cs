using BlogSinhVien.Models.EntitiesNew;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using MimeKit.Text;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;

namespace BlogSinhVien.Controllers
{
    [Authorize(Roles = "VaiTro01,VaiTro04")]
    [Route("managerment")]
    public class Managerment : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public Managerment(IWebHostEnvironment hostEnvironment)
        {
            webHostEnvironment = hostEnvironment;
        }

        [Route("")]
        public IActionResult Index()
        {
            ViewData["Title"] = "Quản lý";
            return View("QuanLyBaiViet");
        }

        [Route("xoa/{MaBD:int}")]
        public IActionResult XoaBD(int MaBD)
        {
            BlogSinhVienNewContext context = new BlogSinhVienNewContext();

            var tran = context.Database.BeginTransaction();
            try
            {
                var bd = context.BaiDang
                    .Include(x => x.BinhLuan)
                    .Include(x => x.ChiTietBaiDang)
                    .Include(x => x.ToCao)
                    .Where(x => x.Id == MaBD).FirstOrDefault();
                if (bd.BinhLuan != null)
                {
                    foreach (BinhLuan bl in bd.BinhLuan)
                    {
                        context.BinhLuan.Remove(bl);
                    }
                }
                if (bd.ChiTietBaiDang.Count > 0)
                {
                    foreach (ChiTietBaiDang ct in bd.ChiTietBaiDang)
                    {
                        string path = Path.Combine(webHostEnvironment.WebRootPath, "BaiDang", ct.Path, ct.NameFile);
                        System.IO.File.Delete(path);
                        context.ChiTietBaiDang.Remove(ct);
                    }
                }
                if (bd.ToCao.Count > 0)
                {
                    foreach (ToCao bl in bd.ToCao)
                    {
                        context.ToCao.Remove(bl);
                    }
                }
                context.BaiDang.Remove(bd);
                context.SaveChanges();
                tran.Commit();
                TempData["ThongBao"] = "Xoá thành công!";
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                tran.Rollback();
                TempData["ThongBao"] = "Xoá thất bại!";
                return RedirectToAction("Index");
            }

        }

        [Route("duyet/{MaBD:int}")]
        public IActionResult Duyet(int MaBD)
        {
            BlogSinhVienNewContext context = new BlogSinhVienNewContext();
            BaiDang bd = context.BaiDang.Find(MaBD);

            bd.TrangThai = true;

            context.Update(bd);
            context.SaveChanges();
            TempData["ThongBao"] = "Duyệt thành công!";
            return RedirectToAction("Index");
        }

        [HttpPost("search-manager")]
        public IActionResult SearchManager(string key)
        {
            BlogSinhVienNewContext context = new BlogSinhVienNewContext();
            ViewBag.ListBaiDang = context.BaiDang
            .Include(x => x.IduserNavigation)
            .Include(x => x.ChiTietBaiDang)
            .Include(x => x.BinhLuan)
            .Where(x => (x.IduserNavigation.Ho + " " + x.IduserNavigation.Ten + " " + x.Content).ToLower().Contains(key))
            .OrderByDescending(x => x.NgayDang)
            .ToList();
            ViewBag.Loai = "searchs";
            return PartialView("loadMoreBDM");
        }
        [Route("add-sv")]
        public IActionResult AddSV()
        {
            ViewData["Title"] = "Thêm sinh viên";
            return View("add_sv");
        }
        [HttpPost("add-sv")]
        [ValidateAntiForgeryToken]
        public IActionResult InsertSV(Users sv, Accounts accounts)
        {
            if (!ModelState.IsValid)
            {
                return View("add_sv");
            }
            BlogSinhVienNewContext context = new BlogSinhVienNewContext();
            var tran = context.Database.BeginTransaction();
            try
            {
                accounts.TrangThai = true;
                accounts.IdvaiTro = 2;
                context.Accounts.Add(accounts);
                context.SaveChanges();

                sv.Idtk = accounts.Id;
                sv.HinhAnh = "avt.png";
                sv.UserTao = int.Parse(User.Identity.Name);
                sv.NgayTao = DateTime.Now;
                context.Users.Add(sv);
                context.SaveChanges();

                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse("0850080056@sv.hcmunre.edu.vn"));
                email.To.Add(MailboxAddress.Parse(sv.EmailEdu));
                email.Subject = "Thông báo kích hoạt tài khoản BLOG sinh viên HCMUNRE";
                email.Body = new TextPart(TextFormat.Plain) { Text = "Tài khoản: " + accounts.TaiKhoan + "\nMật khẩu: " + accounts.MatKhau };

                // send email
                using var smtp = new SmtpClient();
                smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                smtp.Authenticate("0850080056@sv.hcmunre.edu.vn", "thanhy123");
                smtp.Send(email);
                smtp.Disconnect(true);

                tran.Commit();
                TempData["ThongBao"] = "Thêm sinh viên thành công!";
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                tran.Rollback();
                TempData["ThongBao"] = "Thất bại";
                return RedirectToAction("Index");
            }
        }

        [HttpPost("check-mssv")]
        public bool CheckMSSV(string mssv)
        {
            BlogSinhVienNewContext context = new BlogSinhVienNewContext();
            if (context.Users.Where(x => x.MaSv == mssv).FirstOrDefault() != null)
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
            BlogSinhVienNewContext context = new BlogSinhVienNewContext();
            if (context.Users.Where(x => x.EmailEdu == emailEdu).FirstOrDefault() != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        [HttpPost("trangthai-sv")]
        public string XoaSV(int IDtk)
        {
            BlogSinhVienNewContext context = new BlogSinhVienNewContext();
            Accounts ac = context.Accounts.Find(IDtk);
            if (ac.TrangThai == true)
            {
                ac.TrangThai = false;
                context.Accounts.Update(ac);

                context.SaveChanges();
                return "<button id='trangThai_" + ac.Id + "' onclick='doiTrangThai(" + ac.Id + ")' class='btn btn-success'>Khôi phục</button>";
            }
            else
            {
                ac.TrangThai = true;
                context.Accounts.Update(ac);

                context.SaveChanges();
                return "<button id='trangThai_" + ac.Id + "' onclick='doiTrangThai(" + ac.Id + ")' class='btn btn-danger'>Khoá</button>";
            }


        }
        [HttpPost("searchChart")]
        public IActionResult searchChart(string fromDay, string toDay)
        {
            ViewBag.fromDay = fromDay;
            ViewBag.toDay = toDay;
            return PartialView("searchChart");
        }
        [HttpPost("load-more-bdm")]
        public IActionResult load_more_bdm(int slbd)
        {
            BlogSinhVienNewContext context = new BlogSinhVienNewContext();
            ViewBag.ListBaiDang = context.BaiDang
            .Include(x => x.IduserNavigation)
            .Include(x => x.ChiTietBaiDang)
            .Include(x => x.BinhLuan)
            .OrderByDescending(x => x.NgayDang)
            .Take(slbd + 5)
            .ToList();
            ViewBag.Loai = "more";
            return PartialView("loadMoreBDM");
        }

        [Route("tai-mau-excel")]
        public IActionResult tai_Mau_Excel()
        {
            string pathFile = webHostEnvironment.WebRootPath + "\\MauDSSV.xlsx";
            return File(new FileStream(pathFile, FileMode.Open),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Mẫu.xlsx");
        }
        [Route("update-sinh-vien/{Id:int}")]
        public IActionResult View_SuaSinhVien(int Id)
        {
            ViewData["Title"] = "Chỉnh sửa sinh viên";
            BlogSinhVienNewContext context = new BlogSinhVienNewContext();
            return View(context.Users.Find(Id));
        }

        [HttpPost("edit-sv")]
        public IActionResult UpdateSV(Users sv, Accounts accounts)
        {
            BlogSinhVienNewContext context = new BlogSinhVienNewContext();
            var tran = context.Database.BeginTransaction();
            try
            {
                Users us = context.Users.Find(sv.Id);
                us.MaSv = sv.MaSv;
                us.Ten = sv.Ten;
                us.Ho = sv.Ho;
                us.Cccd = sv.Cccd;
                us.Phone = sv.Phone;
                us.EmailEdu = sv.EmailEdu;
                us.EmailPrivate = sv.EmailPrivate;
                us.GioiTinh = sv.GioiTinh;
                us.DiaChi = sv.DiaChi;
                context.Users.Update(us);
                context.SaveChanges();

                Accounts ac = context.Accounts.Find(us.Idtk);
                ac.TaiKhoan = accounts.TaiKhoan;
                ac.MatKhau = accounts.MatKhau;

                context.Accounts.Update(ac);
                context.SaveChanges();

                tran.Commit();
                TempData["ThongBao"] = "Sửa thông tin sinh viên thành công!";
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                tran.Rollback();
                TempData["ThongBao"] = "Thất bại";
                return RedirectToAction("Index");
            }
        }

        [HttpPost("insert-list-sv")]
        public IActionResult insert_List_SV(IFormFile file)
        {
            Stream stream = new MemoryStream();
            file.CopyTo(stream);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage package = new ExcelPackage(stream))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.End.Row;     //get row count
                BlogSinhVienNewContext context = new BlogSinhVienNewContext();
                List<Users> listU = new List<Users>();
                for (int row = 6; row <= rowCount; row++)
                {
                    string mssv = worksheet.Cells[row, 1].Value?.ToString();
                    if (mssv != null)
                    {
                        var tran = context.Database.BeginTransaction();
                        try
                        {
                            Users users = new Users();
                            users.MaSv = mssv;
                            users.Ten = worksheet.Cells[row, 2].Value?.ToString();
                            users.Ho = worksheet.Cells[row, 3].Value?.ToString();
                            users.EmailPrivate = worksheet.Cells[row, 4].Value?.ToString();
                            users.Cccd = worksheet.Cells[row, 5].Value?.ToString();
                            users.Phone = worksheet.Cells[row, 6].Value?.ToString();
                            users.EmailEdu = worksheet.Cells[row, 7].Value?.ToString();

                            string gt = worksheet.Cells[row, 8].Value.ToString();
                            if (gt == "1")
                            {
                                users.GioiTinh = true;
                            }
                            else
                            {
                                users.GioiTinh = false;
                            }

                            users.DiaChi = worksheet.Cells[row, 9].Value?.ToString();
                            users.GhiChu = worksheet.Cells[row, 10].Value?.ToString();
                            users.HinhAnh = "avt.png";

                            Accounts accounts = new Accounts();
                            accounts.TaiKhoan = users.EmailEdu.Substring(0, 10);
                            accounts.IdvaiTro = 2;
                            accounts.MatKhau = new Random().Next(100000, 999999).ToString();
                            accounts.TrangThai = true;
                            context.Accounts.Add(accounts);
                            context.SaveChanges();

                            users.Idtk = accounts.Id;
                            users.UserTao = int.Parse(User.Identity.Name);
                            users.NgayTao = DateTime.Now;
                            context.Users.Add(users);
                            context.SaveChanges();
                            tran.Commit();
                            TempData["ThongBao"] = "Thêm danh sách sinh viên thành công!";
                        }
                        catch (Exception e)
                        {
                            TempData["ThongBao"] = "Thêm danh sách sinh viên thất bại!";
                            tran.Rollback();
                        }
                    }
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost("xuat-bao-cao")]
        public IActionResult Xuat_Bao_Cao(string fromDay, string toDay)
        {
            BlogSinhVienNewContext context = new BlogSinhVienNewContext();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Báo cáo hoạt động chi tiết");

                var header = worksheet.Cells[2, 1, 3, 10];
                header.Merge = true;
                header.Style.Font.Bold = true;
                header.Style.Font.Size = 14;
                header.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                header.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                header.Value = "BÁO CÁO CHI TIẾT HOẠT ĐỘNG BLOG HCMUNRE";
                header.AutoFitColumns();

                var textNgXuat = worksheet.Cells[5, 1];
                textNgXuat.Style.Font.Bold = true;
                textNgXuat.Style.Font.Size = 12;
                textNgXuat.Value = "Người xuất file:";
                textNgXuat.AutoFitColumns();

                var ngXuat = worksheet.Cells[5, 2];
                ngXuat.Style.Font.Size = 12;
                Users u = context.Users.Find(int.Parse(User.Identity.Name));
                ngXuat.Value = u.Ho + " " + u.Ten;
                ngXuat.AutoFitColumns();

                var textNgayXuat = worksheet.Cells[5, 4];
                textNgayXuat.Style.Font.Bold = true;
                textNgayXuat.Style.Font.Size = 12;
                textNgayXuat.Value = "Ngày xuất:";
                textNgayXuat.AutoFitColumns();

                var NgayXuat = worksheet.Cells[5, 5];
                NgayXuat.Style.Font.Size = 12;
                NgayXuat.Value = DateTime.Now.ToString("dd-MM-yyyy");
                NgayXuat.AutoFitColumns();

                var textFromDay = worksheet.Cells[7, 1];
                textFromDay.Style.Font.Bold = true;
                textFromDay.Style.Font.Size = 12;
                textFromDay.Value = "Từ ngày:";

                var TuNgay = worksheet.Cells[7, 2];
                TuNgay.Style.Font.Size = 12;
                TuNgay.Value = fromDay;
                TuNgay.AutoFitColumns();

                var textToiNgay = worksheet.Cells[7, 4];
                textToiNgay.Style.Font.Bold = true;
                textToiNgay.Style.Font.Size = 12;
                textToiNgay.Value = "Tới ngày:";

                var ToiNgay = worksheet.Cells[7, 5];
                ToiNgay.Style.Font.Size = 12;
                ToiNgay.Value = toDay;
                ToiNgay.AutoFitColumns();

                worksheet.Cells[9, 1].Value = "MSSV";
                worksheet.Cells[9, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                worksheet.Cells[9, 2].Value = "Họ";
                worksheet.Cells[9, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                worksheet.Cells[9, 3].Value = "Tên";
                worksheet.Cells[9, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                worksheet.Cells[9, 4].Value = "Số lượng đăng";
                worksheet.Cells[9, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                worksheet.Cells[9, 5].Value = "Số lượng bình luận";
                worksheet.Cells[9, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                worksheet.Cells[9, 6].Value = "Số lượng vote";
                worksheet.Cells[9, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                worksheet.Cells[9, 7].Value = "Đăng nhập cuối";
                worksheet.Cells[9, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                worksheet.Cells[9, 8].Value = "Bài đăng cuối";
                worksheet.Cells[9, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                worksheet.Cells[9, 9].Value = "Bình luận cuối";
                worksheet.Cells[9, 9].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                worksheet.Cells[9, 10].Value = "Vote cuối";
                worksheet.Cells[9, 10].Style.Border.BorderAround(ExcelBorderStyle.Thin);


                worksheet.Cells[9, 1, 9, 10].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[9, 1, 9, 10].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                worksheet.Cells[9, 1, 9, 10].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                worksheet.Cells[9, 1, 9, 10].Style.Font.Bold = true;
                worksheet.Cells[9, 2, 9, 10].AutoFitColumns();
                worksheet.Cells[9, 1, 9, 10].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells[9, 1, 9, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                var listSV = context.Users.Where(x => x.IdtkNavigation.IdvaiTroNavigation.MaVaiTro == "VaiTro02")
                                          .Include(x => x.IdtkNavigation)
                                          .Include(x => x.IdtkNavigation.IdvaiTroNavigation)
                                          .Include(x => x.BinhLuan)
                                          .Include(x => x.BaiDangIduserNavigation)
                                          .Include(x => x.IdtkNavigation)
                                          .ToList();
                DateTime FromDay = DateTime.ParseExact(fromDay, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                DateTime ToDay = DateTime.ParseExact(toDay, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                for (int i = 10; i < listSV.Count + 10; i++)
                {
                    worksheet.Cells[i, 1].Value = listSV[i - 10].MaSv;
                    worksheet.Cells[i, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    worksheet.Cells[i, 2].Value = listSV[i - 10].Ho;
                    worksheet.Cells[i, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    worksheet.Cells[i, 3].Value = listSV[i - 10].Ten;
                    worksheet.Cells[i, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    worksheet.Cells[i, 4].Value = listSV[i - 10].BaiDangIduserNavigation.Where(x => x.NgayDang >= FromDay.AddHours(12.0) && x.NgayDang <= ToDay.AddHours(12.0)).Count();
                    worksheet.Cells[i, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    worksheet.Cells[i, 5].Value = listSV[i - 10].BinhLuan.Where(x => x.NgayDang >= FromDay.AddHours(12.0) && x.NgayDang <= ToDay.AddHours(12.0)).Count();
                    worksheet.Cells[i, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    worksheet.Cells[i, 6].Value = context.Vote.Where(x => x.TimeVote >= FromDay.AddHours(12.0) && x.TimeVote <= ToDay.AddHours(12.0) && x.MaUser == listSV[i - 10].Id).Count();
                    worksheet.Cells[i, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    if (listSV[i - 10].IdtkNavigation.LastLogin != null)
                    {
                        worksheet.Cells[i, 7].Value = listSV[i - 10].IdtkNavigation.LastLogin.Value.ToString("dd-MM-yyyy");
                    }
                    else
                    {
                        worksheet.Cells[i, 7].Value = "NULL";
                    }
                    worksheet.Cells[i, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    if (listSV[i - 10].BaiDangIduserNavigation.Count() > 0)
                    {
                        worksheet.Cells[i, 8].Value = listSV[i - 10].BaiDangIduserNavigation.OrderByDescending(x => x.NgayDang).FirstOrDefault().NgayDang.ToString("dd-MM-yyyy");
                    }
                    else
                    {
                        worksheet.Cells[i, 8].Value = "NULL";
                    }
                    worksheet.Cells[i, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    if (listSV[i - 10].BinhLuan.Count() > 0)
                    {
                        worksheet.Cells[i, 9].Value = listSV[i - 10].BinhLuan.OrderByDescending(x => x.NgayDang).FirstOrDefault().NgayDang.ToString("dd-MM-yyyy");
                    }
                    else
                    {
                        worksheet.Cells[i, 9].Value = "NULL";
                    }
                    worksheet.Cells[i, 9].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    var vo = context.Vote.Where(x => x.MaUser == listSV[i - 10].Id).ToList();
                    if (vo.Count() > 0)
                    {
                        worksheet.Cells[i, 10].Value = vo.OrderByDescending(x => x.TimeVote).FirstOrDefault().TimeVote.Value.ToString("dd-MM-yyyy");
                    }
                    else
                    {
                        worksheet.Cells[i, 10].Value = "NULL";
                    }
                    worksheet.Cells[i, 10].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                }
                worksheet.Column(2).Width = 30;
                worksheet.Column(3).Width = 30;
                return File(package.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "BaoCaoThongKe.xlsx");
            }
        }
    }
}

