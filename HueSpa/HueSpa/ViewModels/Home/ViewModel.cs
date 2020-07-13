using HueSpa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HueSpa.ViewModels.Home
{
    public class ViewModel
    {
        public string KeySearch { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public Product Product { get; set; }
        public string TitleName { get; set; }
    }
}
