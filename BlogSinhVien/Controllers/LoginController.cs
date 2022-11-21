using BlogSinhVien.Models.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Security.Claims;
using BlogSinhVien.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace BlogSinhVien.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;

        public LoginController(ILogger<LoginController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        [Route("/login")]
        public IActionResult Index(string returnUrl)
        {


            if (HttpContext.User.Identity.Name == null)
                return View("login");
            else
            {
                if (string.IsNullOrWhiteSpace(returnUrl) || !returnUrl.StartsWith("/"))
                {
                    returnUrl = "/";
                }
                return this.Redirect(returnUrl);
            }
        }

        [HttpPost]
        [Route("/login")]
        public async Task<IActionResult> LoginUser(Accounts accounts, string returnUrl)
        {
            BlogSinhVienContext context = new BlogSinhVienContext();
            Accounts a = context.Accounts.Find(accounts.TaiKhoan);
            if (a == null)
            {
                return Redirect("/login");
            }
            if (a.MatKhau.Trim().Equals(accounts.MatKhau.Trim())&&a.TrangThai==true)
            {
                await SignInUser(a);
                if (string.IsNullOrWhiteSpace(returnUrl) || !returnUrl.StartsWith("/"))
                {
                    returnUrl = "/";
                }
                return this.Redirect(returnUrl);
            }
            else
            {
                TempData["LoginFailed"] = true;
                return RedirectToAction("Index");
            }
        }

        private async Task SignInUser(Accounts accounts)
        {
            if (accounts.MaRole.Trim() == "SV1")
            {
                BlogSinhVienContext context = new BlogSinhVienContext();
                SinhVien a = context.SinhVien.Where(x => x.TaiKhoan == accounts.TaiKhoan).FirstOrDefault();
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, accounts.TaiKhoan),
                new Claim(ClaimTypes.Role, accounts.MaRole.Substring(0,2)),
                new Claim("MaSV",a.MaSv),
            };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));
            }
            else
            {
                BlogSinhVienContext context = new BlogSinhVienContext();
                QuanLy a = context.QuanLy.Where(x => x.TaiKhoan == accounts.TaiKhoan).FirstOrDefault();
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, accounts.TaiKhoan),
                new Claim(ClaimTypes.Role, accounts.MaRole.Substring(0,2)),
                new Claim("MaSV",a.MaQl),
            };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));
            }
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }

        [HttpPost("send-mail")]
        public bool SendGmail(String Email)
        {
            BlogSinhVienContext context = new BlogSinhVienContext();
            var sv = context.SinhVien.Where(k => k.EmailEdu.Trim() == Email).ToList();
            if (sv.Count() != 0)
            {
                var email = new MimeMessage();
                _logger.LogInformation("hi");
                Random random = new Random();
                int a = random.Next(1000, 9999);
                HttpContext.Session.SetInt32("code", a);
                email.From.Add(MailboxAddress.Parse("0850080056@sv.hcmunre.edu.vn"));
                email.To.Add(MailboxAddress.Parse(Email));
                email.Subject = "Mã xác nhận đổi mật khẩu tài khoản đăng nhập";
                email.Body = new TextPart(TextFormat.Plain) { Text = "Mã xác thực của bạn là: " + a };

                // send email
                using var smtp = new SmtpClient();
                smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                smtp.Authenticate("0850080056@sv.hcmunre.edu.vn", "thanhy123");
                smtp.Send(email);
                smtp.Disconnect(true);

                return true;
            }
            else
            {
                return false;
            }
        }
        [HttpPost("confirm-code")]
        public bool ComfirmCode(int Code)
        {
           if(Code == HttpContext.Session.GetInt32("code"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        [HttpPost("change-pass")]
        public bool ChangePass(String Email, String Pass)
        {
            BlogSinhVienContext context = new BlogSinhVienContext();
            var sv = context.SinhVien.Where(k => k.EmailEdu.Trim() == Email);
            if (sv != null)
            {
                context.Database.ExecuteSqlRaw("update Accounts set MatKhau ='" + Pass + "' where TaiKhoan = '"+ sv.First().TaiKhoan+ "'");
                return true;
            }
            else return false;
        }
    }
}
