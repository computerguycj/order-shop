using NUnit.Framework;
using OrderShop.Drivers;

namespace OrderShop.Dsl;

// Layer 2 – the DSL.
// Speaks the language of the problem domain, fills in defaults the specs
// don't care about, and remembers which order "that order" is.
public class OrdersDsl(IOrdersDriver driver)
{
    private string orderId = "";
    private bool placementAccepted;
    private bool cancellationAccepted;
    private bool dispatchAccepted;

    // The one place that chooses a protocol driver. To run the same specs
    // through another channel (a browser, a queue), change it here.
    public static OrdersDsl Create() => new(new HttpOrdersDriver(SystemUnderTest.Client));

    public static void Shutdown() => SystemUnderTest.Stop();

    public void PlaceOrder(string product = "Standard widget", int quantity = 1)
    {
        var id = driver.PlaceOrder(new OrderDetails(
            Customer: "Test customer",      // defaults: the specs never mention these
            Address:  "1 Test Street",
            Product:  product,
            Quantity: quantity,
            Payment:  "Test card"));
        placementAccepted = id is not null;
        orderId = id ?? "";
    }

    public void DispatchOrder() => dispatchAccepted = driver.DispatchOrder(orderId);

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

    public void AssertDispatchRefused()
    {
        Assert.That(dispatchAccepted, Is.False, "dispatch was accepted");
        Assert.That(driver.GetOrderStatus(orderId), Is.EqualTo("Cancelled"));
    }

    public void AssertOrderRejected() =>
        Assert.That(placementAccepted, Is.False, "order was accepted");
}
