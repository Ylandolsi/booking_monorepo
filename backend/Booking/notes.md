update PaidValue object in session

Use idempotency keys when calling external services (e.g., payment.Id) and retry policies for network errors.

Added CSRF protection for form-based authentication

Secure webhook

- Use HMAC per account (server-to-server only). Store the shared secret on your side and compare the HMAC from the request to the computed HMAC. Keep the secret out of any client-facing callback to prevent brute force.
- Add retry/failure handling for webhook delivery (exponential backoff, max attempts, idempotency keys).

Callback vs. Webhook

- Callback: gateway -> front-end -> backend (subject to client visibility).
- Webhook: gateway -> backend (server-to-server, preferred for confidential events).

Retry and idempotency

- Implement idempotency keys for webhook events to avoid duplicate processing.
- Retries: exponential backoff, jitter, and a dead-letter queue or alerting after max retries.

Concurrency, locks and migrations

- Review locking strategies (row-level vs. advisory locks) especially around migrations and concurrent operations.
- Consider optimistic concurrency where appropriate (row version/timestamp).

Integrations & tokens

- Test refresh token behavior against real expiration scenarios.
- Save tokens securely (encrypted at rest).
- Consider refactoring refresh token logic (single-source-of-truth, revoke flows).
- Manage auth info: store currentIp and currentUserAgent if needed for anomaly detection.

TODO / Roadmap

- Create wallet for each user by default ! 
- fix session price on frontend 
- fix availbility on front 
- Integrate with calendar; prevent switching to another account if already integrated.
- Review upcoming meetings and previous meetings/mentors.
- Simulation for the payment gateway.
- Fix availability / booking flows.
- ADD Mentor canot book with himself
- Admin dashboard.
- Manage auth metadata (currentIp, currentUserAgent).
- Toggle mentor activity to disable receiving meeting requests.
- Disable booking from profile if the user is not a mentor.
- Booking page: improve mobile version.
- Add fallback objects that contain defaults for missing fields.
- Configure EF Core domains and connection settings carefully.
- Ensure correct handling of profile picture URL storage and delivery.
- Fix notification delivery issues (investigate the linked report).
- for availability : dont show today's slots when time has passed  : example :today at 15 don show me slots from 9 to 14 
- Fix Reselliency
- Data model additions (suggested)
  Escrow
- change all getting services to this :  ( with scope)
-         using var scope = _serviceProvider.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>()
-         var httpClient = httpClientFactory.CreateClient();
- confirgure life span of koonect 
- test the global exception handler  

- handle this with UI :
  [13:56:57 ERR] Email yassine.landolsi@converty.shop  already assigned to someone else
  2025-08-24T13:56:57.246704927Z [13:56:57 ERR] Completed command IntegrateAccountCommand with error
  when integrating with an account alraedy integrated ! 

- COnfigure the expired for the google tokns GoogleTokenService and UsersModuleApi and .. 



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
