using Konscious.Security.Cryptography;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyGuitarShopData;
using Serilog;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Ganss.Xss;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

public class AccountController : Controller
{
    private readonly MyGuitarShopContext _context;

    public AccountController(MyGuitarShopContext context)
    {
        _context = context;
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string emailAddress, string password)
    {
        if (string.IsNullOrEmpty(emailAddress) || string.IsNullOrEmpty(password))
        {
            ModelState.AddModelError("", "Email address and password are required.");
            return View();
        }
        try
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.EmailAddress == emailAddress);

            if (customer != null && VerifyPassword(customer.PasswordHash, password, customer.Salt))
            {
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, customer.EmailAddress),
                new Claim(ClaimTypes.NameIdentifier, customer.CustomerId.ToString()),
            };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Products");
            }
            else
            {
                ModelState.AddModelError("", "Invalid email address or password.");
                return View();
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An exception occurred while attempting to log in for email address {EmailAddress}", emailAddress);
            ModelState.AddModelError("", "An unexpected error occurred. Please try again later.");
            return View();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Products");
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Register(Customer customer)
    {
        if (_context.Customers.Any(c => c.EmailAddress == customer.EmailAddress))
        {
            ModelState.AddModelError("", "Email is already taken.");
            return View(customer);
        }
        try
        {
            byte[] salt = GenerateRandomSalt();
            string hashedPassword = HashPassword(customer.Password, salt);

            var newCustomer = new Customer
            {
                EmailAddress = customer.EmailAddress,
                PasswordHash = hashedPassword,
                Salt = salt,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
            };

            _context.Customers.Add(newCustomer);
            _context.SaveChanges();

            return RedirectToAction("Login");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to register a new customer with email: {EmailAddress}", customer.EmailAddress);
            ModelState.AddModelError("", "An unexpected error occurred during registration. Please try again later.");
            return View(customer);
        }
    }

    private string HashPassword(string password, byte[] salt)
    {
        using (var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password)))
        {
            argon2.Salt = salt;
            argon2.DegreeOfParallelism = 8;
            argon2.MemorySize = 65536;
            argon2.Iterations = 4;

            return Convert.ToBase64String(argon2.GetBytes(16));
        }
    }

    private bool VerifyPassword(string hashedPassword, string password, byte[] salt)
    {
        using (var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password)))
        {
            argon2.Salt = salt;
            argon2.DegreeOfParallelism = 8;
            argon2.MemorySize = 65536;
            argon2.Iterations = 4;

            var hashBytes = Convert.FromBase64String(hashedPassword);
            var testHash = argon2.GetBytes(16);

            return hashBytes.SequenceEqual(testHash);
        }
    }

    public async Task<IActionResult> MyProducts()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Login");
        }
        try
        {
            var orders = await _context.Orders
            .Where(o => o.CustomerId == int.Parse(userId))
            .Select(o => new MyProductsViewModel
            {
                OrderId = o.OrderId,
                OrderDate = o.OrderDate,
                Items = o.OrderItems.Select(i => new OrderItemDetails
                {
                    ProductName = i.Product.ProductName,
                    ItemPrice = i.ItemPrice,
                    DiscountAmount = i.DiscountAmount,
                    Quantity = i.Quantity
                }).ToList()
            })
            .ToListAsync();

            return View(orders);

        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to retrieve products for user ID {UserId}", userId);
            return RedirectToAction("Error", "Home");
        }
    }
    public async Task<IActionResult> ViewInformation()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Login");
        }

        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.CustomerId.ToString() == userId);

        if (customer == null)
        {
            return NotFound();
        }

        return View(customer);
    }
    public async Task<IActionResult> EditInformation()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Login");
        }

        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.CustomerId.ToString() == userId);

        if (customer == null)
        {
            return NotFound();
        }

        return View(customer);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditInformation(Customer customer)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId) || customer.CustomerId.ToString() != userId)
        {
            return RedirectToAction("Login");
        }

        if (ModelState.IsValid)
        {
            try
            {
                var existingCustomer = await _context.Customers
                    .FirstOrDefaultAsync(c => c.CustomerId.ToString() == userId);

                if (existingCustomer == null)
                {
                    return NotFound();
                }

                var sanitizer = new HtmlSanitizer();

                existingCustomer.FirstName = sanitizer.Sanitize(customer.FirstName);
                existingCustomer.LastName = sanitizer.Sanitize(customer.LastName);
                existingCustomer.EmailAddress = sanitizer.Sanitize(customer.EmailAddress);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ViewInformation));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(customer.CustomerId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }
        return View(customer);
    }

    private bool CustomerExists(int id)
    {
        return _context.Customers.Any(e => e.CustomerId == id);
    }
    private byte[] GenerateRandomSalt()
    {
        var buffer = new byte[16];
        using (var rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(buffer);
        }
        return buffer;
    }
}