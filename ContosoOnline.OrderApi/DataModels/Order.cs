namespace ContosoOnline.OrderApi.DataModels;

public class Order
{
    public Guid Id { get; set; }
    public required Guid CartId { get; set; }
    public DateTime Received { get; set; }
    public DateTime? Processed { get; set; } = null;
}