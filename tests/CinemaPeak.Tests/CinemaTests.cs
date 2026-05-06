using CinemaPeak.Domain.Models;
using Xunit;

namespace CinemaPeak.Tests;

public class CinemaTests
{
    [Fact]
    public void StandardTicket_ShouldReturnBasePrice()
    {
        var ticket = new StandardTicket(1, 1, 100m);
        Assert.Equal(100m, ticket.CalculateFinalPrice());
    }

    [Fact]
    public void VipTicket_ShouldHave50PercentMarkup()
    {
        var basePrice = 100m;
        var ticket = new VipTicket(1, 1, basePrice);
        var expected = basePrice * 1.5m;
        Assert.Equal(expected, ticket.CalculateFinalPrice());
    }

    [Fact]
    public void Movie_Constructor_ShouldThrowExceptionForEmptyTitle()
    {
        Assert.Throws<ArgumentException>(() => new Movie("", 120));
    }

    [Fact]
    public void Movie_Constructor_ShouldThrowExceptionForShortDuration()
    {
        Assert.Throws<ArgumentException>(() => new Movie("Бетмен", 5));
    }

    [Fact]
    public void Ticket_ShouldThrowIfRowIsZeroOrNegative()
    {
        Assert.Throws<Exception>(() => new StandardTicket(0, 5, 100m));
    }
}