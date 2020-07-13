using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HueSpa.ViewModels
{
    public class HomeCreateViewModel
    {
        [Required]
        [MaxLength(100)]
        [Display(Name = "Name Product")]
        public string ProdName { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        [Display(Name ="Information")]
        public string ProdInformation { get; set; }
        public IFormFile Images { get; set; }
        [Required]
        public int CategoryId { get; set; }

    }
}
