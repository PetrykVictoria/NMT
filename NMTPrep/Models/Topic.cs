namespace NMT.Models
{
    public class Topic
    {
        public int Id { get; set; }

        public int SectionId { get; set; }
        public string Name { get; set; } = null!;

        public string TheoryHtml { get; set; } = null!;


        public Section? Section { get; set; } 

        public ICollection<Question> Questions { get; set; } = new List<Question>();

        public ICollection<Result> Results { get; set; } = new List<Result>();

    }
}
