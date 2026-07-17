using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FashionStore.Models.Entities
{
    public class Stock
    {
        [Key]
        public int Id { get; set; }

        public string Country { get; set; }
        
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        public int StockQuantity { get; set; }
    }
}
