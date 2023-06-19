#pragma warning disable CS8618
namespace ProductsandCategories.Models;
using System.ComponentModel.DataAnnotations;
public class Category
{
    [Key]
    public int CategoryId { get; set; }
    [Required]
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    // Our navigation property to track the many Posts our user has made
    // Be sure to include the part about instantiating a new List!
    // public List<Product> AllProducts { get; set; } = new List<Product>();
    public List<Association> AllAssociations { get; set; } = new List<Association>();
}

// public class Over18Attribute : ValidationAttribute
// {
//     protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
//     {
//         // Though we have Required as a validation, sometimes we make it here anyways
//         // In which case we must first verify the value is not null before we proceed
//         if (value == null)
//         {
//             // If it was, return the required error
//             return new ValidationResult("Must provide a date of birth!");
//         }

//         // This will connect us to our database since we are not in our Controller

//         // Check to see if there are any records of this email in our database
//         if (DateTime.Now.Date.Year - ((DateTime)value).Year < 18)
//         {
//             // If yes, throw an error
//             return new ValidationResult("User must be over 18!");
//         }
//         else
//         {
//             // If no, proceed
//             return ValidationResult.Success;
//         }
//     }
// }