using System.ComponentModel.DataAnnotations;

namespace MyEcommerceMvc.Models;

public class Product
{
    [Key]
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Product name is required")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "Product name must be between 3 and 200 characters")]
    [Display(Name = "Product Name")]
    public string Name { get; set; } = string.Empty;
    
    [Display(Name = "Description")]
    [DataType(DataType.MultilineText)]
    public string? Description { get; set; }
    
    [Required(ErrorMessage = "Price is required")]
    [Range(0.01, 999999.99, ErrorMessage = "Price must be between 0.01 and 999,999.99")]
    [DataType(DataType.Currency)]
    [Display(Name = "Price")]
    public decimal Price { get; set; }
    
    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Stock quantity cannot be negative")]
    [Display(Name = "Stock Quantity")]
    public int StockQuantity { get; set; }
    
    [Required]
    [Display(Name = "Category")]
    public int CategoryId { get; set; }
    
    public virtual Category Category { get; set; } = null!;
    
    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
    
    [Display(Name = "In Stock")]
    public bool IsInStock => StockQuantity > 0;
    
    [Display(Name = "Stock Status")]
    public string StockStatus => StockQuantity > 0 ? $"In Stock ({StockQuantity})" : "Out of Stock";
}