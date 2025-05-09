namespace NMT.Models
{
    public class Result
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public int TopicId { get; set; }
        public int Score { get; set; }

        public bool IsPerfect { get; set; }
        public TimeSpan Duration { get; set; }
        public User? User { get; set; }
        public Topic? Topic { get; set; }

    }
}
