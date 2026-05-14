using App.Domain;
using App.Interfaces;

namespace App.Services;

public class OrderService
{
    private readonly IOrderRepository _repository;
    private readonly List<Order> _orders = new();

    public OrderService(IOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task LoadStateAsync()
    {
        var loaded = await _repository.LoadAsync();
        _orders.Clear();
        _orders.AddRange(loaded);
    }

    public async Task SaveStateAsync()
    {
        await _repository.SaveAsync(_orders);
    }

    public Order CreateOrder(decimal amount)
    {
        var order = new Order(Guid.NewGuid(), amount);
        _orders.Add(order);
        return order;
    }

    public void PayOrder(Guid id)
    {
        var order = _orders.FirstOrDefault(o => o.Id == id);
        if (order == null) throw new DomainException("Замовлення не знайдено.");
        order.Pay();
    }

    public IEnumerable<Order> GetAllOrders() => _orders.AsReadOnly();
}