namespace CinemaPeak.Domain.Strategies;

public interface IDiscountStrategy
{
    decimal ApplyDiscount(decimal originalPrice);
}

public class NoDiscount : IDiscountStrategy
{
    public decimal ApplyDiscount(decimal originalPrice)
    {
        return originalPrice; 
    }
}

public class StudentDiscount : IDiscountStrategy
{
    public decimal ApplyDiscount(decimal originalPrice)
    {
        return originalPrice * 0.8m; 
    }
}