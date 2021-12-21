using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Models
{
    public class ProductType
    {

        [Key]
        public int ProductId { get; set; }

        [Required]
        [Display(Name ="Product Type")]
        public string Product { get; set; }
    }
}
