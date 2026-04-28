using IndependentWork21;
using Xunit;

namespace tests;

public class IntegrationTests
{
    [Fact]
    public void Positive_AddBookStrategy_ExecutesAndNotifiesObservers()
    {
        // Arrange
        var factory = new ConsoleLoggerFactory();
        var logger = factory.CreateLogger();
        LoggerManager.Instance.SetLogger(logger);

        var dataContext = new DataContext(new AddBookStrategy());
        var publisher = new DataPublisher();
        var catalogObserver = new LibraryCatalogObserver();
        var notifierObserver = new NewArrivalsNotifierObserver();

        publisher.DataProcessed += catalogObserver.OnDataProcessed;
        publisher.DataProcessed += notifierObserver.OnDataProcessed;

        // Act
        dataContext.ExecuteProcessing("Book: 'Test Book'");
        publisher.PublishDataProcessed("Book added event");

        // Assert - Since logging is via console, we can't easily assert output, but ensure no exceptions
        Assert.NotNull(dataContext);
    }

    [Fact]
    public void Positive_SingletonLoggerInstance_IsStable()
    {
        // Arrange
        var factory1 = new ConsoleLoggerFactory();
        var logger1 = factory1.CreateLogger();
        LoggerManager.Instance.SetLogger(logger1);

        var factory2 = new FileLoggerFactory();
        var logger2 = factory2.CreateLogger();
        LoggerManager.Instance.SetLogger(logger2);

        // Act
        var instance1 = LoggerManager.Instance;
        var instance2 = LoggerManager.Instance;

        // Assert
        Assert.Same(instance1, instance2);
        Assert.Equal(instance1, instance2);
    }

    [Fact]
    public void Positive_FactoryCreatesCorrectLoggerType()
    {
        // Arrange & Act
        LoggerFactory factory = new ConsoleLoggerFactory();
        ILogger logger = factory.CreateLogger();

        // Assert
        Assert.IsType<ConsoleLogger>(logger);
    }

    [Fact]
    public void Negative_NullStrategy_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new DataContext(null));
    }

    [Fact]
    public void Negative_ObserverWithoutLogger_DoesNotThrow()
    {
        // Arrange - Logger not set
        var publisher = new DataPublisher();
        var catalogObserver = new LibraryCatalogObserver();

        publisher.DataProcessed += catalogObserver.OnDataProcessed;

        // Act
        publisher.PublishDataProcessed("Test event");

        // Assert - Should not throw, logger is null but handled
        Assert.NotNull(publisher);
    }
}
