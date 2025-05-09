using Microsoft.AspNetCore.Identity;

namespace NMT.Models
{
    public class User : IdentityUser
    {
        public int Coins { get; set; }

        public ICollection<Inventory> Inventory { get; set; } = new List<Inventory>();
        public ICollection<Achievement> Achievements { get; set; } = new List<Achievement>();
        public CharacterState? CharacterState { get; set; } 
    }

}
