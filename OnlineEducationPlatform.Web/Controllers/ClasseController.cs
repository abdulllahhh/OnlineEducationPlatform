using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineEducationPlatform.Infrastructure.Data;
using OnlineEducationPlatform.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineEducationPlatform.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ClassController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClassController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Class
        public async Task<IActionResult> Index()
        {
            var classList = await _context.Classes.Include(c => c.Teacher).ToListAsync();
            // You can set a breakpoint here and inspect classList
            return View(classList);
        }

        // GET: Class/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @class = await _context.Classes
                .Include(c => c.Teacher)
                .FirstOrDefaultAsync(m => m.ClassId == id);
            if (@class == null)
            {
                return NotFound();
            }

            return View(@class);
        }

        // GET: Class/Create
        public IActionResult Create()
        {
            var data = new ClassTeacherViewModel()
            {
                Teachers =  _context.Users.Where(x => x.Role == "Instructor").ToList()
            };
            return View(data);
        }

        // POST: Class/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClassTeacherViewModel model)
        {
            // Validate TeacherId
            if (string.IsNullOrEmpty(model.TeacherId) || model.TeacherId == "0")
            {
                ModelState.AddModelError("TeacherId", "Please select a valid teacher.");
            }

            if (ModelState.IsValid)
            {
                var newClass = new Class
                {
                    ClassName = model.ClassName,
                    TeacherId = model.TeacherId
                };
                _context.Add(newClass);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            // Repopulate teacher list for redisplay
            model.Teachers = _context.Users.Where(x => x.Role == "Instructor").ToList();
            return View(model);
        }

        // GET: Class/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @class = await _context.Classes.FindAsync(id);
            if (@class == null)
            {
                return NotFound();
            }
            var model = new ClassTeacherViewModel
            {
                ClassId = @class.ClassId,
                ClassName = @class.ClassName,
                TeacherId = @class.TeacherId,
                Teachers = _context.Users.Where(x => x.Role == "Instructor").ToList()
            };
            return View(model);
        }

        // POST: Class/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ClassTeacherViewModel model)
        {
            if (id != model.ClassId)
            {
                return NotFound();
            }
            if (string.IsNullOrEmpty(model.TeacherId) || model.TeacherId == "0")
            {
                ModelState.AddModelError("TeacherId", "Please select a valid teacher.");
            }
            if (ModelState.IsValid)
            {
                var @class = await _context.Classes.FindAsync(id);
                if (@class == null)
                {
                    return NotFound();
                }
                @class.ClassName = model.ClassName;
                @class.TeacherId = model.TeacherId;
                _context.Update(@class);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            // Repopulate teacher list for redisplay
            model.Teachers = _context.Users.Where(x => x.Role == "Instructor").ToList();
            return View(model);
        }

        // GET: Class/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @class = await _context.Classes
                .Include(c => c.Teacher)
                .FirstOrDefaultAsync(m => m.ClassId == id);
            if (@class == null)
            {
                return NotFound();
            }

            return View(@class);
        }

        // POST: Class/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @class = await _context.Classes.FindAsync(id);
            if (@class != null)
            {
                _context.Classes.Remove(@class);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClassExists(int id)
        {
            return _context.Classes.Any(e => e.ClassId == id);
        }
    }
}
