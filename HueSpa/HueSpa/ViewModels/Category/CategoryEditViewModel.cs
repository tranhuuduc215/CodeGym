using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HueSpa.ViewModels
{
    public class CategoryEditViewModel
    {
        public int CategoryId { get; set; }
        public string ImagePath { get; set; }
        [Required]
        public string CategoryName { get; set; }
        public IFormFile Image { get; set; }
    }
}
