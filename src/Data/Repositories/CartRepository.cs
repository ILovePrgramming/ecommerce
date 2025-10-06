namespace ECommerce.Data.Repositories
{
    using Context;
    using Core.Entities;
    using ECommerce.Core.DTOs;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Provides methods for managing shopping cart items in the database for a specific user.
    /// </summary>
    public class CartRepository
    {
        private readonly AppDbContext _context;

        public CartRepository(AppDbContext context) => _context = context;

        public IEnumerable<CartItem> GetCart(string userId) =>
            _context.CartItems.Where(c => c.UserId == userId).ToList();

        public void Add(CartItem item) => _context.CartItems.Add(item);

        public void UpdateQuantity(string userId, int productId, int newQuantity)
        {
            var item = _context.CartItems.FirstOrDefault(c => c.UserId == userId && c.ProductId == productId);
            if (item != null)
            {
                item.Quantity = newQuantity;
            }
        }


        public void Remove(string userId, int productId)
        {
            var item = _context.CartItems.FirstOrDefault(c => c.UserId == userId && c.ProductId == productId);
            if (item != null) _context.CartItems.Remove(item);
        }


        public void BulkRemove(string userId, IEnumerable<int> productIds)
        {
            var items = _context.CartItems.Where(c => c.UserId == userId && productIds.Contains(c.ProductId));
            _context.CartItems.RemoveRange(items);
        }


        public void Clear(string userId)
        {
            var items = _context.CartItems.Where(c => c.UserId == userId);
            _context.CartItems.RemoveRange(items);
        }


        public void Save() => _context.SaveChanges();


        public bool ValidateStock(string userId)
        {
            var items = GetCart(userId);
            foreach (var item in items)
            {
                var product = _context.Products.Find(item.ProductId);
                if (product == null || product.Stock < item.Quantity)
                    return false;
            }
            return true;
        }

        public void ClearExpiredItems(string userId, TimeSpan expiry)
        {
            var now = DateTime.UtcNow;
            var items = _context.CartItems
                .Where(c => c.UserId == userId && now - c.AddedAt > expiry)
                .ToList();
            _context.CartItems.RemoveRange(items);
        }


        public void SaveCartForLater(string userId)
        {
            var items = GetCart(userId).ToList();
            var savedCart = new SavedCart
            {
                UserId = userId,
                Items = items,
                SavedAt = DateTime.UtcNow
            };
            _context.Set<SavedCart>().Add(savedCart);
        }

        public IEnumerable<CartItem> GetSavedCart(string userId)
        {
            var savedCart = _context.Set<SavedCart>().FirstOrDefault(c => c.UserId == userId);
            return savedCart?.Items ?? Enumerable.Empty<CartItem>();
        }


        public void StoreCartHistory(string userId)
        {
            var items = GetCart(userId).ToList();
            var history = new CartHistory
            {
                UserId = userId,
                Items = items,
                CheckedOutAt = DateTime.UtcNow
            };
            _context.Set<CartHistory>().Add(history);
        }


        public void MergeGuestCart(string guestId, string userId)
        {
            var guestItems = GetCart(guestId);
            foreach (var item in guestItems)
            {
                var existing = _context.CartItems.FirstOrDefault(c => c.UserId == userId && c.ProductId == item.ProductId);
                if (existing != null)
                    existing.Quantity += item.Quantity;
                else
                    _context.CartItems.Add(new CartItem { UserId = userId, ProductId = item.ProductId, Quantity = item.Quantity });
            }
            Clear(guestId);
        }


        public bool ValidateCart(string userId)
        {
            var items = GetCart(userId);
            foreach (var item in items)
            {
                var product = _context.Products.Find(item.ProductId);
                if (product == null || product.Stock < item.Quantity /* || product.IsDiscontinued */)
                    return false;
            }
            return true;
        }

        public CartSummaryDto GetCartSummary(string userId)
        {
            var items = GetCart(userId);
            decimal subtotal = items.Sum(i => _context.Products.Find(i.ProductId)?.Price ?? 0 * i.Quantity);
            decimal tax = subtotal * 0.1m; // Example: 10% tax
            decimal shipping = 5.0m; // Example: flat shipping
            decimal total = subtotal + tax + shipping;
            return new CartSummaryDto { Subtotal = subtotal, Tax = tax, Shipping = shipping, Total = total };
        }


        public IEnumerable<Product> GetRecommendations(string userId)
        {
            var items = GetCart(userId);
            // Example: recommend products not in cart
            var productIds = items.Select(i => i.ProductId).ToList();
            return _context.Products.Where(p => !productIds.Contains(p.Id)).Take(5).ToList();
        }
    }
}
