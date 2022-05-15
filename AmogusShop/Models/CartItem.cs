namespace AmogusShop.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public AMOGUS Amogus { get; set; }
        public int CartId { get; set; }
        public int Amount { get; set; }
    }
}
