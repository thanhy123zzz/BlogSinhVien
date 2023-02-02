using Microsoft.AspNetCore.Mvc;

namespace BlogSinhVien.Controllers
{
	public class ErrorController : Controller
	{
		[Route("/not-found")]
		public IActionResult Index()
		{
			ViewData["Title"] = "Not found";
			return View();
		}
	}
}
