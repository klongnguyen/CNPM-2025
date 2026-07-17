using Microsoft.AspNetCore.Mvc;
using FashionStore.Models;
using FashionStore.Extensions;
using FashionStore.Data;
using FashionStore.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace FashionStore.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("Cart") ?? new List<CartItem>();
            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity, string size, string color)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) return NotFound();

            var cart = HttpContext.Session.Get<List<CartItem>>("Cart") ?? new List<CartItem>();
            var existingItem = cart.FirstOrDefault(c => c.ProductId == productId && c.Size == size && c.Color == color);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Price = product.CatalogPrice,
                    ImageUrl = product.ImageUrl,
                    Quantity = quantity,
                    Size = size,
                    Color = color
                });
            }

            HttpContext.Session.Set("Cart", cart);
            return RedirectToAction("Index");
        }

        public IActionResult RemoveFromCart(int productId, string size, string color)
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("Cart") ?? new List<CartItem>();
            var item = cart.FirstOrDefault(c => c.ProductId == productId && c.Size == size && c.Color == color);
            if (item != null)
            {
                cart.Remove(item);
                HttpContext.Session.Set("Cart", cart);
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Checkout()
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("Cart");
            if (cart == null || !cart.Any()) return RedirectToAction("Index");

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim)) return RedirectToAction("Login", "Account");

            int customerId = int.Parse(userIdClaim);

            var maxSaleId = await _context.Sales.MaxAsync(s => (int?)s.Id) ?? 0;
            var newSaleId = maxSaleId + 1;

            var sale = new Sale
            {
                Id = newSaleId,
                Channel = "Web",
                IsDiscounted = 0,
                TotalAmount = cart.Sum(c => c.Total),
                SaleDate = DateTime.Now,
                CustomerId = customerId,
                Country = "VN"
            };

            _context.Sales.Add(sale);
            await _context.SaveChangesAsync();

            var maxSaleItemId = await _context.SaleItems.MaxAsync(si => (int?)si.Id) ?? 0;
            int currentSaleItemId = maxSaleItemId + 1;

            foreach (var item in cart)
            {
                var saleItem = new SaleItem
                {
                    Id = currentSaleItemId++,
                    Channel = "Web",
                    ChannelCampaigns = "",
                    DiscountApplied = 0,
                    DiscountPercent = "0",
                    IsDiscounted = 0,
                    ItemTotal = item.Total,
                    OriginalPrice = item.Price,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    SaleDate = DateTime.Now,
                    SaleId = newSaleId,
                    UnitPrice = item.Price
                };
                _context.SaleItems.Add(saleItem);
            }

            await _context.SaveChangesAsync();

            // Clear cart
            HttpContext.Session.Remove("Cart");

            return RedirectToAction("CheckoutSuccess");
        }

        public IActionResult CheckoutSuccess()
        {
            return View();
        }
    }
}
