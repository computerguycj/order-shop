using OrderShop.Domain;

namespace OrderShop.Domain.Tests;

public class OrderQuantityTests
{
    [Test]
    public void AnOrderForOneItemCanBePlaced()
    {
        var order = Order.TryPlace("o-1", quantity: 1);

        Assert.That(order?.Quantity, Is.EqualTo(1));
    }

    [Test]
    public void AnOrderForZeroItemsIsRefused()
    {
        Assert.That(Order.TryPlace("o-1", quantity: 0), Is.Null);
    }

    [Test]
    public void AnOrderForMoreThanTenItemsIsRefused()
    {
        Assert.That(Order.TryPlace("o-1", quantity: 11), Is.Null);
    }

    [Test]
    public void AnOrderForExactlyTenItemsCanBePlaced()
    {
        var order = Order.TryPlace("o-1", quantity: 10);

        Assert.That(order?.Quantity, Is.EqualTo(10));
    }

    [Test]
    public void AnyQuantityFromOneToTenCanBePlaced([Range(1, 10)] int quantity)
    {
        Assert.That(Order.TryPlace("o-1", quantity)?.Quantity, Is.EqualTo(quantity));
    }
}
