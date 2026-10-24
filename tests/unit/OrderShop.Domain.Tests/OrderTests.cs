using OrderShop.Domain;

namespace OrderShop.Domain.Tests;

// Unit tests: check the domain rules directly, in isolation, with no HTTP.
// The acceptance specs check the same behaviour from the outside.
public class OrderTests
{
    [Test]
    public void APlacedOrderCanBeCancelled()
    {
        var order = Order.TryPlace("o-1", quantity: 1)!;

        Assert.That(order.TryCancel(), Is.True);
        Assert.That(order.Status, Is.EqualTo(OrderStatus.Cancelled));
    }

    [Test]
    public void ADispatchedOrderCannotBeCancelled()
    {
        var order = Order.TryPlace("o-1", quantity: 1)!;
        order.TryDispatch();

        Assert.That(order.TryCancel(), Is.False);
        Assert.That(order.Status, Is.EqualTo(OrderStatus.Dispatched));
    }

    [Test]
    public void ACancelledOrderCannotBeDispatched()
    {
        var order = Order.TryPlace("o-1", quantity: 1)!;
        order.TryCancel();

        Assert.That(order.TryDispatch(), Is.False);
        Assert.That(order.Status, Is.EqualTo(OrderStatus.Cancelled));
    }
}
