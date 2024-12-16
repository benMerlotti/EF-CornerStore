using CornerStore.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using CornerStore.Models.DTO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// allows passing datetimes without time zone data 
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// allows our api endpoints to access the database through Entity Framework Core and provides dummy value for testing
builder.Services.AddNpgsql<CornerStoreDbContext>(builder.Configuration["CornerStoreDbConnectionString"] ?? "testing");

// Set the JSON serializer options
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//endpoints go here

app.MapPost("cashiers/create", (CornerStoreDbContext db, Cashier newCashier) =>
{
    db.Cashiers.Add(newCashier);
    db.SaveChanges();
    return Results.Ok();
});

app.MapGet("cashiers", (CornerStoreDbContext db) =>
{
    return db.Cashiers
    .Include(c => c.Orders)
    .ThenInclude(c => c.Products)
    .Select(c => new CashierDTO
    {
        Id = c.Id,
        FirstName = c.FirstName,
        LastName = c.LastName,
        Orders = c.Orders.Select(o => new OrderDTO
        {
            Id = o.Id,
            CashierId = o.CashierId,
            PaidOnDate = o.PaidOnDate,
            Total = o.Total,
            Products = o.Products.Select(p => new ProductDTO
            {
                Id = p.Id,
                ProductName = p.ProductName,
                Price = p.Price,
                Brand = p.Brand,
                CategoryId = p.CategoryId,

            }).ToList()
        }).ToList()
    }).ToList();

});

app.MapGet("products", (CornerStoreDbContext db, string? search) =>
{
    var query = db.Products
        .Include(p => p.Category) // Load related Category for each Product
        .AsQueryable();

    // If a search query is provided, filter the products and categories
    if (!string.IsNullOrEmpty(search))
    {
        // Use `Contains` to check for the search term in both the ProductName and CategoryName (case-insensitive)
        query = query.Where(p => p.ProductName.ToLower().Contains(search.ToLower()) ||
                                 p.Category.CategoryName.ToLower().Contains(search.ToLower()));
    }

    // Execute the query and return the results
    var products = query.Select(p => new ProductQueryDTO
    {
        Id = p.Id,
        ProductName = p.ProductName,
        Price = p.Price,
        Brand = p.Brand,
        CategoryId = p.CategoryId,
        CategoryName = p.Category.CategoryName // Include the Category name in the result
    }).ToList();

    return products;
});

app.MapPost("products/create", (CornerStoreDbContext db, Product newProduct) =>
{
    db.Products.Add(newProduct);
    db.SaveChanges();
    return Results.Ok();
});

app.MapPut("products/edit-price", (CornerStoreDbContext db, int id, decimal price) =>
{
    Product chosenProduct = db.Products.FirstOrDefault(p => p.Id == id);
    chosenProduct.Price = price;

    db.SaveChanges();
    return Results.Ok();
});

app.MapGet("orders/{id}", (CornerStoreDbContext db, int id) =>
{
    return db.Orders
    .Include(o => o.Cashier)
    .Include(o => o.Products)
    .ThenInclude(p => p.Category)
    .Where(o => o.Id == id)
    .Select(o => new OrderDetailsDTO
    {
        Id = o.Id,
        CashierId = o.CashierId,
        CashierName = o.Cashier.FullName,
        Products = o.Products.Select(p => new ProductDTO
        {
            Id = p.Id,
            ProductName = p.ProductName,
            Price = p.Price,
            Brand = p.Brand,
            CategoryId = p.CategoryId,
            CategoryName = p.Category.CategoryName
        }).ToList()
    }).FirstOrDefault();
});

app.Run();

//don't move or change this!
public partial class Program { }