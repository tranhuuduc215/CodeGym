using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HueSpa.Models
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext context;

        public CategoryRepository(AppDbContext context,
            IWebHostEnvironment webHostEnvironment)
        {
            this.context = context;
            WebHostEnvironment = webHostEnvironment;
        }

        public IWebHostEnvironment WebHostEnvironment { get; }

        public Category Create(Category category)
        {
            context.Categories.Add(category);
            context.SaveChanges();
            return category;
        }

        public bool Delete(int id)
        {
            var delCtgr = Get(id);
            if (delCtgr != null)
            {
                string delFile = Path.Combine(WebHostEnvironment.WebRootPath, "images/Category", delCtgr.Image);
                System.IO.File.Delete(delFile);
                context.Categories.Remove(delCtgr);
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public Category Edit(Category category)
        {
            context.Categories.Update(category);
            context.SaveChanges();
            return category;
        }

        public IEnumerable<Category> Get()
        {
            return context.Categories;
        }

        public Category Get(int id)
        {
            return context.Categories.Find(id);
        }
    }
}
