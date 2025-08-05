using Booking.Common;
using Booking.Common.Results;

namespace Booking.Modules.Mentorships.Domain.ValueObjects;

public class Price : ValueObject
{
    public decimal Amount { get; private set; }
    public string Currency { get; private set; }

    private Price() { }

    public Price(decimal amount, string currency = "USD")
    {
        if (amount < 0)
            throw new ArgumentException("Price cannot be negative", nameof(amount));

        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException("Currency cannot be empty", nameof(currency));

        Amount = amount;
        Currency = currency.ToUpperInvariant();
    }

    public static Result<Price> Create(decimal amount, string currency = "USD")
    {
        if (amount < 0)
            return Result.Failure<Price>(Error.Problem("Price.InvalidAmount", "Price cannot be negative"));

        if (string.IsNullOrWhiteSpace(currency))
            return Result.Failure<Price>(Error.Problem("Price.InvalidCurrency", "Currency cannot be empty"));

        return Result.Success(new Price(amount, currency));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }

    public override string ToString()
    {
        return $"{Amount:C} {Currency}";
    }
}
