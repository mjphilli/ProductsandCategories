using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsandCategories.Models;

namespace ProductsandCategories.Controllers;

public class CategoryController : Controller
{
    private readonly ILogger<CategoryController> _logger;
    private MyContext db;

    public CategoryController(ILogger<CategoryController> logger, MyContext context)
    {
        _logger = logger;
        db = context;
    }

    [HttpGet("categories")]
    public IActionResult Categories()
    {
        List<Category> allCategories = db.Categories.ToList();
        return View("Category", allCategories);
    }

    [HttpPost("categories/create")]
    public IActionResult CreateCategory(Category newCategory)
    {
        if (!ModelState.IsValid)
        {
            return Categories();
        }

        db.Categories.Add(newCategory);

        db.SaveChanges();

        return RedirectToAction("Categories");
    }

    [HttpGet("categories/{id}")]
    public IActionResult ViewCategory(int id)
    {
        Category? category = db.Categories.Include(category => category.AllAssociations).ThenInclude(association => association.Product)
        .FirstOrDefault(category => category.CategoryId == id);

        ViewBag.missingproducts = db.Products.Include(product => product.AllAssociations)
        .Where(product => product.AllAssociations.All(association => association.CategoryId != id));

        if (category == null)
        {
            return RedirectToAction("Categories");
        }

        return View("ViewCategory", category);
    }

    [HttpPost("categories/{id}")]
    public IActionResult UpdateProducts(int id, int productId)
    {
        Association newAssociation = new Association()
        {
            ProductId = productId,
            CategoryId = id
        };

        db.Associations.Add(newAssociation);

        db.SaveChanges();

        return RedirectToAction("ViewCategory", new { id = id });
    }
}
