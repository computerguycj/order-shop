namespace OrderShop.Drivers;

// Layer 3 – the protocol driver's interface.
// Anything that can place, dispatch and cancel orders can sit behind this:
// HTTP today, a message queue or a browser tomorrow.
public interface IOrdersDriver
{
    string PlaceOrder(OrderDetails details);   // returns the new order's id
    void DispatchOrder(string orderId);
    bool CancelOrder(string orderId);          // true = accepted, false = refused
    string GetOrderStatus(string orderId);
}

// The tests' own description of an order. Deliberately not the app's
// OrderRequest type, so the tests don't depend on the app's internals.
public record OrderDetails(string Customer, string Address, string Product, int Quantity, string Payment);
