using OrderShop.Dsl;

namespace OrderShop.Specs;

// Layer 1 – the test cases. The specs, almost word for word.
// This project can only see the DSL: it cannot reference a driver or HTTP.
public class CancelOrderSpecs
{
    private OrdersDsl orders = null!;

    [SetUp]
    public void SetUp() => orders = OrdersDsl.Create();

    [Test]
    public void CancellingAnUndispatchedOrderCancelsIt()
    {
        orders.PlaceOrder();                // Given I have placed an order that has not been dispatched
        orders.CancelOrder();               // When I cancel that order
        orders.AssertOrderCancelled();      // Then the order is cancelled
    }

    [Test]
    public void CancellingADispatchedOrderIsRefused()
    {
        orders.PlaceOrder();                // Given I have placed an order
        orders.DispatchOrder();             //   and it has been dispatched
        orders.CancelOrder();               // When I cancel that order
        orders.AssertCancellationRefused(); // Then the cancellation is refused
    }
}
