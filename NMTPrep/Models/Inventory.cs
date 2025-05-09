namespace NMT.Models
{
    public class Inventory
    {
        public int Id { get; set; }

        public string UserId { get; set; } = null!;
        public User? User { get; set; }

        public int ShopItemId { get; set; }          
        public ShopItem? ShopItem { get; set; }
    }

}
