using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyEcommerceMvc.Data;
using MyEcommerceMvc.Models;

namespace MyEcommerceMvc.Controllers;

public class OrdersController : Controller
{
    private readonly ApplicationDbContext _context;
    
    public OrdersController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    // GET: Orders
    public async Task<IActionResult> Index()
    {
        var orders = await _context.Orders
            .Include(o => o.User)
            .ThenInclude(u => u.Profile)
            .Include(o => o.OrderProducts)
            .ThenInclude(op => op.Product)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
        return View(orders);
    }
    
    // GET: Orders/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();
        
        var order = await _context.Orders
            .Include(o => o.User)
            .ThenInclude(u => u.Profile)
            .Include(o => o.OrderProducts)
            .ThenInclude(op => op.Product)
            .FirstOrDefaultAsync(o => o.Id == id);
        
        if (order == null)
            return NotFound();
        
        return View(order);
    }
    
    // GET: Orders/Create
    public async Task<IActionResult> Create()
    {
        ViewBag.Users = await _context.Users.Include(u => u.Profile).ToListAsync();
        ViewBag.Products = await _context.Products.Where(p => p.StockQuantity > 0).ToListAsync();
        return View();
    }
    
    // POST: Orders/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(int userId, int[] productIds, int[] quantities)
    {
        if (userId <= 0 || productIds == null || productIds.Length == 0)
        {
            TempData["Error"] = "Please select a customer and at least one product";
            ViewBag.Users = await _context.Users.Include(u => u.Profile).ToListAsync();
            ViewBag.Products = await _context.Products.Where(p => p.StockQuantity > 0).ToListAsync();
            return View();
        }
        
        var order = new Order
        {
            UserId = userId,
            OrderDate = DateTime.UtcNow,
            Status = "Pending",
            TotalAmount = 0
        };
        
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        
        decimal totalAmount = 0;
        
        for (int i = 0; i < productIds.Length; i++)
        {
            var product = await _context.Products.FindAsync(productIds[i]);
            if (product != null && quantities[i] > 0 && product.StockQuantity >= quantities[i])
            {
                var orderProduct = new OrderProduct
                {
                    OrderId = order.Id,
                    ProductId = product.Id,
                    Quantity = quantities[i],
                    UnitPrice = product.Price
                };
                
                _context.OrderProducts.Add(orderProduct);
                totalAmount += product.Price * quantities[i];
                
                // Update stock
                product.StockQuantity -= quantities[i];
                _context.Update(product);
            }
        }
        
        order.TotalAmount = totalAmount;
        _context.Update(order);
        
        await _context.SaveChangesAsync();
        
        TempData["Success"] = $"Order #{order.Id} created successfully!";
        return RedirectToAction(nameof(Index));
    }
    
    // GET: Orders/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();
        
        var order = await _context.Orders
            .Include(o => o.OrderProducts)
            .FirstOrDefaultAsync(o => o.Id == id);
        
        if (order == null)
            return NotFound();
        
        ViewBag.Statuses = new[] { "Pending", "Processing", "Shipped", "Delivered", "Cancelled" };
        return View(order);
    }
    
    // POST: Orders/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, string status)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null)
            return NotFound();
        
        order.Status = status;
        _context.Update(order);
        await _context.SaveChangesAsync();
        
        TempData["Success"] = $"Order #{id} status updated to {status}!";
        return RedirectToAction(nameof(Index));
    }
    
    // GET: Orders/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();
        
        var order = await _context.Orders
            .Include(o => o.User)
            .Include(o => o.OrderProducts)
            .ThenInclude(op => op.Product)
            .FirstOrDefaultAsync(o => o.Id == id);
        
        if (order == null)
            return NotFound();
        
        return View(order);
    }
    
    // POST: Orders/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var order = await _context.Orders
            .Include(o => o.OrderProducts)
            .FirstOrDefaultAsync(o => o.Id == id);
        
        if (order != null)
        {
            // Restore stock quantities
            foreach (var orderProduct in order.OrderProducts)
            {
                var product = await _context.Products.FindAsync(orderProduct.ProductId);
                if (product != null)
                {
                    product.StockQuantity += orderProduct.Quantity;
                    _context.Update(product);
                }
            }
            
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Order #{id} deleted successfully!";
        }
        
        return RedirectToAction(nameof(Index));
    }
}