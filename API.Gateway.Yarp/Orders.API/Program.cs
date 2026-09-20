using Microsoft.EntityFrameworkCore;
// using Microsoft.OpenApi.Models; // removed: avoid compile dependency on Microsoft.OpenApi types

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// Swagger (Swashbuckle)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbContext using InMemory provider for demo purposes
builder.Services.AddDbContext<Orders.API.Data.OrdersDbContext>(options =>
    options.UseInMemoryDatabase("OrdersDb"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

// Minimal APIs for Orders CRUD
app.MapGet("/api/orders", async (Orders.API.Data.OrdersDbContext db) =>
{
    return await db.Orders.ToListAsync<Orders.API.Models.Order>();
});

app.MapGet("/api/orders/{id:int}", async (int id, Orders.API.Data.OrdersDbContext db) =>
{
    var order = await db.Orders.FindAsync(id);
    return order is not null ? Results.Ok(order) : Results.NotFound();
});

app.MapPost("/api/orders", async (Orders.API.Models.Order order, Orders.API.Data.OrdersDbContext db) =>
{
    db.Orders.Add(order);
    await db.SaveChangesAsync();
    return Results.Created($"/api/orders/{order.Id}", order);
});

app.MapPut("/api/orders/{id:int}", async (int id, Orders.API.Models.Order input, Orders.API.Data.OrdersDbContext db) =>
{
    var order = await db.Orders.FindAsync(id);
    if (order is null) return Results.NotFound();
    order.CustomerName = input.CustomerName;
    order.OrderDate = input.OrderDate;
    order.Total = input.Total;
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/api/orders/{id:int}", async (int id, Orders.API.Data.OrdersDbContext db) =>
{
    var order = await db.Orders.FindAsync(id);
    if (order is null) return Results.NotFound();
    db.Orders.Remove(order);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();
