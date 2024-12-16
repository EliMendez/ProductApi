using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductApi.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }
        public string Description { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        [Precision(11, 2)]
        public decimal Price { get; set; }

        [Precision(8, 2)]
        public decimal DiscountPercentage { get; set; }

        [Required]
        public int Stock { get; set; }
        public enum ProductStatus { Available, OutOfStock, Discontinued }

        [Required]
        public ProductStatus Status { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime UpdatedDate { get; set; }

        //Category relationship
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

    }
}
