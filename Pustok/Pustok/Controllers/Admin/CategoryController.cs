using Microsoft.AspNetCore.Mvc;
using Pustok.Database;
using Pustok.Database.Models;
using Pustok.Services.Abstracts;
using Pustok.ViewModels.Admin.Category; // Make sure you have the appropriate namespace for Category-related view models

namespace Pustok.Controllers.Admin;

[Route("admin/categories")]
public class CategoryController : Controller
{
    private readonly PustokDbContext _dbContext;
    private readonly IFileService _fileService;

    public CategoryController(PustokDbContext dbContext, IFileService fileService)
    {
        _dbContext = dbContext;
        _fileService = fileService;
    }

    #region Index

    [HttpGet]
    public IActionResult Index()
    {
        var categories = _dbContext.Categories.ToList();
        var categoryViewModels = categories
            .Select(c => new CategoryListItemViewModel
            {
                Id = c.Id,
                Name = c.Name
            })
            .ToList();

        var result = View("~/Views/Admin/Category/Index.cshtml", categoryViewModels);
        return result;
    }

    #endregion

    #region Add

    [HttpGet("add")]
    public IActionResult Add()
    {
        return View("~/Views/Admin/Category/Add.cshtml");
    }

    [HttpPost("add")]
    public IActionResult Add(CategoryAddViewModel model)
    {
        if (!ModelState.IsValid) 
            return View("~/Views/Admin/Category/Add.cshtml");

        var category = new Category
        {
            Name = model.Name
        };

        _dbContext.Categories.Add(category);
        _dbContext.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    #endregion

    #region Update

    [HttpGet("{id}/update")]
    public IActionResult Update(int id)
    {
        var category = _dbContext.Categories.FirstOrDefault(c => c.Id == id);
        if (category == null) return NotFound();

        var model = new CategoryUpdateViewModel
        {
            Id = category.Id,
            Name = category.Name
        };

        return View("~/Views/Admin/Category/Update.cshtml", model);
    }

    [HttpPost("{id}/update")]
    public IActionResult Update(CategoryUpdateViewModel model)
    {
        if (!ModelState.IsValid)
            return View("~/Views/Admin/Category/Update.cshtml");

        var category = _dbContext.Categories.FirstOrDefault(c => c.Id == model.Id);
        if (category == null)
        {
            ModelState.AddModelError("Name", "Category not found");
            return View("~/Views/Admin/Category/Update.cshtml");
        }

        category.Name = model.Name;

        _dbContext.Categories.Update(category);
        _dbContext.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    #endregion

    #region Delete

    [HttpPost("{id}/delete")]
    public IActionResult Delete(int id)
    {
        var category = _dbContext.Categories.FirstOrDefault(c => c.Id == id);
        if (category == null) return NotFound();

        _dbContext.Categories.Remove(category);
        _dbContext.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    #endregion

    // Dispose pattern
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _dbContext.Dispose();
        }
        base.Dispose(disposing);
    }
}
