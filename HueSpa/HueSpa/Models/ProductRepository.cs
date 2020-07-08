using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HueSpa.Models
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ProductRepository(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            this.context = context;
            this.webHostEnvironment = webHostEnvironment;
        }

        public Product Create(Product product)
        {
            context.Products.Add(product);
            context.SaveChanges();
            return product;
        }

        public bool Delete(int id)
        {
             var delPrd = Get(id);
            if (delPrd != null)
            {
                string delFile = Path.Combine(webHostEnvironment.WebRootPath, "images", delPrd.ImagePath);
                System.IO.File.Delete(delFile);
                context.Products.Remove(delPrd);
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public Product Edit(Product product)
        {
            context.Products.Update(product);
            context.SaveChanges();
            return product;
        }

        public Product Get(int id)
        {
            return context.Products.Find(id);
        }

        public IEnumerable<Product> Gets()
        {
            return context.Products;
        }


    }
}
