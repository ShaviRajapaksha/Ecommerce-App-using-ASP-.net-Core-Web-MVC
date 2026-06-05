using System.ComponentModel.DataAnnotations;

namespace MyEcommerceMvc.Models;

public class User
{
    [Key]
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Username is required")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 100 characters")]
    [Display(Name = "Username")]
    public string Username { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    [Display(Name = "Email Address")]
    public string Email { get; set; } = string.Empty;
    
    [Display(Name = "Created At")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // 1-to-1 relationship with Profile
    public virtual Profile? Profile { get; set; }
    
    // 1-to-many relationship with Order
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}