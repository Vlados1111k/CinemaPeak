using System.Text.Json.Serialization;

namespace CinemaPeak.Domain.Models;

[JsonDerivedType(typeof(StandardTicket), typeDiscriminator: "standard")]
[JsonDerivedType(typeof(VipTicket), typeDiscriminator: "vip")]
public abstract class Ticket 
{
    public Guid Id { get; } = Guid.NewGuid();

    public int Row { get; }

    public int Seat { get; }

    public decimal BasePrice { get; }

    protected Ticket(int row, int seat, decimal basePrice) 
    {
        if (basePrice <= 0) 
            throw new ArgumentException("Ціна має бути більшою за нуль.");
        
        if (row <= 0 || seat <= 0) 
            throw new ArgumentException("Ряд та місце мають бути додатними числами.");

        Row = row;
        Seat = seat;
        BasePrice = basePrice;
    }

    public abstract decimal CalculateFinalPrice();
}

public class StandardTicket : Ticket 
{
    [JsonConstructor]
    public StandardTicket(int row, int seat, decimal basePrice) : base(row, seat, basePrice) { }
    
    public override decimal CalculateFinalPrice() => BasePrice;
}

public class VipTicket : Ticket 
{
    [JsonConstructor]
    public VipTicket(int row, int seat, decimal basePrice) : base(row, seat, basePrice) { }
    
    public override decimal CalculateFinalPrice() => BasePrice * 1.5m; 
}