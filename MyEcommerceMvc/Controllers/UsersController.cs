using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyEcommerceMvc.Data;
using MyEcommerceMvc.Models;
using MyEcommerceMvc.Models.ViewModels;

namespace MyEcommerceMvc.Controllers;

public class UsersController : Controller
{
    private readonly ApplicationDbContext _context;
    
    public UsersController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    // GET: Users
    public async Task<IActionResult> Index()
    {
        var users = await _context.Users
            .Include(u => u.Profile)
            .Include(u => u.Orders)
            .ToListAsync();
        return View(users);
    }
    
    // GET: Users/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();
        
        var user = await _context.Users
            .Include(u => u.Profile)
            .Include(u => u.Orders)
            .ThenInclude(o => o.OrderProducts)
            .ThenInclude(op => op.Product)
            .FirstOrDefaultAsync(u => u.Id == id);
        
        if (user == null)
            return NotFound();
        
        return View(user);
    }
    
    // GET: Users/Create
    public IActionResult Create()
    {
        return View();
    }
    
    // POST: Users/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(UserProfileViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new User
            {
                Username = model.Username,
                Email = model.Email,
                CreatedAt = DateTime.UtcNow
            };
            
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            
            var profile = new Profile
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Phone = model.Phone,
                Address = model.Address,
                UserId = user.Id
            };
            
            _context.Profiles.Add(profile);
            await _context.SaveChangesAsync();
            
            TempData["Success"] = "User created successfully!";
            return RedirectToAction(nameof(Index));
        }
        
        return View(model);
    }
    
    // GET: Users/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();
        
        var user = await _context.Users
            .Include(u => u.Profile)
            .FirstOrDefaultAsync(u => u.Id == id);
        
        if (user == null)
            return NotFound();
        
        var model = new UserProfileViewModel
        {
            UserId = user.Id,
            Username = user.Username,
            Email = user.Email,
            FirstName = user.Profile?.FirstName ?? "",
            LastName = user.Profile?.LastName ?? "",
            Phone = user.Profile?.Phone,
            Address = user.Profile?.Address
        };
        
        return View(model);
    }
    
    // POST: Users/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UserProfileViewModel model)
    {
        if (id != model.UserId)
            return NotFound();
        
        if (ModelState.IsValid)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.Profile)
                    .FirstOrDefaultAsync(u => u.Id == id);
                
                if (user == null)
                    return NotFound();
                
                user.Username = model.Username;
                user.Email = model.Email;
                
                if (user.Profile != null)
                {
                    user.Profile.FirstName = model.FirstName;
                    user.Profile.LastName = model.LastName;
                    user.Profile.Phone = model.Phone;
                    user.Profile.Address = model.Address;
                }
                else
                {
                    var profile = new Profile
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Phone = model.Phone,
                        Address = model.Address,
                        UserId = user.Id
                    };
                    _context.Profiles.Add(profile);
                }
                
                await _context.SaveChangesAsync();
                TempData["Success"] = "User updated successfully!";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(model.UserId))
                    return NotFound();
                throw;
            }
            
            return RedirectToAction(nameof(Index));
        }
        
        return View(model);
    }
    
    // GET: Users/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();
        
        var user = await _context.Users
            .Include(u => u.Profile)
            .FirstOrDefaultAsync(u => u.Id == id);
        
        if (user == null)
            return NotFound();
        
        return View(user);
    }
    
    // POST: Users/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            TempData["Success"] = "User deleted successfully!";
        }
        
        return RedirectToAction(nameof(Index));
    }
    
    private bool UserExists(int id)
    {
        return _context.Users.Any(e => e.Id == id);
    }
}