using System.ComponentModel.DataAnnotations;

namespace MyEcommerceMvc.Models;

public class OrderProduct
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
    public int Quantity { get; set; }
    
    [DataType(DataType.Currency)]
    public decimal UnitPrice { get; set; }
    
    public virtual Order Order { get; set; } = null!;
    public virtual Product Product { get; set; } = null!;
    
    [Display(Name = "Subtotal")]
    public decimal Subtotal => Quantity * UnitPrice;
}