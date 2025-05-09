namespace NMT.Models
{
    public class TestResultVM
    {
        public int QuestionId { get; set; }
        public string Text { get; set; } = "";
        public List<AnswerVM> Answers { get; set; } = new();
        public int SelectedAnswerId { get; set; }
        public bool IsCorrect { get; set; }
    }
}
