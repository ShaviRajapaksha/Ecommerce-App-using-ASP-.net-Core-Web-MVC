using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyEcommerceMvc.Data;

namespace MyEcommerceMvc.Controllers;

public class UsersController : Controller
{
    private readonly ApplicationDbContext _context;
    
    public UsersController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    // Get: Users
    public async Task<IActionResult> Index()
    {
        var users = await _context.Users
            .Include(u => u.Profile)
            .Include(u => u.Orders)
            .ToListAsync();
        return View(users);
    }
    
    
    
    
}