namespace CinemaPeak.Domain.Strategies;

public interface IDiscountStrategy { decimal Apply(decimal price); }

public class StudentDiscount : IDiscountStrategy { 
    public decimal Apply(decimal price) => price * 0.8m; 
}

public class NoDiscount : IDiscountStrategy { 
    public decimal Apply(decimal price) => price; 
}