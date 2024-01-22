using Mairala202.Areas.Admin.ViewModels;
using Mairala202.DAL;
using Mairala202.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mairala202.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SettingController : Controller
    {
        private readonly AppDbContext _db;

        public SettingController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            
            return View(await _db.Settings.ToListAsync());

        }
        public async Task<IActionResult> Update(int id)
        {
            if (id<=0)
            {
                return BadRequest();
            }
            Setting setting = await _db.Settings.FirstOrDefaultAsync(x => x.Id == id);
            if (setting == null)
            {
                return NotFound();
            }
            UpdateSettingVM vm = new UpdateSettingVM 
            {
                Value = setting.Value,
            };
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id,UpdateSettingVM vm)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            Setting setting = await _db.Settings.FirstOrDefaultAsync(x => x.Id == id);
            if (setting == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            setting.Value = vm.Value;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}
