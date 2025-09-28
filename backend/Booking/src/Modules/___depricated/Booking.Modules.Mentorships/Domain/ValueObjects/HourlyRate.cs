using Booking.Common;
using Booking.Common.Results;

namespace Booking.Modules.Mentorships.Domain.ValueObjects;

public class HourlyRate : ValueObject
{
    public decimal Amount { get; private set; }
    public string Currency { get; private set; }

    private HourlyRate() { }

    public HourlyRate(decimal amount, string currency = "USD")
    {
        if (amount <= 0)
            throw new ArgumentException("Hourly rate must be greater than zero", nameof(amount));

        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException("Currency cannot be empty", nameof(currency));

        Amount = amount;
        Currency = currency.ToUpperInvariant();
    }

    public static Result<HourlyRate> Create(decimal amount, string currency = "USD")
    {
        if (amount <= 0)
            return Result.Failure<HourlyRate>(Error.Problem("HourlyRate.InvalidAmount", "Hourly rate must be greater than zero"));

        if (string.IsNullOrWhiteSpace(currency))
            return Result.Failure<HourlyRate>(Error.Problem("HourlyRate.InvalidCurrency", "Currency cannot be empty"));

        return Result.Success(new HourlyRate(amount, currency));
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
