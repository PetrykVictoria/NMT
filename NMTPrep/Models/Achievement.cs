namespace NMT.Models
{
    public class Achievement
    {
        public int Id { get; set; }

        public string UserId { get; set; } = null!;
        public User? User { get; set; }

        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime DateEarned { get; set; } 
    }

}
