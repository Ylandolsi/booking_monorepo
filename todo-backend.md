- [ ] verify the backend cron job for approved payout and not complete back to pending ! ( test it ) !
- [ ] Add resiliency to background jobs / Konnect
- [ ] Handle when a user integrates with an account already integrated / or email integrated by another account
- [ ] secure become mentor endpoint ( now only secured from front end)
- [ ] Configure EF Core domains and connection settings carefully and validators for endpoints
- [ ] mentor statistic : current month revenue / annual revenue : dashboard
- [ ] test payment of a session when the user already has some balance
- [ ] remove unnecessary payment test wallet and repace with our backend wallet
- [ ] test admin get histgory with different variations ...
- [ ] fix return url of google oauth from front : now it always return to home
- [ ] Ensure correct handling of profile picture URL storage and delivery. watch milan video and cdn
- [ ] configure life span of konnect and add timeout
- [ ] test the global exception handler
- [ ] remove private route from mentor/calendar or profile but any action needs to be logged in ( with google or email )
- [ ] remove all logDebug !
- [ ] add fallback objects that contain defaults for missing fields.
- [ ] Aim for clear telemetry and alerting around webhook failures and retries.
- [ ] make the price entity consistent ? and duration

---

Compiling a query which loads related collections for more than one collection navigation, either via 'Include' or through projection, but no 'QuerySplittingBehavior' has been configured. By default, Entity Framework will use 'QuerySplittingBehavior.SingleQuery', which can potentially result in slow query performance. See https://go.microsoft.com/fwlink/?linkid=2134277 for more information. To identify the query that's triggering this warning call 'ConfigureWarnings(w => w.Throw(RelationalEventId.MultipleCollectionIncludeWarning))'.
ConnectionId
0HNFRDHIKEBB3
CorrelationId
0HNFRDHIKEBB3:00000013
EventId.Id
20504
EventId.Name
Microsoft.EntityFrameworkCore.Query.MultipleCollectionIncludeWarning
RequestId
0HNFRDHIKEBB3:00000013
RequestPath
/api/products/s/create
SourceContext
Microsoft.EntityFrameworkCore.Query

another error

```bash
Npgsql.NpgsqlOperationInProgressException (0x80004005): A command is already in progress: SELECT s.id, s.created_at, s.day_id, s.day_of_week, s.is_active, s.session_product_id, s.session_product_slug, s.time_zone_id, s.updated_at, s.end_time, s.start_time
FROM catalog.session_availabilities AS s
   at Npgsql.ThrowHelper.ThrowNpgsqlOperationInProgressException(NpgsqlCommand command)
   at Npgsql.Internal.NpgsqlConnector.<StartUserAction>g__DoStartUserAction|282_0(ConnectorState newState, NpgsqlCommand command, CancellationToken cancellationToken, Boolean attemptPgCancellation)
   at Npgsql.Internal.NpgsqlConnector.StartUserAction(ConnectorState newState, NpgsqlCommand command, CancellationToken cancellationToken, Boolean attemptPgCancellation)
   at Npgsql.Internal.NpgsqlConnector.StartUserAction(CancellationToken cancellationToken, Boolean attemptPgCancellation)
   at Npgsql.NpgsqlTransaction.Commit(Boolean async, CancellationToken cancellationToken)
   at Microsoft.EntityFrameworkCore.Storage.RelationalTransaction.CommitAsync(CancellationToken cancellationToken)
   at Microsoft.EntityFrameworkCore.Storage.RelationalTransaction.CommitAsync(CancellationToken cancellationToken)
   at Booking.Modules.Catalog.Persistence.UnitOfWork.CommitTransactionAsync(CancellationToken cancellationToken)
   at Booking.Modules.Catalog.Features.Products.Sessions.Private.CreateSessionProduct.CreateSessionProductHandler.Handle(PostSessionProductCommand command, CancellationToken cancellationToken)
   could u identify why is this happening ?
```

### Later

- [ ] type of middleware / filters : from all endpoints vs specific
- [ ] why later ? ( already handled by google ):send email verification of session / reminder before some time
- [ ] why .net app could not communicate with itself but with internal exposed port from docker can 8080
- [ ] what is krestel/ipv4/ipv6
- [ ] add activity
- [ ] add realtime communication
- [ ] previous meetings/mentors.
- [ ] maybe add tag of skills to the meeting to show it !
- [ ]Manage auth metadata (currentIp, currentUserAgent).
- [ ] configureAwait : false
- [ ] cloudFlare
- [ ] pagination for the getPayouts and getSession
- [ ] Toggle mentor activity to disable receiving meeting requests. for now its working by toggling days , so for future toggle it all
- [ ] add fallback for profile picture when showing session , front and back
- [ ] add timezone in the frontend for each request
- [ ] Multiple session duration options
- [ ] make the app extensible and can replace any payment gateway
      services.TryAddSingleton(typeof(IUserIdProvider), typeof(DefaultUserIdProvider));

// !!!
.WithMetadata(new IgnoreAntiforgeryTokenAttribute());
and app.UseAntiforgery()
