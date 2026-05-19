using CinemaPeak.Domain.Interfaces;
using CinemaPeak.Domain.Models;

namespace CinemaPeak.Application.Services;

public class AnalyticsService
{
    private readonly ITicketRepository _repository;

    public AnalyticsService(ITicketRepository repository)
    {
        _repository = repository;
    }

    public decimal GetTotalRevenue()
    {
        var tickets = _repository.GetAll();
        return tickets.Sum(t => t.CalculateFinalPrice());
    }

    public HashSet<int> GetOccupiedRowsLog()
    {
        var tickets = _repository.GetAll();
        var uniqueRows = new HashSet<int>();
        
        foreach (var ticket in tickets)
        {
            uniqueRows.Add(ticket.Row);
        }
        
        return uniqueRows;
    }

    public Dictionary<string, int> GetTicketsStats()
    {
        return _repository.GetAll()
            .GroupBy(t => t.GetType().Name)
            .ToDictionary(g => g.Key, g => g.Count());
    }
}