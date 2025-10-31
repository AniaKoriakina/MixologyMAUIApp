namespace Mixology.Core.ValueObjects;

public record RatingValue
{
    public int Value { get; }
    
    public RatingValue(int value)
    {
        if (Value is < 1 or > 5)
            throw new ArgumentOutOfRangeException(nameof(Value), "Рейтинг должен быть от 1 до 5");
        
        Value = value;
    }
}