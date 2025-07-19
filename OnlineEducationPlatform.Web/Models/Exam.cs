namespace OnlineEducationPlatform.Web.Models
{
    public class Exam
    {
        public int ExamId { get; set; }
        public string Title { get; set; }
        public string Instructions { get; set; }
        public DateTime AvailableFrom { get; set; }
        public DateTime AvailableTo { get; set; }
        public int TimeLimitMinutes { get; set; }
        public int PassingScore { get; set; }

        // Relationships
        public int ClassId { get; set; }
        public Class Class { get; set; }
        public ICollection<Question> Questions { get; set; }
        public ICollection<ExamSubmission> Submissions { get; set; }
    }
}
