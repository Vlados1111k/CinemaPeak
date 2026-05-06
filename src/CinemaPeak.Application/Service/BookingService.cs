namespace CinemaPeak.Application.Services;

using CinemaPeak.Domain.Interfaces;
using CinemaPeak.Domain.Models;

public class BookingService
{
    private readonly ITicketRepository _repository;

    public BookingService(ITicketRepository repository)
    {
        _repository = repository;
    }

    public void CreateBooking(int row, int seat, bool isVip)
    {
        Ticket ticket = isVip 
            ? new VipTicket(row, seat, 200) 
            : new StandardTicket(row, seat, 120);

        _repository.Add(ticket);
        
        Console.WriteLine($"[Service]: Квиток заброньовано (Ряд: {row}, Місце: {seat})");
    }
}