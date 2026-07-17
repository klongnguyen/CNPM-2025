using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using FashionStore.Data;
using FashionStore.Models.Entities;

namespace FashionStore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var totalSales = await _context.Sales.SumAsync(s => s.TotalAmount);
            var orderCount = await _context.Sales.CountAsync();
            var customerCount = await _context.Customers.CountAsync();
            
            ViewData["TotalSales"] = totalSales;
            ViewData["OrderCount"] = orderCount;
            ViewData["CustomerCount"] = customerCount;

            return View();
        }

        public async Task<IActionResult> Products()
        {
            var products = await _context.Products.ToListAsync();
            return View(products);
        }

        [HttpGet]
        public IActionResult CreateProduct()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                var maxId = await _context.Products.MaxAsync(p => (int?)p.Id) ?? 0;
                product.Id = maxId + 1;
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction("Products");
            }
            return View(product);
        }

        public async Task<IActionResult> Orders()
        {
            var orders = await _context.Sales
                .Include(s => s.Customer)
                .OrderByDescending(s => s.SaleDate)
                .ToListAsync();
            return View(orders);
        }
    }
}
