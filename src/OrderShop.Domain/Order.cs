namespace OrderShop.Domain;

public enum OrderStatus { Placed, Dispatched, Cancelled }

// The business rules live here, with no knowledge of HTTP, JSON or storage.
public class Order(string id)
{
    public string Id { get; } = id;
    public OrderStatus Status { get; private set; } = OrderStatus.Placed;

    public void Dispatch() => Status = OrderStatus.Dispatched;

    public bool TryCancel()
    {
        if (Status == OrderStatus.Dispatched) return false;   // business rule
        Status = OrderStatus.Cancelled;
        return true;
    }
}
