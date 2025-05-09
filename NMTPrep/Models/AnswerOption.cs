namespace NMT.Models
{
    public class AnswerOption
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string OptionText { get; set; } = null!;
        public bool IsCorrect { get; set; }
        public Question? Question { get; set; }

    }
}
