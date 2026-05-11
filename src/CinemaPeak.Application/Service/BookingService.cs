using CinemaPeak.Domain.Interfaces;
using CinemaPeak.Domain.Models;
using CinemaPeak.Domain.Strategies;

namespace CinemaPeak.Application.Services;

public class BookingService
{
    private readonly ITicketRepository _repository;

    public BookingService(ITicketRepository repository)
    {
        _repository = repository;
    }

    public void BookTicket(int row, int seat, bool isVip, IDiscountStrategy discount)
    {
        var allTickets = _repository.GetAll();
        if (allTickets.Any(t => t.Row == row && t.Seat == seat))
        {
            throw new InvalidOperationException($"Місце (Ряд: {row}, Місце: {seat}) вже зайняте!");
        }

        Ticket ticket = isVip 
            ? new VipTicket(row, seat, 200) 
            : new StandardTicket(row, seat, 120);

        _repository.Add(ticket);
    }
}