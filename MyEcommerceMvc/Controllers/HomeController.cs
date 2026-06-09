using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyEcommerceMvc.Data;

namespace MyEcommerceMvc.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;
    
    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<IActionResult> Index()
    {
        var totalProducts = await _context.Products.CountAsync();
        var totalOrders = await _context.Orders.CountAsync();
        var totalUsers = await _context.Users.CountAsync();
        
        var recentOrders = await _context.Orders
            .Include(o => o.User)
            .ThenInclude(u => u.Profile)
            .OrderByDescending(o => o.OrderDate)
            .Take(5)
            .ToListAsync();
        
        ViewBag.TotalProducts = totalProducts;
        ViewBag.TotalOrders = totalOrders;
        ViewBag.TotalUsers = totalUsers;
        
        // Pass the model to the view
        return View(recentOrders);
    }
    
    public IActionResult Privacy()
    {
        return View();
    }
    
    public IActionResult Error()
    {
        return View();
    }
}