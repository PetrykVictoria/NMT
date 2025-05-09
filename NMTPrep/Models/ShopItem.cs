namespace NMT.Models
{
    public class ShopItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Type { get; set; } = null!;        
        public int Cost { get; set; }            
        public byte[]? ImagePath { get; set; }    

        public ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();
    }

}
