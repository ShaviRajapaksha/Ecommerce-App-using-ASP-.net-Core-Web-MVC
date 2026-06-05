using System.ComponentModel.DataAnnotations;

namespace MyEcommerceMvc.Models.ViewModels;

public class UserProfileViewModel
{
    public int UserId { get; set; }
    
    [Required]
    [Display(Name = "Username")]
    public string Username { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;
    
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;
    
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = string.Empty;
    
    [Phone]
    [Display(Name = "Phone")]
    public string? Phone { get; set; }
    
    [Display(Name = "Address")]
    public string? Address { get; set; }
}