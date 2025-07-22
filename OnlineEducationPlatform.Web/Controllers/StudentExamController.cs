using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineEducationPlatform.Infrastructure.Data;
using OnlineEducationPlatform.Web.ViewModels;

namespace OnlineEducationPlatform.Web.Controllers
{
    [Authorize(Roles = "Student")]

    public class StudentExamController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentExamController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var exams = _context.Exams
                .Where(e => DateTime.Now >= e.AvailableFrom && DateTime.Now <= e.AvailableTo)
                .Select(e => new ExamListItemViewModel
                {
                    ExamId = e.ExamId,
                    Title = e.Title,
                    Instructions = e.Instructions,
                    AvailableFrom = e.AvailableFrom,
                    AvailableTo = e.AvailableTo
                }).ToList();

            return View(exams);
        }

        [HttpGet]
        public IActionResult Solve(int id)
        {
            var exam = _context.Exams
                .Include(e => e.Questions)
                .FirstOrDefault(e => e.ExamId == id);

            if (exam == null) return NotFound();

            var viewModel = new StudentSolveExamViewModel
            {
                ExamId = exam.ExamId,
                Title = exam.Title,
                Questions = exam.Questions.Select(q => new StudentQuestionAnswerViewModel
                {
                    QuestionId = q.QuestionId,
                    Text = q.Text,
                    Options = q.Options
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Solve(StudentSolveExamViewModel model)
        {
            var exam = _context.Exams
                .Include(e => e.Questions)
                .FirstOrDefault(e => e.ExamId == model.ExamId);

            if (exam == null) return NotFound();

            int totalScore = 0;
            int score = 0;

            foreach (var question in exam.Questions)
            {
                totalScore += (int)question.Points;

                var studentAnswer = model.Questions.FirstOrDefault(q => q.QuestionId == question.QuestionId)?.Answer;

                if (!string.IsNullOrWhiteSpace(studentAnswer) && studentAnswer == question.CorrectAnswer)
                {
                    score += (int)question.Points;
                }
            }

            ViewBag.TotalScore = totalScore;
            ViewBag.StudentScore = score;

            return View("Result");
        }
    }
}
