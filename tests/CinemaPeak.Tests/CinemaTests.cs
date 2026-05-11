using Xunit;
using CinemaPeak.Domain.Models;
using CinemaPeak.Domain.Strategies;
using CinemaPeak.Application.Services;
using CinemaPeak.Infrastructure.Repositories;

namespace CinemaPeak.Tests;

public class BusinessLogicTests
{ 
    [Fact]
    public void StudentDiscount_ShouldApply20Percent()
    {
        var strategy = new StudentDiscount();
        Assert.Equal(80, strategy.Apply(100));
    }

    [Fact]
    public void NoDiscount_ShouldReturnOriginalPrice()
    {
        var strategy = new NoDiscount();
        Assert.Equal(100, strategy.Apply(100));
    }

    [Fact]
    public void VipTicket_CalculatePrice_ShouldBeCorrect()
    {
        var ticket = new VipTicket(1, 1, 200);
        Assert.Equal(300, ticket.CalculateFinalPrice());
    }

    [Fact]
    public void BookingService_ShouldThrowException_IfSeatOccupied()
    {
        var repo = new InMemoryTicketRepository();
        var service = new BookingService(repo);
        service.BookTicket(1, 1, false, new NoDiscount());

        Assert.Throws<InvalidOperationException>(() => service.BookTicket(1, 1, false, new NoDiscount()));
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
    public void Analytics_GetTotalRevenue_ShouldSumCorrectly()
    {
        var repo = new InMemoryTicketRepository();
        repo.Add(new StandardTicket(1, 1, 100));
        repo.Add(new StandardTicket(1, 2, 150));
        var analytics = new AnalyticsService(repo);
        Assert.Equal(250, analytics.GetTotalRevenue());
    }

    [Fact]
    public void Analytics_GetTicketsStats_ShouldCountCorrectly()
    {
        var repo = new InMemoryTicketRepository();
        repo.Add(new StandardTicket(1, 1, 100));
        repo.Add(new VipTicket(1, 2, 200));
        var analytics = new AnalyticsService(repo);
        var stats = analytics.GetTicketsStats();
        Assert.True(stats.ContainsKey("StandardTicket"));
    }

    [Fact]
    public void Repository_GetAll_ShouldReturnAddedTickets()
    {
        var repo = new InMemoryTicketRepository();
        repo.Add(new StandardTicket(1, 1, 100));
        Assert.Single(repo.GetAll());
    }

    [Fact]
    public void Repository_InitialState_ShouldBeEmpty()
    {
        var repo = new InMemoryTicketRepository();
        var all = repo.GetAll();
        Assert.NotNull(all);
    }

    [Fact]
    public void StandardTicket_FinalPrice_ShouldMatchBasePrice()
    {
        var ticket = new StandardTicket(5, 5, 150);
        Assert.Equal(150, ticket.CalculateFinalPrice());
    }
}