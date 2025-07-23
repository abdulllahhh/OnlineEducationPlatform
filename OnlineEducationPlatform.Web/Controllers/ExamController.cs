using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineEducationPlatform.Infrastructure.Data;

namespace OnlineEducationPlatform.Web.Controllers
{
    [Authorize(Roles = "Admin,Instructor")]
    public class ExamController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExamController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var exams = _context.Exams.Include(e => e.Class).ToList();

            return View(exams);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var classes = _context.Classes.ToList();
            if (!classes.Any())
            {
                return NotFound("No classes found.");
            }

            var examViewModel = new ExamViewModel
            {
                Classes = classes.Select(c => new SelectListItem
                {
                    Value = c.ClassId.ToString(),
                    Text = c.ClassName
                }).ToList()
            };

            return View(examViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ExamViewModel exam)
        {
            if (!ModelState.IsValid)
            {
                exam.Classes = _context.Classes.Select(c => new SelectListItem
                {
                    Value = c.ClassId.ToString(),
                    Text = c.ClassName
                }).ToList();

                return View(exam);
            }

            var newExam = new Exam
            {
                Title = exam.Title,
                Instructions = exam.Instructions,
                AvailableFrom = exam.AvailableFrom,
                AvailableTo = exam.AvailableTo,
                TimeLimitMinutes = exam.TimeLimitMinutes,
                PassingScore = exam.PassingScore,
                ClassId = exam.ClassId,
                Questions = exam.Questions?.Select(q => new Question
                {
                    CorrectAnswer = q.CorrectAnswer,
                    Options = q.Options,
                    Points = q.Points,
                    Text = q.Text
                }).ToList() ?? new List<Question>()
            };

            _context.Exams.Add(newExam);
            _context.SaveChanges();

            TempData["Success"] = "Exam created successfully!";
            return RedirectToAction("Index");
        }
        public IActionResult Details(int id)
        {
            var exam = _context.Exams
                .Include(e => e.Class)
                .Include(e => e.Questions)
                .FirstOrDefault(e => e.ExamId == id);

            if (exam == null)
            {
                return NotFound();
            }

            return View(exam);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var exam = _context.Exams
                .Include(e => e.Questions)
                .FirstOrDefault(e => e.ExamId == id);

            if (exam == null) return NotFound();

            var viewModel = new ExamViewModel
            {
                ExamId = exam.ExamId,
                Title = exam.Title,
                Instructions = exam.Instructions,
                AvailableFrom = exam.AvailableFrom,
                AvailableTo = exam.AvailableTo,
                TimeLimitMinutes = exam.TimeLimitMinutes,
                PassingScore = exam.PassingScore,
                ClassId = exam.ClassId,
                Questions = exam.Questions.Select(q => new QuestionViewModel
                {
                    QuestionId = q.QuestionId,
                    Text = q.Text,
                    Points = q.Points,
                    CorrectAnswer = q.CorrectAnswer,
                    Options = q.Options
                }).ToList(),
                Classes = _context.Classes.Select(c => new SelectListItem
                {
                    Value = c.ClassId.ToString(),
                    Text = c.ClassName
                }).ToList()
            };

            return View(viewModel);
        }
        [HttpPost]
        public IActionResult Edit(ExamViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Classes = _context.Classes.Select(c => new SelectListItem
                {
                    Value = c.ClassId.ToString(),
                    Text = c.ClassName
                }).ToList();
                return View(model);
            }

            var exam = _context.Exams
                .Include(e => e.Questions)
                .FirstOrDefault(e => e.ExamId == model.ExamId);

            if (exam == null) return NotFound();

            // Update exam fields
            exam.Title = model.Title;
            exam.Instructions = model.Instructions;
            exam.AvailableFrom = model.AvailableFrom;
            exam.AvailableTo = model.AvailableTo;
            exam.TimeLimitMinutes = model.TimeLimitMinutes;
            exam.PassingScore = model.PassingScore;
            exam.ClassId = model.ClassId;

            // Remove deleted questions
            var incomingIds = model.Questions.Where(q => q.QuestionId != 0).Select(q => q.QuestionId).ToList();
            var toRemove = exam.Questions.Where(q => !incomingIds.Contains(q.QuestionId)).ToList();
            _context.Questions.RemoveRange(toRemove);

            // Update or add questions
            foreach (var q in model.Questions)
            {
                if (q.QuestionId != 0)
                {
                    // Update existing
                    var existing = exam.Questions.FirstOrDefault(x => x.QuestionId == q.QuestionId);
                    if (existing != null)
                    {
                        existing.Text = q.Text;
                        existing.Points = q.Points;
                        existing.CorrectAnswer = q.CorrectAnswer;
                        existing.Options = q.Options;
                    }
                }
                else
                {
                    // Add new
                    exam.Questions.Add(new Question
                    {
                        Text = q.Text,
                        Points = q.Points,
                        CorrectAnswer = q.CorrectAnswer,
                        Options = q.Options,
                        ExamId = exam.ExamId // Ensure new questions are linked to the exam
                    });
                }
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var exam = _context.Exams.Include(e => e.Questions).FirstOrDefault(e => e.ExamId == id);
            if (exam == null)
            {
                return NotFound();
            }
            _context.Questions.RemoveRange(exam.Questions);
            _context.Exams.Remove(exam);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}
