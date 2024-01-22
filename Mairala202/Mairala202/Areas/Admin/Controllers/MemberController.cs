using Mairala202.Areas.Admin.ViewModels;
using Mairala202.DAL;
using Mairala202.Models;
using Mairala202.Utilities.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mairala202.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class MemberController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        

        public MemberController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        public async Task<IActionResult> Index(int page =1)
        {
            int total = await _db.Members.CountAsync();
            int limit = 3;
            int tp = total/ limit;
            if (total%limit>0)
            {
                tp++;
            }
            if (page>tp || page<=0)
            {
                return BadRequest();
            }
            PaginationVM<Member> vm = new PaginationVM<Member>
            {
                Items = await _db.Members.Skip((page - 1) * limit).Take(limit).ToListAsync(),
                Limit = limit,
                TotalPage= tp,
                CurrentPage=page
            };
            return View(vm);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateMemberVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);

            }
            if (vm.Photo.CheckFileSize(2))
            {
                ModelState.AddModelError("Photo", "Cannot exceed 2mb");
                return View(vm);
            }
            if (!vm.Photo.CheckFileType("image"))
            {
                ModelState.AddModelError("Photo", "Only images");
                return View(vm);
            }
            Member member = new Member
            {
                Name = vm.Name,
                Position= vm.Position,
                Image = await vm.Photo.CreateFileAsync(_env.WebRootPath,"assets","images")
            };
            await _db.Members.AddAsync(member);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            if (id<=0)
            {
                return BadRequest();
            }
            Member member = await _db.Members.FirstOrDefaultAsync(x => x.Id == id);
            if (member is null)
            {
                return NotFound();
            }
            UpdateMemberVM vm = new UpdateMemberVM 
            {
                Name = member.Name,
                Position = member.Position,
                Image = member.Image,
            };
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id,UpdateMemberVM vm)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            Member member = await _db.Members.FirstOrDefaultAsync(x => x.Id == id);
            if (member is null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            if (vm.Photo is not null)
            {
                if (vm.Photo.CheckFileSize(2))
                {
                    ModelState.AddModelError("Photo", "Cannot exceed 2mb");
                    return View(vm);
                }
                if (!vm.Photo.CheckFileType("image"))
                {
                    ModelState.AddModelError("Photo", "Only images");
                    return View(vm);
                }
                vm.Image.DeleteFile(_env.WebRootPath, "assets", "images");
                member.Image = await vm.Photo.CreateFileAsync(_env.WebRootPath, "assets", "images");

            }
            member.Name = vm.Name;
            member.Position = vm.Position;
             await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            Member member = await _db.Members.FirstOrDefaultAsync(x => x.Id == id);
            if (member is null)
            {
                return NotFound();
            }
            member.Image.DeleteFile(_env.WebRootPath, "assets", "images");
            _db.Members.Remove(member);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
