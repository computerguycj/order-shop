using System.Collections.Concurrent;
using OrderShop.Domain;

namespace OrderShop.Api.Infrastructure;

// Storage adapter. Swap for a database-backed repository without touching the domain.
public class InMemoryOrderRepository : IOrderRepository
{
    private readonly ConcurrentDictionary<string, Order> orders = new();

    public void Add(Order order) => orders[order.Id] = order;

    public Order? Find(string id) => orders.GetValueOrDefault(id);
}
