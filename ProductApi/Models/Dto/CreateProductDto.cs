using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Diagnostics;

namespace ProductApi.Models.Dto
{
    public class CreateProductDto
    {
        [Required(ErrorMessage = "El título del producto es obligatorio.")]
        [StringLength(100, ErrorMessage = "El número máximo de caracteres es de 100.")]
        public string Title { get; set; }

        [StringLength(500, ErrorMessage = "El número máximo de caracteres es de 500.")]
        public string Description { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        [Range(0.01, 10000.00, ErrorMessage = "El precio debe ser entre 0.01 y 10000.00")]
        [Precision(11, 2)]
        public decimal Price { get; set; }

        [Range(0.01, 100.00, ErrorMessage = "El porcentaje de descuento debe ser entre 0.01 y 100.00")]
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
