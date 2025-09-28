﻿using Booking.Common.Messaging;
using Booking.Common.Results;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace Booking.Common.Behaviors;

public static class LoggingDecorator
{
    public sealed class CommandHandler<TCommand, TResponse>(
        ICommandHandler<TCommand, TResponse> innerHandler,
        ILogger<CommandHandler<TCommand, TResponse>> logger)
        : ICommandHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
    {
        public async Task<Result<TResponse>> Handle(TCommand command, CancellationToken cancellationToken)
        {
            var commandName = typeof(TCommand).Name;

            logger.LogInformation("Processing command {Command}", commandName);

            var result = await innerHandler.Handle(command, cancellationToken);

            if (result.IsSuccess)
                logger.LogInformation("Completed command {Command}", commandName);
            else
                // Push the error details into the log context
                using (LogContext.PushProperty("Error", result.Error, true))
                {
                    logger.LogError("Completed command {Command} with error", commandName);
                }

            /*
                {
                        "timestamp": "2024-06-04T10:30:01Z",
                        "level": "Error",
                        "message": "Completed command LoginUserCommand with error",
                        "properties": {
                            "Command": "LoginUserCommand", // added automatically from {Command}
                            "CorrelationId": "req-123-456",    // ← From middleware
                            "Error": {
                                "Code": "Auth.InvalidCredentials",
                                "Description": "Invalid email or password"
                            }
                        }
                }
                */
            return result;
        }
    }

    public sealed class CommandBaseHandler<TCommand>(
        ICommandHandler<TCommand> innerHandler,
        ILogger<CommandBaseHandler<TCommand>> logger)
        : ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        public async Task<Result> Handle(TCommand command, CancellationToken cancellationToken)
        {
            var commandName = typeof(TCommand).Name;

            logger.LogInformation("Processing command {Command}", commandName);

            var result = await innerHandler.Handle(command, cancellationToken);

            if (result.IsSuccess)
                logger.LogInformation("Completed command {Command}", commandName);
            else
                using (LogContext.PushProperty("Error", result.Error, true))
                {
                    logger.LogError("Completed command {Command} with error", commandName);
                }

            return result;
        }
    }

    public sealed class QueryHandler<TQuery, TResponse>(
        IQueryHandler<TQuery, TResponse> innerHandler,
        ILogger<QueryHandler<TQuery, TResponse>> logger)
        : IQueryHandler<TQuery, TResponse>
        where TQuery : IQuery<TResponse>
    {
        public async Task<Result<TResponse>> Handle(TQuery query, CancellationToken cancellationToken)
        {
            var queryName = typeof(TQuery).Name;

            logger.LogInformation("Processing query {Query}", queryName);

            var result = await innerHandler.Handle(query, cancellationToken);

            if (result.IsSuccess)
                logger.LogInformation("Completed query {Query}", queryName);
            else
                using (LogContext.PushProperty("Error", result.Error, true))
                {
                    logger.LogError("Completed query {Query} with error", queryName);
                }

            return result;
        }
    }
}