using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyGuitarShopData;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Ganss.Xss;
using Serilog;

public class ProductsController : Controller
{
    private readonly ProductDb _productDb;

    public ProductsController(ProductDb productDb)
    {
        _productDb = productDb;
    }

    public async Task<IActionResult> Index(string search)
    {
        var products = await (string.IsNullOrWhiteSpace(search) ?
                              _productDb.GetAllProductsAsync() :
                              _productDb.SearchProductsAsync(search));
        return View(products);
    }

    public async Task<IActionResult> Details(int id)
    {
        var product = await _productDb.GetProductByIdAsync(id);
        if (product == null)
            return NotFound();

        var sanitizer = new HtmlSanitizer();
        sanitizer.AllowedTags.Add("p");
        product.Description = sanitizer.Sanitize(product.Description);

        return View(product);
    }
    public async Task<IActionResult> Purchase(int id)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdString, out var customerId))
        {
            return RedirectToAction("Login", "Account");
        }

        var product = await _productDb.GetProductByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        decimal discountAmount = product.ListPrice * (product.DiscountPercent / 100m);
        var order = new Order
        {
            CustomerId = customerId,
            OrderDate = DateTime.UtcNow,
            ShipAmount = 0,
            TaxAmount = 0,
            ShipDate = null,
            CardType = "Default",
            CardNumber = "0000000000000000",
            CardExpires = "01/99",
        };
        var orderItem = new OrderItem
        {
            ProductId = id,
            Quantity = 1,
            ItemPrice = product.ListPrice,
            DiscountAmount = discountAmount,
        };

        try
        {
            await _productDb.AddOrderAsync(order, orderItem);
            TempData["SuccessMessage"] = "Product purchased successfully!";
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error processing purchase for customer ID {CustomerId} and product ID {ProductId}", customerId, id);
            TempData["ErrorMessage"] = "There was an error processing your purchase.";
        }

        return RedirectToAction("MyProducts", "Account");
    }
}