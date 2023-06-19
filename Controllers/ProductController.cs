using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsandCategories.Models;

namespace ProductsandCategories.Controllers;

public class ProductController : Controller
{
    private readonly ILogger<ProductController> _logger;
    private MyContext db;

    public ProductController(ILogger<ProductController> logger, MyContext context)
    {
        _logger = logger;
        db = context;
    }

    [HttpGet("")]
    public IActionResult Index()
    {
        List<Product> allProducts = db.Products.ToList();
        return View("Product", allProducts);
    }

    [HttpPost("products/create")]
    public IActionResult CreateProduct(Product newProduct)
    {
        if (!ModelState.IsValid)
        {
            return Index();
        }

        db.Products.Add(newProduct);

        db.SaveChanges();

        return RedirectToAction("Index");
    }

    [HttpGet("products/{id}")]
    public IActionResult ViewProduct(int id)
    {
        Product? product = db.Products.Include(product => product.AllAssociations).ThenInclude(association => association.Category)
        .FirstOrDefault(product => product.ProductId == id);

        ViewBag.missingcategories = db.Categories.Include(category => category.AllAssociations)
        .Where(category => category.AllAssociations.All(association => association.ProductId != id));

        if (product == null)
        {
            return RedirectToAction("Index");
        }

        return View("ViewProduct", product);
    }

    [HttpPost("products/{id}")]
    public IActionResult UpdateCategories(int id, int categoryId)
    {
        Association newAssociation = new Association()
        {
            ProductId = id,
            CategoryId = categoryId
        };

        db.Associations.Add(newAssociation);

        db.SaveChanges();

        // return RedirectToAction("ViewProduct", id);
        return ViewProduct(id);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
