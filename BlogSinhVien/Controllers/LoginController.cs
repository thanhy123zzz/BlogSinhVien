using BlogSinhVien.Models.EntitiesNew;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

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
                return Redirect(returnUrl);
            }
        }

        [HttpPost]
        [Route("/login")]
        public async Task<IActionResult> LoginUser(Accounts accounts, string returnUrl)
        {
            BlogSinhVienNewContext context = new BlogSinhVienNewContext();
            Accounts a = context.Accounts
                .Include(x => x.IdvaiTroNavigation)
                .FirstOrDefault(x => x.TaiKhoan == accounts.TaiKhoan);
            if (a == null)
            {
                return Redirect("/login");
            }
            if (a.MatKhau.Equals(accounts.MatKhau) && a.TrangThai == true)
            {
                await SignInUser(a);
                a.LastLogin = DateTime.Now;
                context.Accounts.Update(a);
                context.SaveChanges();
                if (string.IsNullOrWhiteSpace(returnUrl) || !returnUrl.StartsWith("/"))
                {
                    returnUrl = "/";
                }
                return Redirect(returnUrl);
            }
            else
            {
                TempData["LoginFailed"] = true;
                return RedirectToAction("Index");
            }
        }

        private async Task SignInUser(Accounts accounts)
        {
            BlogSinhVienNewContext context = new BlogSinhVienNewContext();
            Users user = context.Users.Where(x => x.Idtk == accounts.Id).FirstOrDefault();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Id.ToString()),
                new Claim(ClaimTypes.Role, accounts.IdvaiTroNavigation.MaVaiTro),
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }

        [HttpPost("send-mail")]
        public bool SendGmail(string Email)
        {
            BlogSinhVienNewContext context = new BlogSinhVienNewContext();
            var sv = context.Users.Where(k => k.EmailEdu == Email).ToList();
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
            if (Code == HttpContext.Session.GetInt32("code"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        [HttpPost("change-pass")]
        public bool ChangePass(string Email, string Pass)
        {
            BlogSinhVienNewContext context = new BlogSinhVienNewContext();
            var sv = context.Users.Where(k => k.EmailEdu == Email).FirstOrDefault();
            if (sv != null)
            {

                context.Database.ExecuteSqlRaw("update Accounts set MatKhau ='" + Pass + "' where TaiKhoan = '" + sv.Idtk + "'");
                return true;
            }
            else return false;
        }
    }
}
