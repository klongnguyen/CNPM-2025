using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FashionStore.Models.Entities
{
    public class SaleItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        
        public int SaleId { get; set; }
        [ForeignKey("SaleId")]
        public Sale Sale { get; set; }

        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        public int Quantity { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal OriginalPrice { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountApplied { get; set; }
        
        public string DiscountPercent { get; set; }
        public int IsDiscounted { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal ItemTotal { get; set; }
        
        public DateTime SaleDate { get; set; }
        public string Channel { get; set; }
        public string ChannelCampaigns { get; set; }
    }
}
