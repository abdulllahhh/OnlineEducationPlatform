namespace OnlineEducationPlatform.Web.ViewModels
{
    public class QuestionViewModel
    {
        public int QuestionId { get; set; }
        public string Text { get; set; }
        public decimal Points { get; set; }
        public List<string>? Options { get; set; }
        public string? CorrectAnswer { get; set; }
    }
}
