using CinemaPeak.Application.Services;
using CinemaPeak.Infrastructure.Repositories;
using CinemaPeak.Domain.Strategies;

var repo = new InMemoryTicketRepository();

var bookingService = new BookingService(repo);
var analytics = new AnalyticsService(repo);

Console.WriteLine("Аналітика");

bookingService.BookTicket(1, 10, true, new NoDiscount());

Console.WriteLine($"1. Загальна каса: {analytics.GetTotalRevenue()} грн");

var stats = analytics.GetTicketsStats();
Console.WriteLine("2. Статистика по типах:");
foreach (var s in stats)
{
    Console.WriteLine($"- {s.Key}: {s.Value} шт.");
}