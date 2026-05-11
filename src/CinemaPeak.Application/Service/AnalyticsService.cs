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
        => _repository.GetAll().Sum(t => t.CalculateFinalPrice());

    public IEnumerable<Ticket> GetTicketsByRow(int row) 
        => _repository.GetAll().Where(t => t.Row == row);

    public IEnumerable<Ticket> GetTicketsSortedByPrice() 
        => _repository.GetAll().OrderByDescending(t => t.CalculateFinalPrice());

    public Dictionary<string, int> GetTicketsStats()
    {
        return _repository.GetAll()
            .GroupBy(t => t.GetType().Name)
            .ToDictionary(g => g.Key, g => g.Count());
    }
}