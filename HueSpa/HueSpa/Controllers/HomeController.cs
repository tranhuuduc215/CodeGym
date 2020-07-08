using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HueSpa.Models;
using HueSpa.ViewModels;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace HueSpa.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductRepository productRepository;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ICategoryRepository CategoryRepository { get; }

        public HomeController(IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IWebHostEnvironment webHostEnvironment)
        {
            this.productRepository = productRepository;
            CategoryRepository = categoryRepository;
            this.webHostEnvironment = webHostEnvironment;
        }
        [AllowAnonymous]
        public ViewResult Index()
        {
            ViewBag.Categories = GetCategories();
            return View(productRepository.Gets());
        }
        [AllowAnonymous]
        public ViewResult Details(int? id)
        {
            try
            {
                int.Parse(id.Value.ToString());
                var product = productRepository.Get(id.Value);
                if (product == null)
                {
                    return View("~/Views/Error/ProductNotFound.cshtml", id.Value);
                }
                var detailViewModel = new HomeDetailViewModel()
                {
                    Product = productRepository.Get(id ?? 1),
                    TitleName = "Product Detail"
                };
                return View(detailViewModel);
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        [HttpGet]
        public ViewResult Create()
        {
            ViewBag.Categories = GetCategories().ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(HomeCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var product = new Product()
                {
                    ProdName = model.ProdName,
                    Price = model.Price,
                    ProdInformation = model.ProdInformation,
                    CategoryId=model.CategoryId
                };
                var fileName = string.Empty;
                if (model.Images != null)
                {
                    string uploadFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                    fileName = $"{Guid.NewGuid()}_{model.Images.FileName}";
                    var filePath = Path.Combine(uploadFolder, fileName);
                    using (var fs = new FileStream(filePath, FileMode.Create))
                    {
                        model.Images.CopyTo(fs);
                    }
                }
                product.ImagePath = fileName;
                var newPrd = productRepository.Create(product);
                return RedirectToAction("Details", new { id = newPrd.Id });
            }
            return View();
        }
        [HttpGet]
        public ViewResult Edit(int id)
        {
            var product = productRepository.Get(id);
            if (product == null)
            {
                return View("~/Views/Error/ProductNotFound.cshtml", id);
            }
            var prdEdit = new HomeEditViewModel()
            {
                ImagePath = product.ImagePath,
                ProdName = product.ProdName,
                Price = product.Price,
                ProdInformation = product.ProdInformation,
                Id = product.Id,
                CategoryId=product.CategoryId
            };
            ViewBag.Categories = CategoryRepository.Get().ToList();
            return View(prdEdit);
        }

        [HttpPost]
        public IActionResult Edit(HomeEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var product = new Product()
                {
                    ImagePath = model.ImagePath,
                    ProdName = model.ProdName,
                    Price = model.Price,
                    ProdInformation = model.ProdInformation,
                    Id = model.Id,
                    CategoryId=model.CategoryId
                };
                var fileName = string.Empty;
                if (model.Images != null)
                {
                    string uploadFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                    fileName = $"{Guid.NewGuid()}_{model.Images.FileName}";
                    var filePath = Path.Combine(uploadFolder, fileName);
                    using (var fs = new FileStream(filePath, FileMode.Create))
                    {
                        model.Images.CopyTo(fs);
                    }
                    product.ImagePath = fileName;
                    if (!string.IsNullOrEmpty(model.ImagePath))
                    {
                        string delFile = Path.Combine(webHostEnvironment.WebRootPath, "images", model.ImagePath);
                        System.IO.File.Delete(delFile);
                    }
                }
                var editEmp = productRepository.Edit(product);
                if (editEmp != null)
                {
                    return RedirectToAction("Details", new { id = product.Id });
                }
            }
            return View(model);
        }
        public IActionResult Delete(int id)
        {
            var product = productRepository.Get(id);
            if (productRepository.Delete(id))
            {
                return RedirectToActionPermanentPreserveMethod("Products","Category",new { id=product.Id});
            }
            return View();
        }

        public List<Category> GetCategories()
        {
            return CategoryRepository.Get().ToList();
        }
    }
}
