using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Siimple.DAL;
using Siimple.Models;
using Siimple.ViewModels;

namespace Siimple.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles ="Admin")]
    public class SettingController : Controller
    {
        AppDbContext _context { get; }

        public SettingController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int page=1)
        {
            PaginateVM<Setting> paginateVM = new PaginateVM<Setting>();
            paginateVM.MaxPage = (int)Math.Ceiling((decimal)_context.Settings.Count() / 2);
            paginateVM.CurrentPage = page;
            if (page < 1 || page > paginateVM.MaxPage) return BadRequest();
            paginateVM.Items = _context.Settings.Skip((page - 1) * 2).Take(2).ToList();
            return View(paginateVM);
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null || id == 0) return BadRequest();
            var existedsetting = await _context.Settings.FirstOrDefaultAsync(c => c.Id == id);
            if (existedsetting == null) return NotFound();
            UpdateSettingVM settingVM = new UpdateSettingVM
            {
                Key = existedsetting.Key,
                Value = existedsetting.Value
            };
            return View(settingVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, UpdateSettingVM settingVM)
        {
            if (id == null || id == 0) return BadRequest();
            var existedsetting = await _context.Settings.FirstOrDefaultAsync(c => c.Id == id);
            if (existedsetting == null) return NotFound();
            existedsetting.Value= settingVM.Value;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
