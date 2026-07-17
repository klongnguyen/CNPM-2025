using System.ComponentModel.DataAnnotations;

namespace FashionStore.Models.Entities
{
    public class Channel
    {
        [Key]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
