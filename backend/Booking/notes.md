2025-08-30T10:45:00
is an ISO 8601 date-time format,
--
cd "c:\Users\USER\Desktop\me\booking_monorepo\backend\Booking"; dotnet test tests\IntegrationsTests\IntegrationsTests.csproj --filter "FullyQualifiedName~AvailabilityTests" --verbosity normal
---

## --

Authored cautious SQL scripts to avoid resource locking and minimize performance impact in production. Prepared and monitored migration jobs with detailed logging to detect failures early, and completed migration with zero downtime and <2% CPU overhead during peak loads

---

Azure Key Vault for managing secrets (API keys, tokens) in the card lifecycle process, reducing exposure of sensitive data.

---

when bakcend want to send error after redirection to front :
we can specify it as query param ?error=  


---

take a look at biome : alternative for pretteir & eslint 

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

Libraries / misc

- Consider NodaTime for reliable time handling across zones and DST.
- Read about locks and concurrency in EF Core and database docs before applying migration or heavy concurrency changes.
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

## Future Enhancements


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

