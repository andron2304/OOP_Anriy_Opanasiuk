using Xunit;
using FluentAssertions;
using App.Domain;
using App.Infrastructure;
using App.Services;

namespace App.Tests;

public class IntegrationTests : IDisposable
{
    private readonly string _tempFile;

    public IntegrationTests()
    {
        _tempFile = Path.GetTempFileName();
    }

    public void Dispose()
    {
        if (File.Exists(_tempFile)) File.Delete(_tempFile);
    }

    [Fact]
    public async Task SaveAndReload_PreservesState() // Інтеграційний 1
    {
        var repo1 = new JsonOrderRepository(_tempFile);
        var service1 = new OrderService(repo1);
        service1.CreateOrder(150.5m);
        await service1.SaveStateAsync();

        var repo2 = new JsonOrderRepository(_tempFile);
        var service2 = new OrderService(repo2);
        await service2.LoadStateAsync();

        service2.GetAllOrders().Should().ContainSingle();
        service2.GetAllOrders().First().TotalAmount.Should().Be(150.5m);
    }

    [Fact]
    public async Task Load_EmptyFile_ReturnsEmptyList() // Інтеграційний 2
    {
        File.WriteAllText(_tempFile, "");
        var repo = new JsonOrderRepository(_tempFile);
        var result = await repo.LoadAsync();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task Load_CorruptedJson_ThrowsDomainException() // Інтеграційний 3 (Негативний)
    {
        File.WriteAllText(_tempFile, "{ bad_json: true ");
        var repo = new JsonOrderRepository(_tempFile);
        
        Func<Task> act = async () => await repo.LoadAsync();
        await act.Should().ThrowAsync<DomainException>().WithMessage("Файл пошкоджено*");
    }

    [Fact]
    public async Task Save_ToInvalidPath_ThrowsException() // Інтеграційний 4 (Негативний I/O)
    {
        var invalidPath = "X:\\InvalidDrive\\file.json";
        var repo = new JsonOrderRepository(invalidPath);
        
        Func<Task> act = async () => await repo.SaveAsync(new List<Order> { new Order(Guid.NewGuid(), 10) });
        await act.Should().ThrowAsync<DomainException>().WithMessage("Помилка I/O*");
    }

    [Fact]
    public async Task FullCycle_Create_Save_Load_Modify_Save() // Інтеграційний 5-8 (Комплексний)
    {
        var repo = new JsonOrderRepository(_tempFile);
        var service = new OrderService(repo);
        
        var order = service.CreateOrder(500);
        await service.SaveStateAsync();
        
        var service2 = new OrderService(new JsonOrderRepository(_tempFile));
        await service2.LoadStateAsync();
        
        service2.PayOrder(order.Id);
        await service2.SaveStateAsync();
        
        var service3 = new OrderService(new JsonOrderRepository(_tempFile));
        await service3.LoadStateAsync();
        service3.GetAllOrders().First().Status.Should().Be("Paid");
    }
}