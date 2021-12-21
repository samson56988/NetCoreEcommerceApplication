using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Product Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Price")]
        public decimal Price { get; set; }

       
        [Display(Name = "Image")]
        public string Image { get; set; }

        [Required]
        [Display(Name = "Product Color")]
        public string ProductColor { get; set; }

        [Required]
        [Display(Name = "Available")]
        public bool isAvailable { get; set; }

        [Required]
        [Display(Name = "Product Type")]
        public int  ProductTypeId { get; set; }


        public string ProductType { get; set; }

        
        [Required]
        [Display(Name = "Special Tag")]
        public int SpecialTypeId { get; set; }

        public string SpecialType { get; set; }

        


    }
}
