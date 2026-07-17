using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FashionStore.Data;

namespace FashionStore.Controllers
{
    public class SearchController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SearchController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string query)
        {
            ViewData["Query"] = query;
            
            var products = new List<FashionStore.Models.Entities.Product>();
            if (!string.IsNullOrEmpty(query))
            {
                products = await _context.Products
                    .Where(p => p.Name.Contains(query) || p.Category.Contains(query))
                    .ToListAsync();
            }

            return View(products);
        }
    }
}
