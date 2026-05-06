namespace CinemaPeak.Domain.Interfaces;
using CinemaPeak.Domain.Models; 

public interface ITicketRepository {
    void Add(Ticket ticket);            
    IEnumerable<Ticket> GetAll();    
}