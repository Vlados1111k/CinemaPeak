namespace CinemaPeak.Infrastructure.Repositories;

using CinemaPeak.Domain.Interfaces;
using CinemaPeak.Domain.Models;
using System.Collections.Generic;

public class InMemoryTicketRepository : ITicketRepository
{
    private readonly List<Ticket> _tickets = new();

    public void Add(Ticket ticket)
    {
        _tickets.Add(ticket);
        Console.WriteLine("[Repo]: Квиток додано в пам'ять.");
    }

    public IEnumerable<Ticket> GetAll()
    {
        return _tickets;
    }
}