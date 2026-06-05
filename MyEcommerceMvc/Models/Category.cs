using System.ComponentModel.DataAnnotations;

namespace MyEcommerceMvc.Models;

public class Category
{
    [Key]
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Category name is required")]
    [StringLength(100, ErrorMessage = "Category name cannot exceed 100 characters")]
    [Display(Name = "Category Name")]
    public string Name { get; set; } = string.Empty;
    
    [Display(Name = "Description")]
    [DataType(DataType.MultilineText)]
    public string? Description { get; set; }
    
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    
    [Display(Name = "Total Products")]
    public int ProductCount => Products?.Count ?? 0;
}