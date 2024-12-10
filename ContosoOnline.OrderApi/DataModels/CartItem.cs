namespace ContosoOnline.OrderApi.DataModels;

public class CartItem
{
    public Guid Id { get; set; }
    public Guid CartId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}