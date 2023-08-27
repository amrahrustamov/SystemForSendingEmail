using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok.Contracts;
using Pustok.Database;
using Pustok.Database.Models;
using Pustok.Migrations;
using Pustok.Services.Abstracts;
using Pustok.ViewModels.Admin.Product;
using System.IO;

namespace Pustok.Controllers.Admin;

[Route("admin/products")]
public class ProductController : Controller
{
    private readonly PustokDbContext _dbContext;
    private readonly IFileService _fileService;

    public ProductController(PustokDbContext dbContext, IFileService fileService)
    {
        _dbContext = dbContext;
        _fileService = fileService;
    }

    #region Index

    [HttpGet]
    public IActionResult Index()
    {
        //LINQ
        //Collections

        var products = _dbContext.Products
            //.Include(p => p.Category)
            .OrderBy(p => p.Name)
            .ToList();

        var productViewModels = products
        .Select(p => new ProductListItemViewModel
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Color = p.Color,
            Size = p.Size,
            Price = p.Price,
            //CategoryName = p.Category?.Name
        })
        .ToList();

        var result = View("~/Views/Admin/Product/Index.cshtml", productViewModels);
        return result;
    }

    #endregion

    #region Add

    [HttpGet("add")]
    public IActionResult Add()
    {
        var model = new ProductAddViewModel
        {
            Categories = _dbContext.Categories.ToList(),
        };

        return View("~/Views/Admin/Product/Add.cshtml", model);
    }

    [HttpPost("add")]
    public IActionResult Add(ProductAddViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Categories = _dbContext.Categories.ToList();
            return View("~/Views/Admin/Product/Add.cshtml", model);
        }

        var uniqueFileName = _fileService.Upload(model.Image, CustomUploadDirectories.Products);

        var product = new Product(
            model.Name,
            model.Description,
            model.Color,
            model.Size,
            model.Price,
            /*model.CategoryId,*/ default,
            uniqueFileName);

        _dbContext.Products.Add(product);

        foreach (var categoryId in model.CategoryIds)
        {
            var category = _dbContext.Categories.SingleOrDefault(c => c.Id == categoryId);
            if (category == null)
            {
                model.Categories = _dbContext.Categories.ToList();
                ModelState.AddModelError("CategoryIds", "Category not found");
                return View("~/Views/Admin/Product/Add.cshtml", model);
            }

            var productCategory = new CategoryProduct
            {
                CategoryId = category.Id,
                Product = product,
            };

            _dbContext.CategoryProducts.Add(productCategory);
        }

        _dbContext.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    #endregion

    #region Update

    [HttpGet("{id}/update")]
    public IActionResult Update(int id)
    {
        var product = _dbContext.Products.FirstOrDefault(p => p.Id == id);
        if (product == null) return NotFound();

        var selectedCategoryIds = _dbContext.CategoryProducts
                .Where(cp => cp.ProductId == product.Id)
                .Select(cp => cp.CategoryId)
                .ToArray();

        var model = new ProductUpdateViewModel
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Color = product.Color,
            Size = product.Size,
            Price = product.Price,
            CategoryIds = selectedCategoryIds,
            Categories = _dbContext.Categories.ToList(),
            CurrentFileName = product.PhysicalImageName
        };

        return View("~/Views/Admin/Product/Update.cshtml", model);
    }

    [HttpPost("{id}/update")]
    public IActionResult Update(ProductUpdateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Categories = _dbContext.Categories.ToList();
            return View("~/Views/Admin/Product/Update.cshtml", model);
        }

        var product = _dbContext.Products.FirstOrDefault(p => p.Id == model.Id);
        if (product == null)
        {
            ModelState.AddModelError("Name", "Product not found");
            model.Categories = _dbContext.Categories.ToList();
            return View("~/Views/Admin/Product/Update.cshtml", model);
        }

        var previousFileName = product.PhysicalImageName;
        var path = "C:\\Users\\qarib\\Desktop\\Code academy\\Pustok\\Pustok\\Pustok\\wwwroot\\admin\\uploads\\images\\";

        if (model.Image != null)
        {
            var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(model.Image.FileName)}";
            var newFullPath = path + uniqueFileName;

            using var fileStream = new FileStream(newFullPath, FileMode.Create);
            model.Image.CopyTo(fileStream); //Upload

            product.PhysicalImageName = uniqueFileName;
        }

        product.Name = model.Name;
        product.Description = model.Description;
        product.Color = model.Color;
        product.Price = model.Price;

        _dbContext.Products.Update(product);

        if (model.Image != null && previousFileName != null)
        {
            var previousFullPath = path + previousFileName;
            System.IO.File.Delete(previousFullPath);
        }

        //records to remove
        var removeableProductCategories = _dbContext.CategoryProducts
            .Where(cp =>
                cp.ProductId == product.Id &&
                !model.CategoryIds.Contains(cp.CategoryId))
            .ToList();

        //records to remove
        var addableCategoryIds = model.CategoryIds
            .Where(id => !_dbContext.CategoryProducts
                            .Where(cp => cp.ProductId == product.Id)
                            .Any(cp => cp.CategoryId == id))
            .ToList();

        var addableProductCateories = addableCategoryIds.Select(id => new CategoryProduct
        {
            CategoryId = id,
            ProductId = product.Id,
        });

        _dbContext.CategoryProducts.RemoveRange(removeableProductCategories);
        _dbContext.CategoryProducts.AddRange(addableProductCateories);

        _dbContext.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    #endregion

    #region Delete

    [HttpPost("{id}/delete")]
    public IActionResult Delete(int id)
    {
        var product = _dbContext.Products.FirstOrDefault(p => p.Id == id);
        if (product == null) return NotFound();

        _dbContext.Remove(product);
        _dbContext.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    #endregion


    //Desctruct or finalizer
    // => dispose pattern
    ~ProductController()
    {
        _dbContext.Dispose();
    }
}