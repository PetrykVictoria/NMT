namespace NMT.Models
{
    public class TestQuestionVM
    {
        public int QuestionId { get; set; }
        public string Text { get; set; } = "";
        public List<AnswerVM> Answers { get; set; } = new();
    }
    public class AnswerVM
    {
        public int Id { get; set; }
        public string Text { get; set; } = "";
        public bool IsCorrect { get; set; }  
    }

}
