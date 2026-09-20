namespace Orders.API.Models;

public class Order
{
    public int Id { get; set; }
    public string CustomerName { get; set; } = null!;
    public DateTime OrderDate { get; set; }
    public decimal Total { get; set; }
}
