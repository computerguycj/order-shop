using OrderShop.Dsl;

namespace OrderShop.Specs;

public class PlaceOrderSpecs
{
    private OrdersDsl orders = null!;

    [SetUp]
    public void SetUp() => orders = OrdersDsl.Create();

    [Test]
    public void AnOrderForMoreThanTenItemsIsRejected()
    {
        orders.PlaceOrder(quantity: 11);    // When I place an order for 11 items
        orders.AssertOrderRejected();       // Then the order is rejected
    }
}
