using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HueSpa.Models
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> Get();
        Category Get(int id);
        Category Create(Category category);
        Category Edit(Category category);
        bool Delete(int id);
    }
}
