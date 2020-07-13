using HueSpa.Models;
using HueSpa.ViewModels;
using HueSpa.ViewModels.Home;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HueSpa.Controllers
{
    public class CategoryController:Controller
    {
        private readonly IProductRepository productRepository;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ICategoryRepository CategoryRepository;

        public CategoryController(IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IWebHostEnvironment webHostEnvironment)
        {
            this.productRepository = productRepository;
            CategoryRepository = categoryRepository;
            this.webHostEnvironment = webHostEnvironment;
        }
        [HttpGet]
        public ViewResult Create()
        {
            ViewBag.Categories = CategoryRepository.Get().ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(CategoryCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var category = new Category()
                {
                    CategoryName = model.CategoryName
                };
                var result = (from c in CategoryRepository.Get() 
                              where c.CategoryName.ToLower().Equals(category.CategoryName.ToLower()) select c).FirstOrDefault();
                if (result == null)
                {
                    var fileName = string.Empty;
                    if (model.Image != null)
                    {
                        string uploadFolder = Path.Combine(webHostEnvironment.WebRootPath, "images/category");
                        fileName = $"{Guid.NewGuid()}_{model.Image.FileName}";
                        var filePath = Path.Combine(uploadFolder, fileName);
                        using (var fs = new FileStream(filePath, FileMode.Create))
                        {
                            model.Image.CopyTo(fs);
                        }
                    }
                    category.Image = fileName;
                    var newCtgr = CategoryRepository.Create(category);
                    return RedirectToAction("Dashboard", new { id = newCtgr.CategoryId });
                }
                else
                {
                    ModelState.AddModelError("", "Category name already exists. Please enter another name !");
                }
            }
            return View();
        }

        [HttpGet]
        public ViewResult Edit(int id)
        {
            var category = CategoryRepository.Get(id);
            if (category == null)
            {
                return View("~/Views/Error/ProductNotFound.cshtml", id);
            }
            var ctgrdEdit = new CategoryEditViewModel()
            {
                ImagePath = category.Image,
                CategoryName = category.CategoryName,
                CategoryId = category.CategoryId,
            };
            ViewBag.Categories = CategoryRepository.Get().ToList();
            return View(ctgrdEdit);
        }

        [HttpPost]
        public IActionResult Edit(CategoryEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var category = new Category()
                {
                    Image = model.ImagePath,
                    CategoryName = model.CategoryName,
                    CategoryId = model.CategoryId,
                };
                    var fileName = string.Empty;
                    if (model.Image != null)
                    {
                        string uploadFolder = Path.Combine(webHostEnvironment.WebRootPath, "images/Category");
                        fileName = $"{Guid.NewGuid()}_{model.Image.FileName}";
                        var filePath = Path.Combine(uploadFolder, fileName);
                        using (var fs = new FileStream(filePath, FileMode.Create))
                        {
                            model.Image.CopyTo(fs);
                        }
                        category.Image = fileName;
                        if (!string.IsNullOrEmpty(model.ImagePath))
                        {
                            string delFile = Path.Combine(webHostEnvironment.WebRootPath, "images/Category", model.ImagePath);
                            System.IO.File.Delete(delFile);
                        }
                    }
                    var editCtgr = CategoryRepository.Edit(category);
                    if (editCtgr != null)
                    {
                        return RedirectToAction("Dashboard", new { id = category.CategoryId });
                    }
            }
            return View(model);
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            ViewBag.Categories = GetCategories();
            var model = new ViewModel()
            {
                Products = productRepository.Gets().Take(8)
            };
            return View(model);
        }

         [AllowAnonymous]
        public IActionResult Products(int id)
        {
            var products = (from p in productRepository.Gets() where p.CategoryId == id select p).ToList();
            var model = new ViewModel()
            {
                Products = products,
            };
            return View(model);
        }
        public List<Category> GetCategories()
        {
            return CategoryRepository.Get().ToList();
        }

        public IActionResult Delete(int id)
        {
            if (CategoryRepository.Delete(id))
            {
                return RedirectToAction("Dashboard");
            }
            return View();
        }
        public IActionResult Dashboard()
        {
            ViewBag.Categories = GetCategories();
            return View(CategoryRepository.Get());
        }

        [AllowAnonymous]
        public IActionResult Search(ViewModel model)
        {
            List<Product> products = (from pd in productRepository.Gets() 
                                      where pd.ProdName.ToLower().Contains(model.KeySearch.ToLower()) select pd).ToList();
            var search = new ViewModel()
            {
                Products = products
            };
            return View(search);
        }
    }
}
