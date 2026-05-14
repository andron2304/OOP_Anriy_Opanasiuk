using App.Domain;

namespace App.Interfaces;

public interface IOrderRepository
{
    Task SaveAsync(IEnumerable<Order> orders);
    Task<IEnumerable<Order>> LoadAsync();
}