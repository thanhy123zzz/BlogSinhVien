using BlogSinhVien.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogSinhVien.Controllers
{
    [Authorize(Roles = "QL")]
    [Route("managerment")]
	public class Managerment : Controller
	{
		[Route("")]
		public IActionResult Index()
		{
			return RedirectToAction("BaiViet");
		}
		[Route("baiviet")]
        public IActionResult BaiViet()
        {
            return View("QuanLyBaiViet");
        }
    }
}
