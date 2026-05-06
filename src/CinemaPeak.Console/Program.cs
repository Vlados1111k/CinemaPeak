using CinemaPeak.Application.Services;
using CinemaPeak.Infrastructure.Repositories;

Console.WriteLine("Ласкаво просимо до CinemaPeak");

var repo = new InMemoryTicketRepository();

var bookingService = new BookingService(repo);

Console.Write("Введіть ряд: ");
int row = int.Parse(Console.ReadLine() ?? "1");

Console.Write("Введіть місце: ");
int seat = int.Parse(Console.ReadLine() ?? "1");

Console.Write("Ви VIP-клієнт? (так/ні): ");
bool isVip = Console.ReadLine()?.ToLower() == "так";

try 
{
    bookingService.CreateBooking(row, seat, isVip);
    Console.WriteLine("Операція завершена успішно!");
}
catch (Exception ex)
{
    Console.WriteLine($"Помилка: {ex.Message}");
}