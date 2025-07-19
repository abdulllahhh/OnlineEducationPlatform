namespace OnlineEducationPlatform.Web.Models
{
    public class ExamSubmission
    {
        public int SubmissionId { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? SubmittedAt { get; set; }
        public Dictionary<int, string>? Answers { get; set; } // Serialized question/answer pairs
        public decimal? Score { get; set; }

        // Relationships
        public int ExamId { get; set; }
        public Exam Exam { get; set; }
        public string StudentId { get; set; }
        public Student Student { get; set; }
    }
}
