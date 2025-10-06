using ECommerce.Core.Entities;

namespace ECommerce.Core.DTOs
{
    public class CartHistory
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public List<CartItem> Items { get; set; }
        public DateTime CheckedOutAt { get; set; }
    }
}
