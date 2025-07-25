using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineEducationPlatform.Infrastructure.Data;

namespace OnlineEducationPlatform.Web.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentAssignmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentAssignmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var studentId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var assignments = _context.Assignments
                .Include(a => a.Submissions)
                .Select(a => new AssignmentListItemViewModel
                {
                    AssignmentId = a.AssignmentId,
                    Title = a.Title,
                    DueDate = a.DueDate,
                    Description = a.Description,
                    SubjectName = a.Subject.Name,
                    AssignmentPath = a.FilePath,
                    HasSubmitted = a.Submissions.Any(s => s.StudentId == studentId),
                    Score = a.Submissions
                            .Where(s => s.StudentId == studentId)
                            .Select(s => s.Score)
                            .FirstOrDefault(),
                    SubmitionPath = a.Submissions
                                    .Where(s => s.StudentId == studentId)
                                    .Select(s => s.FilePath)
                                    .FirstOrDefault(),
                }).ToList();
            return View(assignments);
        }
    }
}
