namespace OrderShop.Domain;

public enum OrderStatus { Placed, Dispatched, Cancelled }

// The business rules live here, with no knowledge of HTTP, JSON or storage.
public class Order(string id)
{
    public string Id { get; } = id;
    public OrderStatus Status { get; private set; } = OrderStatus.Placed;
    public int Quantity { get; private init; }

    public static Order? TryPlace(string id, int quantity) =>
        quantity is < 1 or > 10 ? null : new(id) { Quantity = quantity };   // business rule

    public bool TryDispatch()
    {
        if (Status == OrderStatus.Cancelled) return false;    // business rule
        Status = OrderStatus.Dispatched;
        return true;
    }

    public bool TryCancel()
    {
        if (Status == OrderStatus.Dispatched) return false;   // business rule
        Status = OrderStatus.Cancelled;
        return true;
    }
}
