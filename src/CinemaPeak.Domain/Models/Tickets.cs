namespace CinemaPeak.Domain.Models;

public abstract class Ticket {
    public Guid Id { get; } = Guid.NewGuid();
    public int Row { get; }
    public int Seat { get; }
    public decimal BasePrice { get; }

    protected Ticket(int row, int seat, decimal price) {
        if (row <= 0 || seat <= 0) throw new Exception("Місце має бути додатним числом");
        Row = row;
        Seat = seat;
        BasePrice = price;
    }

    public abstract decimal CalculateFinalPrice();
}

public class StandardTicket : Ticket {
    public StandardTicket(int row, int seat, decimal price) : base(row, seat, price) { }
    public override decimal CalculateFinalPrice() => BasePrice;
}

public class VipTicket : Ticket {
    public VipTicket(int row, int seat, decimal price) : base(row, seat, price) { }
    public override decimal CalculateFinalPrice() => BasePrice * 1.5m; 
}