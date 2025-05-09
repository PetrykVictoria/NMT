namespace NMT.Models
{
    public class Question
    {
        public int Id { get; set; }

        public int TopicId { get; set; }

        public int QuestionTypeId { get; set; }

        public byte Difficulty { get; set; }
        public string Text { get; set; } = null!;

        public byte[]? Image { get; set; }
        public string Hint { get; set; } = null!;

        public Topic? Topic { get; set; }
        public QuestionType? QuestionType { get; set; }

        public ICollection<AnswerOption> AnswerOptions { get; set; } = new List<AnswerOption>();
    }
}
