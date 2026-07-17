using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace FashionStore.Models.Entities
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] // Id comes from CSV
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        public string Category { get; set; }
        public string Brand { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal CatalogPrice { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal CostPrice { get; set; }
        
        public string Gender { get; set; }

        public string? ImageUrl { get; set; }
        public string? Description { get; set; }

        // Navigation
        public ICollection<Stock> Stocks { get; set; } = new List<Stock>();
        public ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
    }
}
