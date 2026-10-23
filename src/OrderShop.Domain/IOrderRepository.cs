namespace OrderShop.Domain;

// Declared by the domain, implemented outside it, so the domain never
// depends on how orders are stored.
public interface IOrderRepository
{
    void Add(Order order);
    Order? Find(string id);
}
