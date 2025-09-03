2025-08-30T10:45:00
is an ISO 8601 date-time format,
--
cd "c:\Users\USER\Desktop\me\booking_monorepo\backend\Booking"; dotnet test tests\IntegrationsTests\IntegrationsTests.csproj --filter "FullyQualifiedName~AvailabilityTests" --verbosity normal

----
MUST read DB 
https://www.linkedin.com/posts/ahmed-emad-abdelall_%D9%81%D9%8A-%D8%A7%D9%84%D9%85%D9%86%D8%AA%D9%88%D8%B1%D8%B4%D9%8A%D8%A8-%D9%81%D9%8A-%D8%A7%D9%84%D8%AC%D8%B2%D8%A1-%D8%A7%D9%84%D8%AE%D8%A7%D8%B5-%D8%A8%D8%A7%D9%84%D9%80-database-activity-7368965607087607810-qdrH?originalSubdomain=ae
---
Database Locks: Consider using SET LOCK_TIMEOUT for long-running queries
Memory Usage: Monitor memory consumption with large batch operations

Must read for async operations !!!

## https://www.reddit.com/r/dotnet/comments/1g9c8lu/looking_for_a_solution_for_async_processing_of/

## Test ressources :

https://antondevtips.com/blog/asp-net-core-integration-testing-best-practises
https://blog.markvincze.com/overriding-configuration-in-asp-net-core-integration-tests/

---

back to useContext and go in depth in that multiselect dialog

update PaidValue object in session

Use idempotency keys when calling external services (e.g., payment.Id) and retry policies for network errors.

Added CSRF protection for form-based authentication

Secure webhook

- Use HMAC per account (server-to-server only). Store the shared secret on your side and compare the HMAC from the request to the computed HMAC. Keep the secret out of any client-facing callback to prevent brute force.
- Add retry/failure handling for webhook delivery (exponential backoff, max attempts, idempotency keys).

Retry and idempotency

- Implement idempotency keys for webhook events to avoid duplicate processing.
- Retries: exponential backoff, jitter, and a dead-letter queue or alerting after max retries.

Concurrency, locks and migrations

- Review locking strategies (row-level vs. advisory locks) especially around migrations and concurrent operations.
- Consider optimistic concurrency where appropriate (row version/timestamp).
  // Concurrency check — prevents overwriting changes made by others
  if (!string.Equals(role.ConcurrencyStamp, model.ConcurrencyStamp, StringComparison.Ordinal))

Integrations & tokens

- Test refresh token behavior against real expiration scenarios.
- Save tokens securely (encrypted at rest).
- Consider refactoring refresh token logic (single-source-of-truth, revoke flows).
- Manage auth info: store currentIp and currentUserAgent if needed for anomaly detection.

TODO / Roadmap

(done but make sure its okay)

- for availability : dont show today's slots when time has passed : example :today at 15 don show me slots from 9 to 14

remove all logDebug !

cloudflare ?

configureAwait : false

await userRoleStore.IsInRoleAsync(user, normalizedRole, CancellationToken).ConfigureAwait(false)

all entity updaed add should be configured !

fix this aand timeout
services.AddHttpClient("KonnectClient", client =>
{
// TODO : make this more reselllient
/_
client.BaseAddress = new Uri(configuration["Konnect:ApiUrl"]);
#1#
client.BaseAddress = new Uri("https://api.sandbox.konnect.network/api/v2");
client.DefaultRequestHeaders.Add("x-api-key", configuration["Konnect:ApiKey"]);
client.Timeout = TimeSpan.FromSeconds(10);
})
.AddStandardResilienceHandler();_/

- add fallback for profile picture when showing session , front and back
- add timezone in the frontend for each request

-fix return url of google oauth from front : now it always return to home

- add method to retieve money : transfer balance to Konnect with fees 15%
- Fix availability / booking flows.
- Booking page: improve mobile version.
- Add fallback objects that contain defaults for missing fields.
- Configure EF Core domains and connection settings carefully and validators for endpoints
- Ensure correct handling of profile picture URL storage and delivery. watch milan video and cdn
- Fix Reselliency

- confirgure life span of koonect
- test the global exception handler
- COnfigure the expired for the google tokns GoogleTokenService and UsersModuleApi and ..

- remove private route from mentor/calendar or profile
  but any action needs to be logged in ( with google or email )
  ------ LATER
- pagination for the getPayouts and getSessiosn
- Toggle mentor activity to disable receiving meeting requests. for now its working by togglign days , so for future toggle it all
- add : naviagte to mentor or mentee when showing the meets
- previous meetings/mentors.
- maybe add tag of skills to the meeting to show it !
- Manage auth metadata (currentIp, currentUserAgent).
- eAdmin dashboard.
- add user can have solde

Notes

- Consider using a money/decimal type with fixed precision for amounts.
- Store audit metadata for sensitive actions (who, when, why).

Serialization guidance (C#)

- Use ReadFromJsonAsync<T> for known models (most efficient and type-safe).
  Example:
  ```csharp
  var data = await response.Content.ReadFromJsonAsync<MeData>();
  ```
- Use ReadFromJsonAsync<JsonElement> for dynamic or unknown structures.
  Example:
  ```csharp
  var json = await response.Content.ReadFromJsonAsync<JsonElement>();
  var value = json.GetProperty("key").GetString();
  ```
- Use ReadAsStreamAsync() for large responses to reduce memory usage.
  Example:
  ```csharp
  using var stream = await response.Content.ReadAsStreamAsync();
  var data = await JsonSerializer.DeserializeAsync<MyData>(stream);
  ```

Libraries / misc

- Consider NodaTime for reliable time handling across zones and DST.
- Read about locks and concurrency in EF Core and database docs before applying migration or heavy concurrency changes.
- Aim for clear telemetry and alerting around webhook failures and retries.
- Keep secrets and HMAC keys out of client flows and public callbacks.
- Maintain a small simulation/test harness for payment gateway integration.
- Add monitoring for token refresh failures and failed webhook deliveries.

## outbox

✅ Best Practices for Multiple Outboxes

One Outbox per DB (per bounded context/module).

One Processor per Outbox (can run multiple instances for scaling, but with row-level locks).

Consumers are idempotent (must accept duplicates).

Events are immutable (never modify, only append).

Outbox cleanup job (archive or delete dispatched events after N days).

Running multiple outbox jobs across separate modules (each with its own schema) does not cause issues — in fact, it’s the recommended approach.
The only thing to watch for is concurrency inside the same outbox table, but that’s solved with FOR UPDATE SKIP LOCKED or equivalent.

---

## learn azure cosmos db

The write path. We built a daemon that would select log entries that were older than two weeks, copy them into a file in S3, and delete the rows from the database.
The read path. Our web application would look at the time period of the logs being queried and dynamically decide to query either the database or S3.

---

## In AWS terms, you can use a SNS topic to send messages to any subscriber (if a topic has many subscribers, each will receive the message) and SQS to queue messages (only the first\* puller will receive the message)

hangfire for in memoryy events

That’s what I use for simpler, smaller messaging needs. And bonus, it can be transactionally consistent with the rest of your business data!
Your problem description is textbook use-case for Hangfire. Scaling Hangfire is as simple as having all your jobs shared or part of a separate project, hosted independent from your main app (ideally). Or... just deploy your main app to multiple machines. Might seem lazy, but let monoliths do what monoliths are best at..

---

DB as queue instead of rabitmq :
https://dagster.io/blog/skip-kafka-use-postgres-message-queue
https://en.m.wikipedia.org/wiki/Change_data_capture

It's pretty trivial to implement a good enough queue manually in a traditional database.

For smaller apps that aren't pushing mega-amounts of messages, this my preference if it means reducing my external dependency count.

I have done this, but by the time you have multiple servers interacting with the queue for failover/scalability and have dealt with edge cases relating to that I wouldn't call it trivial. I think in the future I would certainly take a hard look at something like rabbitmq rather than roll my own.

---

event : https://github.com/rebus-org/Rebus is pretty nice.
https://wolverine.netlify.app/ is nice

---

https://learn.microsoft.com/en-us/azure/storage/queues/storage-queues-introduction
https://learn.microsoft.com/en-us/azure/service-bus-messaging/service-bus-dotnet-multi-tier-app-using-service-bus-queues

--
deployment :
f you’re self hosting a web app on Windows and IIS then you need to tweak the IIS application pool to be always on. And you can can use a background task for the processing. Or you can deploy the worker as Windows Service.

--
https://github.com/zeromq/netmq

Session Created (Booked)
↓
Wallet Check & Deduction
↓
Payment Required?
├─ No → Confirm Session + Create Escrow
└─ Yes → Create Payment + Set WaitingForPayment
↓
Konnect Payment Gateway
↓
Webhook Received → Payment Completed
↓
Session Confirmed + Meet Link + Escrow Created

---

DONE :

- Integrate with calendar; prevent switching to another account if already integrated.
  and handle this UI :
  - handle this with UI :
    [13:56:57 ERR] Email yassine.landolsi@converty.shop already assigned to someone else
    2025-08-24T13:56:57.246704927Z [13:56:57 ERR] Completed command IntegrateAccountCommand with error
    when integrating with an account alraedy integrated !

## Future Enhancements

- [ ] Multiple session duration options

```
ts

  useEffect(() => {
    // Create a function to handle wheel events on the dialog/drawer content
    const handleWheel = (e: WheelEvent) => {
      if (hasOpenPopover) {
        // If a popover is open inside the dialog, check if it's part of a command list
        const target = e.target as HTMLElement;
        const popoverElement = target.closest('[data-state="open"]'); // Find the open popover
        const isCommandContent =
          popoverElement &&
          popoverElement.querySelector('.max-h-\\[200px\\]')?.contains(target); // Escape the class name

        if (isCommandContent) {
          // Let the command list handle its own scrolling
          // stop the scroll of the dialog and let the select handle its scroll !
          e.stopPropagation();
        }
      }
    };

    // Add the event listener when the dialog is open
    if (open) {
      document.addEventListener('wheel', handleWheel, { capture: true });
    }

    return () => {
      document.removeEventListener('wheel', handleWheel, { capture: true });
    };
  }, [open, hasOpenPopover]);
```

---

## --

Authored cautious SQL scripts to avoid resource locking and minimize performance impact in production. Prepared and monitored migration jobs with detailed logging to detect failures early, and completed migration with zero downtime and <2% CPU overhead during peak loads

---

Azure Key Vault for managing secrets (API keys, tokens) in the card lifecycle process, reducing exposure of sensitive data.

---

policy management UI react ,
