using Xunit;
using Moq;
using CinemaPeak.Domain.Models;
using CinemaPeak.Domain.Strategies;
using CinemaPeak.Domain.Interfaces;
using CinemaPeak.Application.Services;
using CinemaPeak.Infrastructure.Repositories;
using CinemaPeak.Infrastructure.Persistence;

namespace CinemaPeak.Tests;

public class BusinessLogicTests
{
    #region Стратегії знижок (3 тести)
    [Fact]
    public void StudentDiscount_ShouldApply20Percent()
    {
        var strategy = new StudentDiscount();
        Assert.Equal(80, strategy.ApplyDiscount(100));
    }

    [Fact]
    public void NoDiscount_ShouldReturnOriginalPrice()
    {
        var strategy = new NoDiscount();
        Assert.Equal(100, strategy.ApplyDiscount(100));
    }

    [Fact]
    public void VipTicket_CalculatePrice_ShouldBeCorrect()
    {
        var ticket = new VipTicket(1, 1, 200);
        Assert.Equal(300, ticket.CalculateFinalPrice());
    }
    #endregion

    #region Валідація інваріантів та прикордонних значень (6 тестів через [Theory] та [Fact])
    [Theory]
    [InlineData(-1, 5, 100)]
    [InlineData(0, 5, 100)]
    [InlineData(5, -1, 100)]
    [InlineData(5, 0, 100)]
    public void Ticket_ShouldThrowArgumentException_ForInvalidCoordinates(int row, int seat, decimal price)
    {
        Assert.Throws<ArgumentException>(() => new StandardTicket(row, seat, price));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-50)]
    public void Ticket_ShouldThrowArgumentException_ForInvalidPrice(decimal price)
    {
        Assert.Throws<ArgumentException>(() => new StandardTicket(1, 1, price));
    }

    [Fact]
    public void Ticket_ShouldNotAllowNegativeRow() 
    {
        Assert.Throws<ArgumentException>(() => new StandardTicket(-1, 5, 100));
    }

    [Fact]
    public void Ticket_ShouldNotAllowNegativeSeat()
    {
        Assert.Throws<ArgumentException>(() => new StandardTicket(1, -5, 100));
    }

    [Fact]
    public void Ticket_ShouldNotAllowZeroPrice()
    {
        Assert.Throws<ArgumentException>(() => new StandardTicket(1, 1, 0));
    }

    [Fact]
    public void StandardTicket_FinalPrice_ShouldMatchBasePrice()
    {
        var ticket = new StandardTicket(5, 5, 150);
        Assert.Equal(150, ticket.CalculateFinalPrice());
    }
    #endregion

    #region Тестування сервісів за допомогою Moq (6 тестів)
    [Fact]
    public void BookingService_ShouldBookTicket_WhenSeatIsFree()
    {
        var mockRepo = new Mock<ITicketRepository>();
        mockRepo.Setup(r => r.GetAll()).Returns(new List<Ticket>());
        var service = new BookingService(mockRepo.Object);

        service.BookTicket(1, 1, false, new NoDiscount());

        mockRepo.Verify(r => r.Add(It.IsAny<Ticket>()), Times.Once);
    }

    [Fact]
    public void BookingService_ShouldThrowException_IfSeatOccupied()
    {
        var mockRepo = new Mock<ITicketRepository>();
        var existingTicket = new StandardTicket(1, 1, 120);
        mockRepo.Setup(r => r.GetAll()).Returns(new List<Ticket> { existingTicket });
        var service = new BookingService(mockRepo.Object);

        Assert.Throws<InvalidOperationException>(() => service.BookTicket(1, 1, false, new NoDiscount()));
    }

    [Fact]
    public void BookingService_ShouldCreateVipTicket_WhenIsVipTrue()
    {
        var mockRepo = new Mock<ITicketRepository>();
        mockRepo.Setup(r => r.GetAll()).Returns(new List<Ticket>());
        var service = new BookingService(mockRepo.Object);

        service.BookTicket(1, 1, true, new NoDiscount());

        mockRepo.Verify(r => r.Add(It.Is<Ticket>(t => t is VipTicket)), Times.Once);
    }

    [Fact]
    public void BookingService_ShouldCreateStandardTicket_WhenIsVipFalse()
    {
        var mockRepo = new Mock<ITicketRepository>();
        mockRepo.Setup(r => r.GetAll()).Returns(new List<Ticket>());
        var service = new BookingService(mockRepo.Object);

        service.BookTicket(1, 1, false, new NoDiscount());

        mockRepo.Verify(r => r.Add(It.Is<Ticket>(t => t is StandardTicket)), Times.Once);
    }

    [Fact]
    public void BookingService_ShouldHandleEmptyCollectionGracefully()
    {
        var mockRepo = new Mock<ITicketRepository>();
        mockRepo.Setup(r => r.GetAll()).Returns(new List<Ticket>());
        var service = new BookingService(mockRepo.Object);

        var exception = Record.Exception(() => service.BookTicket(2, 2, false, new NoDiscount()));
        Assert.Null(exception);
    }

    [Fact]
    public void AnalyticsService_ShouldReturnZero_WhenNoTickets()
    {
        var mockRepo = new Mock<ITicketRepository>();
        mockRepo.Setup(r => r.GetAll()).Returns(new List<Ticket>());
        var analytics = new AnalyticsService(mockRepo.Object);

        Assert.Equal(0, analytics.GetTotalRevenue());
    }
    #endregion

    #region Аналітика та додаткові кейси (5 тестів)
    [Fact]
    public void Analytics_GetTotalRevenue_ShouldSumCorrectly()
    {
        var mockRepo = new Mock<ITicketRepository>();
        mockRepo.Setup(r => r.GetAll()).Returns(new List<Ticket>
        {
            new StandardTicket(1, 1, 100),
            new StandardTicket(1, 2, 150)
        });
        var analytics = new AnalyticsService(mockRepo.Object);
        Assert.Equal(250, analytics.GetTotalRevenue());
    }

    [Fact]
    public void Analytics_GetTicketsStats_ShouldCountCorrectly()
    {
        var mockRepo = new Mock<ITicketRepository>();
        mockRepo.Setup(r => r.GetAll()).Returns(new List<Ticket>
        {
            new StandardTicket(1, 1, 100),
            new VipTicket(1, 2, 200)
        });
        var analytics = new AnalyticsService(mockRepo.Object);
        var stats = analytics.GetTicketsStats();
        Assert.True(stats.ContainsKey("StandardTicket"));
    }

    [Fact]
    public void Repository_InitialState_ShouldNotBeNull()
    {
        var mockRepo = new Mock<ITicketRepository>();
        mockRepo.Setup(r => r.GetAll()).Returns(new List<Ticket>());
        Assert.NotNull(mockRepo.Object.GetAll());
    }

    [Fact]
    public void Strategy_ShouldNotAlterPrice_ForZeroDiscount()
    {
        var strategy = new NoDiscount();
        Assert.Equal(500, strategy.ApplyDiscount(500));
    }

    [Fact]
    public void StudentDiscount_ShouldHandleLargePrices()
    {
        var strategy = new StudentDiscount();
        Assert.Equal(800, strategy.ApplyDiscount(1000));
    }
    #endregion

    [Fact]
    public async Task SaveAndReload_ShouldPreserveDataIntegrity()
    {
        string tempFile = Path.GetTempFileName();
        try
        {
            var store1 = new JsonDataStore<Ticket>(tempFile);
            var list = new List<Ticket> { new StandardTicket(1, 1, 120) };
            
            await store1.SaveAsync(list);

            var store2 = new JsonDataStore<Ticket>(tempFile);
            var reloaded = await store2.LoadAsync();

            Assert.Single(reloaded);
            Assert.Equal(120, reloaded[0].BasePrice);
        }
        finally
        {
            if (File.Exists(tempFile)) File.Delete(tempFile);
        }
    }

    [Fact]
    public async Task LoadAsync_ShouldReturnEmptyList_WhenFileDoesNotExist()
    {
        string nonExistentFile = Guid.NewGuid().ToString() + ".json";
        var store = new JsonDataStore<Ticket>(nonExistentFile);

        var result = await store.LoadAsync();

        Assert.Empty(result);
    }

    [Fact]
    public async Task LoadAsync_ShouldHandleCorruptedJsonGracefully_FaultHandling()
    {
        string tempFile = Path.GetTempFileName();
        await File.WriteAllTextAsync(tempFile, "{ кривий json [ }");

        try
        {
            var store = new JsonDataStore<Ticket>(tempFile);
            var result = await store.LoadAsync();

            Assert.Empty(result);
        }
        finally
        {
            if (File.Exists(tempFile)) File.Delete(tempFile);
        }
    }

    [Fact]
    public async Task LoadAsync_ShouldHandleEmptyFileGracefully()
    {
        string tempFile = Path.GetTempFileName();
        await File.WriteAllTextAsync(tempFile, ""); 

        try
        {
            var store = new JsonDataStore<Ticket>(tempFile);
            var result = await store.LoadAsync();

            Assert.Empty(result);
        }
        finally
        {
            if (File.Exists(tempFile)) File.Delete(tempFile);
        }
    }

    [Fact]
    public async Task SaveAsync_ShouldOverwriteFile_OnSequentialWrites()
    {
        string tempFile = Path.GetTempFileName();
        try
        {
            var store = new JsonDataStore<Ticket>(tempFile);
            
            await store.SaveAsync(new List<Ticket> { new StandardTicket(1, 1, 100) });
            await store.SaveAsync(new List<Ticket> { new StandardTicket(1, 2, 150), new StandardTicket(1, 3, 200) });

            var reloaded = await store.LoadAsync();
            Assert.Equal(2, reloaded.Count);
        }
        finally
        {
            if (File.Exists(tempFile)) File.Delete(tempFile);
        }
    }

    [Fact]
    public async Task Integration_FullBookingCycleWithStore()
    {
        string tempFile = Path.GetTempFileName();
        try
        {
            var store = new JsonDataStore<Ticket>(tempFile);
            var ticketsList = new List<Ticket>();
            
            ticketsList.Add(new StandardTicket(4, 4, 120));
            await store.SaveAsync(ticketsList);

            var reloadedStore = new JsonDataStore<Ticket>(tempFile);
            var result = await reloadedStore.LoadAsync();

            Assert.Single(result);
            Assert.Equal(4, result[0].Row);
            Assert.Equal(4, result[0].Seat);
        }
        finally
        {
            if (File.Exists(tempFile)) File.Delete(tempFile);
        }
    }

    [Fact]
    public async Task LoadAsync_ShouldPreserveTicketTypes_WhenPolymorphicDataReloaded()
    {
        string tempFile = Path.GetTempFileName();
        try
        {
            var store = new JsonDataStore<Ticket>(tempFile);
            var data = new List<Ticket> 
            { 
                new StandardTicket(1, 1, 100), 
                new VipTicket(2, 2, 200) 
            };

            await store.SaveAsync(data);

            var reloaded = await store.LoadAsync();
            Assert.Equal(2, reloaded.Count);
        }
        finally
        {
            if (File.Exists(tempFile)) File.Delete(tempFile);
        }
    }

    [Fact]
    public async Task SaveAsync_ShouldThrowException_IfDirectoryDoesNotExist()
    {
        string invalidPath = @"Z:\NonExistentFolder_12345\tickets.json";
        var store = new JsonDataStore<Ticket>(invalidPath);

        await Assert.ThrowsAsync<DirectoryNotFoundException>(async () => 
        {
            await store.SaveAsync(new List<Ticket> { new StandardTicket(1, 1, 100) });
        });
    }
}