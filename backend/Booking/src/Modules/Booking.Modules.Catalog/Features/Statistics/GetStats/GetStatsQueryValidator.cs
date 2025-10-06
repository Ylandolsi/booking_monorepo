using FluentValidation;

namespace Booking.Modules.Catalog.Features.Statistics.GetStats;

public class GetStatsQueryValidator : AbstractValidator<GetStatsQuery>
{
    public GetStatsQueryValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("User ID must be greater than 0");

        RuleFor(x => x.Type)
            .Must(type => type == null || 
                          type == "revenue" || 
                          type == "visitors" || 
                          type == "customers" || 
                          type == "all")
            .WithMessage("Type must be one of: revenue, visitors, customers, all");

        RuleFor(x => x.StartsAt)
            .LessThanOrEqualTo(x => x.EndsAt ?? DateTime.UtcNow)
            .When(x => x.StartsAt.HasValue && x.EndsAt.HasValue)
            .WithMessage("Start date must be before or equal to end date");

        RuleFor(x => x.EndsAt)
            .LessThanOrEqualTo(DateTime.UtcNow.AddDays(1))
            .When(x => x.EndsAt.HasValue)
            .WithMessage("End date cannot be in the future");

        RuleFor(x => x)
            .Must(x => !x.StartsAt.HasValue || !x.EndsAt.HasValue || 
                      (x.EndsAt.Value - x.StartsAt.Value).TotalDays <= 365)
            .WithMessage("Date range cannot exceed 365 days");
    }
}
