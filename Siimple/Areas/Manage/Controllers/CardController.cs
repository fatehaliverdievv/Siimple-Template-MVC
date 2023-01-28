using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Siimple.DAL;
using Siimple.Models;
using Siimple.Utilies.Extension;
using Siimple.ViewModels;

namespace Siimple.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Admin")]
    public class CardController : Controller
    {
        AppDbContext _context { get; }
        IWebHostEnvironment _env { get; }

        public CardController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0) return BadRequest();
            var existedcard = await _context.Cards.FirstOrDefaultAsync(c => c.Id == id);
            if (existedcard == null) return NotFound();
            existedcard.ImageUrl.DeleteFile(_env.WebRootPath, "assets/img");
            _context.Cards.Remove(existedcard);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Index(int page = 1)
        {
            PaginateVM<Card> paginateVM = new PaginateVM<Card>();
            paginateVM.MaxPage =(int)Math.Ceiling((decimal)_context.Cards.Count() / 2);
            paginateVM.CurrentPage = page;
            if (page < 1 || page>paginateVM.MaxPage) return BadRequest();
            paginateVM.Items = _context.Cards.Skip((page-1)*2).Take(2).ToList();
            return View(paginateVM);
        }
        public IActionResult Create()
        {
            if (_context.Cards.Count() > 6)
            {
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateCardVM cardVM)
        {
            if (cardVM.Image != null)
            {
                string result = cardVM.Image.CheckValidate("image/", 500);
                if (result.Length > 0)
                {
                    ModelState.AddModelError("Image", result);
                }
            }
            if (!ModelState.IsValid) return View();
            Card card = new Card
            {
                Title = cardVM.Title,
                Description = cardVM.Description,
                ImageUrl = cardVM.Image.SaveFile(Path.Combine(_env.WebRootPath, "assets", "img")),
                IconUrl = cardVM.IconUrl,
            };
            await _context.Cards.AddAsync(card);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null || id == 0) return BadRequest();
            var existedcard = await _context.Cards.FirstOrDefaultAsync(c => c.Id == id);
            if (existedcard == null) return NotFound();
            UpdateCardVM cardVM = new UpdateCardVM
            {
                Title = existedcard.Title,
                Description = existedcard.Description,
                IconUrl = existedcard.IconUrl,
            };
            ViewBag.Image = existedcard.ImageUrl;
            return View(cardVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, UpdateCardVM cardVM)
        {
            if (id == null || id == 0) return BadRequest();
            var existedcard = await _context.Cards.FirstOrDefaultAsync(c => c.Id == id);
            if (existedcard == null) return NotFound();
            if (!ModelState.IsValid)
            {
                ViewBag.Image = existedcard.ImageUrl;

                return View();
            }
            if (cardVM.Image != null)
            {
                string result = cardVM.Image.CheckValidate("image/", 500);
                if (result.Length > 0)
                {
                    ModelState.AddModelError("Image", result);
                    ViewBag.Image = existedcard.ImageUrl;

                    return View();
                }
                string existedimg = existedcard.ImageUrl;
                existedcard.ImageUrl = cardVM.Image.SaveFile(Path.Combine(_env.WebRootPath, "assets", "img"));
                existedimg.DeleteFile(_env.WebRootPath, "assets/img");
            }
            existedcard.Title = cardVM.Title;
            existedcard.Description = cardVM.Description;
            existedcard.IconUrl = cardVM.IconUrl;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}