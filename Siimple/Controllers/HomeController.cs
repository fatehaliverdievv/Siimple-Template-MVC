using Microsoft.AspNetCore.Mvc;
using Siimple;
using Siimple.DAL;
using System.Diagnostics;

namespace Siimple.Controllers
{
    public class HomeController : Controller
    {
        AppDbContext _context { get; }
        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_context.Cards.ToList());
        }
    }
}