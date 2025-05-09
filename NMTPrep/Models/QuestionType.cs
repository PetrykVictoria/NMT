using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace NMT.Models
{
    public class QuestionType
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;

        public ICollection<Question> Questions { get; set; } = new List<Question>();
    }
}
