namespace NMT.Models
{
    public class CharacterState
    {
        public int Id { get; set; }

        public string UserId { get; set; } = null!;
        public User? User { get; set; }

        public byte[]? Hat { get; set; }
        public byte[]? Outfit { get; set; }
        public byte[]? Background { get; set; }
        public byte[]? Accessory { get; set; }
    }

}
