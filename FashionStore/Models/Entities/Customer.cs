using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FashionStore.Models.Entities
{
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] // Id comes from CSV
        public int Id { get; set; }
        
        public string Country { get; set; }
        public string AgeRange { get; set; }
        public DateTime SignupDate { get; set; }

        // Extra fields added for login functionality
        [Required]
        public string FullName { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        public string Password { get; set; } // Plain text as requested by user
        
        public string Role { get; set; } = "Customer"; // Customer or Admin

        // Navigation
        public ICollection<Sale> Sales { get; set; } = new List<Sale>();
    }
}
