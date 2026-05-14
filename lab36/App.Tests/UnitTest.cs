using Xunit;
using Moq;
using FluentAssertions;
using App.Domain;
using App.Interfaces;
using App.Services;

namespace App.Tests;

public class UnitTests
{
    // --- Тести сутностей (Domain) ---
    [Theory]
    [InlineData(10.5)]
    [InlineData(0)]
    [InlineData(9999.99)]
    public void Order_Creation_WithValidAmount_SetsStatusNew(decimal amount) // 3 кейси
    {
        var order = new Order(Guid.NewGuid(), amount);
        order.Status.Should().Be("New");
        order.TotalAmount.Should().Be(amount);
    }

    [Fact]
    public void Order_Creation_NegativeAmount_ThrowsDomainException() // 1 кейс
    {
        Action act = () => new Order(Guid.NewGuid(), -5m);
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Order_Pay_ValidState_ChangesStatus() // 1 кейс
    {
        var order = new Order(Guid.NewGuid(), 100);
        order.Pay();
        order.Status.Should().Be("Paid");
    }

    [Fact]
    public void Order_Pay_AlreadyPaid_ThrowsException() // 1 кейс
    {
        var order = new Order(Guid.NewGuid(), 100);
        order.Pay();
        Action act = () => order.Pay();
        act.Should().Throw<DomainException>().WithMessage("Оплатити можна лише нове замовлення.");
    }

    // --- Тести сервісів (Service) із Mocks ---
    [Fact]
    public void OrderService_CreateOrder_AddsToList() // 1 кейс
    {
        var mockRepo = new Mock<IOrderRepository>();
        var service = new OrderService(mockRepo.Object);
        var order = service.CreateOrder(50);
        
        service.GetAllOrders().Should().Contain(order);
    }

    [Fact]
    public void OrderService_PayOrder_NonExistent_ThrowsException() // 1 кейс
    {
        var mockRepo = new Mock<IOrderRepository>();
        var service = new OrderService(mockRepo.Object);
        Action act = () => service.PayOrder(Guid.NewGuid());
        act.Should().Throw<DomainException>().WithMessage("Замовлення не знайдено.");
    }

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    public async Task OrderService_LoadState_PopulatesInternalList(int orderCount) // 3 кейси
    {
        var mockRepo = new Mock<IOrderRepository>();
        var fakeOrders = Enumerable.Range(0, orderCount).Select(_ => new Order(Guid.NewGuid(), 10)).ToList();
        mockRepo.Setup(r => r.LoadAsync()).ReturnsAsync(fakeOrders);
        
        var service = new OrderService(mockRepo.Object);
        await service.LoadStateAsync();
        
        service.GetAllOrders().Should().HaveCount(orderCount);
    }

    [Fact]
    public async Task OrderService_SaveState_CallsRepository() // 1 кейс
    {
        var mockRepo = new Mock<IOrderRepository>();
        var service = new OrderService(mockRepo.Object);
        service.CreateOrder(100);
        
        await service.SaveStateAsync();
        mockRepo.Verify(r => r.SaveAsync(It.IsAny<IEnumerable<Order>>()), Times.Once);
    }
}