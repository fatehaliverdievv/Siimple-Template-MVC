using Microsoft.AspNetCore.Mvc;
using Siimple.DAL;
namespace Siimple.ViewComponents
{
    public class FooterViewComponent:ViewComponent
    {
        AppDbContext _context { get; }

        public FooterViewComponent(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(_context.Settings.ToDictionary(s => s.Key, s => s.Value));
        }
    }
}
