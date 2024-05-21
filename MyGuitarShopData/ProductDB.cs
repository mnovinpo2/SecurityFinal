using Microsoft.EntityFrameworkCore;
using MyGuitarShopData;
using Serilog; // Ensure Serilog is referenced
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class ProductDb
{
    private readonly MyGuitarShopContext _context;

    public ProductDb(MyGuitarShopContext context)
    {
        _context = context;
    }

    public async Task<List<Product>> GetAllProductsAsync()
    {
        try
        {
            return await _context.Products.ToListAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to retrieve all products.");
            throw;
        }
    }

    public async Task<List<Product>> SearchProductsAsync(string searchTerm)
    {
        try
        {
            return await _context.Products
                                 .Where(p => EF.Functions.Like(p.ProductName, $"%{searchTerm}%"))
                                 .ToListAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to search products with search term: {SearchTerm}", searchTerm);
            throw;
        }
    }

    public async Task AddOrderAsync(Order order, OrderItem orderItem)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            orderItem.OrderId = order.OrderId;
            _context.OrderItems.Add(orderItem);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            Log.Error(ex, "Failed to add order and order item. Order ID: {OrderId}, Product ID: {ProductId}", order.OrderId, orderItem.ProductId);
            throw;
        }
    }

    public async Task<Product> GetProductByIdAsync(int id)
    {
        try
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to retrieve product by ID: {ProductId}", id);
            throw;
        }
    }
}