using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineEducationPlatform.Infrastructure.Data;

namespace OnlineEducationPlatform.Web.Controllers
{
    [Authorize(Roles ="Admin")]
    public class SubjectController : Controller
    {
        private readonly ApplicationDbContext _context;
        public SubjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var s = _context.Subjects.ToList();
            if (s == null || !s.Any())
            {
                return NotFound("No subjects found.");
            }
            SubjectViewModel[] subjectViewModels = s.Select(subject => new SubjectViewModel
            {
                Id = subject.SubjectId,
                Name = subject.Name,
                Description = subject.Description
            }).ToArray();
            return View(subjectViewModels);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(SubjectViewModel subjectViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }
            var subject = new Subject
            {
                Name = subjectViewModel.Name,
                Description = subjectViewModel.Description
            };
            _context.Subjects.Add(subject);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var subject = _context.Subjects.Find(id);
            if (subject == null)
            {
                return NotFound();
            }
            _context.Subjects.Remove(subject);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var subject = _context.Subjects.Find(id);
            if (subject == null)
            {
                return NotFound();
            }
            var subjectViewModel = new SubjectViewModel
            {
                Id = subject.SubjectId,
                Name = subject.Name,
                Description = subject.Description
            };
            return View(subjectViewModel);
        }

        [HttpPost]
        public IActionResult Edit(int id, SubjectViewModel subjectViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }
            var subject = _context.Subjects.Find(id);
            if (subject == null)
            {
                return NotFound();
            }
            subject.Name = subjectViewModel.Name;
            subject.Description = subjectViewModel.Description;
            _context.Subjects.Update(subject);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]

/*        [HttpPost]
        public IActionResult AddClassToSubject(int subjectId, int classId)
        {
            var subject = _context.Subjects.Find(subjectId);
            if (subject == null)
            {
                return NotFound();
            }
            var classSubject = new ClassSubject
            {
                SubjectId = subjectId,
                ClassId = classId,
                AddedDate = DateTime.Now
            };
            _context.ClassSubjects.Add(classSubject);
            _context.SaveChanges();
            return RedirectToAction("Details", new { id = subjectId });
        }*/
        public IActionResult Details(int id)
        {
            var subject = _context.Subjects.Find(id);

            if (subject == null)
            {
                return NotFound();
            }

            var subjectClass = _context.ClassSubjects
                .Include(cs => cs.Class)
                .Where(cs => cs.SubjectId == id)
                .ToList();
            if (subjectClass == null)
            {
                return NotFound();
            }
            var subjectDetailsViewModel = new SubjectDetailsViewModel
            {
                Id = subject.SubjectId,
                Name = subject.Name,
                Description = subject.Description,
                Class = _context.ClassSubjects
                    .Where(cs => cs.SubjectId == subject.SubjectId)
                    .Select(cs => cs.Class)
                    .ToList(),
                AddedDate = _context.ClassSubjects
                    .Where(s => s.SubjectId == subject.SubjectId)
                    .Select(s => s.AddedDate)
                    .ToList()
            };
            return View(subjectDetailsViewModel);
        }
    }
}