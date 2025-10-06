
namespace ECommerce.Core.DTOs
{
    using Entities;

    public class SavedCart
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public List<CartItem> Items { get; set; }
        public DateTime SavedAt { get; set; }
    }
}
