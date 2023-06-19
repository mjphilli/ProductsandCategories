using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsandCategories.Models;

namespace ProductsandCategories.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private MyContext db;

    public HomeController(ILogger<HomeController> logger, MyContext context)
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

    [HttpGet("categories")]
    public IActionResult Categories()
    {
        List<Category> allCategories = db.Categories.ToList();
        return View("Category", allCategories);
    }

    [HttpPost("products/create")]
    public IActionResult CreateProduct(Product newProduct)
    {
        if (!ModelState.IsValid)
        {
            // return View("Product");
            return RedirectToAction("Index"); //redirects to page with no errors
        }

        db.Products.Add(newProduct);

        db.SaveChanges();

        return RedirectToAction("Index");
    }

    // [HttpGet("dishes/new")]
    // public IActionResult NewCategory()
    // {
    //     return View("Category");
    // }

    [HttpPost("categories/create")]
    public IActionResult CreateCategory(Category newCategory)
    {
        if (!ModelState.IsValid)
        {
            // return View("Category");
            return RedirectToAction("Categories"); //redirects to page with no errors
        }

        db.Categories.Add(newCategory);

        db.SaveChanges();

        return RedirectToAction("Categories");
    }

    [HttpGet("products/{id}")]
    public IActionResult ViewProduct(int id)
    {
        Product? product = db.Products.Include(post => post.Name).Include(product => product.AllAssociations).ThenInclude(category => category.Category)
        .FirstOrDefault(product => product.ProductId == id);

        if (product == null)
        {
            return RedirectToAction("Index");
        }

        return View("ViewProduct", product);
    }

    [HttpGet("categories/{id}")]
    public IActionResult ViewCategory(int id)
    {
        Category? category = db.Categories.FirstOrDefault(category => category.CategoryId == id);

        if (category == null)
        {
            return RedirectToAction("Categories");
        }

        return View("ViewCategory", category);
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
