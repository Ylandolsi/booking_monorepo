using Booking.Common.Endpoints;
using Booking.Common.Messaging;
using Booking.Common.Results;
using Booking.Modules.Mentorships.Domain.Entities;
using Booking.Modules.Mentorships.Features.Payment;
using Booking.Modules.Mentorships.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Mentorships.Features.Payout.Admin.Approve.Webhook;

public class PayoutWebhook : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(MentorshipEndpoints.Payment.Admin.WebhookPayout, async (
            [FromQuery] string payment_ref,
            ICommandHandler<PayoutWebhookCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new PayoutWebhookCommand(payment_ref);
            var result = await handler.Handle(command, cancellationToken);
            return result.Match(() => Results.Ok(), CustomResults.Problem);
        });
    }
}

public record PayoutWebhookCommand(string PaymentRef) : ICommand;

public class PayoutWebhookCommandHandler(
    MentorshipsDbContext dbContext,
    KonnectService konnectService,
    ILogger<PayoutWebhookCommandHandler> logger) : ICommandHandler<PayoutWebhookCommand>
{
    public async Task<Result> Handle(PayoutWebhookCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Webhook received (Admin Approve payout) with paymentRef : {ref}", command.PaymentRef);

        var payout =
            await dbContext.Payouts.FirstOrDefaultAsync(p => p.PaymentRef == command.PaymentRef, cancellationToken);

        if (payout is null)
        {
            logger.LogError("Webhook ( admin approve payout ) paymentRef :{ref}, dosent exists in payout table",
                command.PaymentRef);
            return Result.Failure(Error.None);
        }

        if (payout.Status == PayoutStatus.Completed)
        {
            logger.LogError(
                "Webhook ( admin approve payout ) trying to handle a payout (ref = {ref}) already completed ",
                command.PaymentRef);

            return Result.Failure(Error.None);
        }

        var paymentDetails = await konnectService.GetPaymentDetails(command.PaymentRef);
        if (paymentDetails.IsFailure)
        {
            logger.LogError(paymentDetails.Error.Description);
            return Result.Failure(paymentDetails.Error);
        }

        payout.Complete();
        await dbContext.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}