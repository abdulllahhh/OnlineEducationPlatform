using Microsoft.AspNetCore.Mvc.Rendering;

namespace OnlineEducationPlatform.Web.ViewModels
{
    public class ExamViewModel
    {
        public int ExamId { get; set; }
        public string Title { get; set; }
        public string Instructions { get; set; }
        public DateTime AvailableFrom { get; set; }
        public DateTime AvailableTo { get; set; }
        public int TimeLimitMinutes { get; set; }
        public int PassingScore { get; set; }

        // Dropdown selection
        public int ClassId { get; set; }
        public List<SelectListItem> Classes { get; set; } = new List<SelectListItem>();

        // Questions for the exam
        public ICollection<QuestionViewModel> Questions { get; set; } = new List<QuestionViewModel>();
    }
}
