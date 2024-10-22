using DocumentRepository.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DocumentRepository.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Error(Guid LogID)
        {
            return View(LogID);
        }
    }
}
