using System.ComponentModel.DataAnnotations;

namespace MyEcommerceMvc.Models;

public class Product
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;
    
    public string? Description { get; set; }
    
    [Required]
    [Range(0, 999999.99)]
    public decimal Price { get; set; }
    
    public int StockQuantity { get; set; }
    
    // Foreign key for 1-to-many relationship with Category
    [Required]
    public int CategoryId { get; set; }
    
    // Navigation Properties
    public virtual Category? Category { get; set; }
    
    // Many-to-many relationship with Order
    //public virtual ICollection<OrderProduct> OrderProduct { get; set; } = new List<OrderProduct>();
}