using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Models
{
    public class SpecialTags
    {
        [Key]
        public int SpecialTagId { get; set; }

        [Required]
        [Display(Name = "Special Tag")]
        public string SpecialTag { get; set; }
    }
}
