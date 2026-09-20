using Microsoft.EntityFrameworkCore;
using Orders.API.Models;

namespace Orders.API.Data;

public class OrdersDbContext : DbContext
{
    public OrdersDbContext(DbContextOptions<OrdersDbContext> options) : base(options) { }

    public DbSet<Order> Orders => Set<Order>();
}
