using System.ComponentModel.DataAnnotations;

namespace MyEcommerceMvc.Models;

public class Order
{
    [Key]
    public int Id { get; set; }
    
    [Display(Name = "Order Date")]
    [DataType(DataType.DateTime)]
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    
    [Required]
    [StringLength(50)]
    [Display(Name = "Order Status")]
    public string Status { get; set; } = "Pending";
    
    [Display(Name = "Total Amount")]
    [DataType(DataType.Currency)]
    public decimal TotalAmount { get; set; }
    
    [Required]
    [Display(Name = "Customer")]
    public int UserId { get; set; }
    
    public virtual User User { get; set; } = null!;
    
    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
    
    [Display(Name = "Number of Items")]
    public int ItemCount => OrderProducts?.Sum(op => op.Quantity) ?? 0;
    
    public string StatusBadgeClass => Status switch
    {
        "Pending" => "warning",
        "Processing" => "info",
        "Shipped" => "primary",
        "Delivered" => "success",
        "Cancelled" => "danger",
        _ => "secondary"
    };
}