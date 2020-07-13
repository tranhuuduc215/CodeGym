using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HueSpa.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Image { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}
