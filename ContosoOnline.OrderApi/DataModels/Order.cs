namespace ContosoOnline.OrderApi.DataModels;

public class Order
{
    public int Id { get; set; }
    public required int CartId { get; set; }
    public DateTime Received { get; set; }
    public DateTime? Processed { get; set; } = null;
}