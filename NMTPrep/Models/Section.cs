namespace NMT.Models
{
    public class Section
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public bool Category { get; set; } 

        public ICollection<Topic> Topics { get; set; } = new List<Topic>();
    }
}
