using System.ComponentModel.DataAnnotations;

namespace MyEcommerceMvc.Models;

public class Profile
{
    [Key]
    public int Id { get; set; }
    
    [StringLength(50)]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;
    
    [StringLength(50)]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = string.Empty;
    
    [StringLength(15)]
    [Display(Name = "Phone Number")]
    [Phone(ErrorMessage = "Invalid phone number")]
    public string? Phone { get; set; }
    
    [Display(Name = "Address")]
    public string? Address { get; set; }
    
    [Required]
    public int UserId { get; set; }
    
    public virtual User User { get; set; } = null!;
    
    [Display(Name = "Full Name")]
    public string FullName => $"{FirstName} {LastName}".Trim();
}