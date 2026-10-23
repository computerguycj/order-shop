using OrderShop.AcceptanceTests.Drivers;

namespace OrderShop.AcceptanceTests.Dsl;

// Layer 2 – the DSL.
// Speaks the language of the problem domain, fills in defaults the specs
// don't care about, and remembers which order "that order" is.
public class OrdersDsl(IOrdersDriver driver)
{
    private string orderId = "";
    private bool cancellationAccepted;

    public void PlaceOrder(string product = "Standard widget", int quantity = 1) =>
        orderId = driver.PlaceOrder(new OrderDetails(
            Customer: "Test customer",      // defaults: the specs never mention these
            Address:  "1 Test Street",
            Product:  product,
            Quantity: quantity,
            Payment:  "Test card"));

    public void DispatchOrder() => driver.DispatchOrder(orderId);

    public void CancelOrder() => cancellationAccepted = driver.CancelOrder(orderId);

    public void AssertOrderCancelled()
    {
        Assert.That(cancellationAccepted, Is.True, "cancellation was refused");
        Assert.That(driver.GetOrderStatus(orderId), Is.EqualTo("Cancelled"));
    }

    public void AssertCancellationRefused()
    {
        Assert.That(cancellationAccepted, Is.False, "cancellation was accepted");
        Assert.That(driver.GetOrderStatus(orderId), Is.EqualTo("Dispatched"));
    }
}
