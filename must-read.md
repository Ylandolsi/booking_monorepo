# MUST Read: Database

- [LinkedIn Post on Database Mentorship (Arabic)](https://www.linkedin.com/posts/ahmed-emad-abdelall_%D9%81%D9%8A-%D8%A7%D9%84%D9%85%D9%86%D8%AA%D9%88%D8%B1%D8%B4%D9%8A%D8%A8-%D9%81%D9%8A-%D8%A7%D9%84%D8%AC%D8%B2%D8%A1-%D8%A7%D9%84%D8%AE%D8%A7%D8%B5-%D8%A8%D8%A7%D9%84%D9%80-database-activity-7368965607087607810-qdrH?originalSubdomain=ae)

---

**Database Locks:**  
Consider using `SET LOCK_TIMEOUT` for long-running queries.

**Memory Usage:**  
Monitor memory consumption with large batch operations.

---

## Must Read for Async Operations

- [Reddit: Solution for Async Processing](https://www.reddit.com/r/dotnet/comments/1g9c8lu/looking_for_a_solution_for_async_processing_of/)

---

## Test Resources

- [ASP\.NET Core Integration Testing Best Practices](https://antondevtips.com/blog/asp-net-core-integration-testing-best-practises)
- [Overriding Configuration in ASP\.NET Core Integration Tests](https://blog.markvincze.com/overriding-configuration-in-asp-net-core-integration-tests/)

---

---

## DB as queue instead of rabitmq :
- [read1](https://dagster.io/blog/skip-kafka-use-postgres-message-queue)
- [read2](https://en.m.wikipedia.org/wiki/Change_data_capture)

It's pretty trivial to implement a good enough queue manually in a traditional database.
For smaller apps that aren't pushing mega-amounts of messages, this my preference if it means reducing my external dependency count.
I have done this, but by the time you have multiple servers interacting with the queue for failover/scalability and have dealt with edge cases relating to that I wouldn't call it trivial. I think in the future I would certainly take a hard look at something like rabbitmq rather than roll my own.

---

event libraries :
- https://github.com/rebus-org/Rebus is pretty nice.
- https://wolverine.netlify.app/ is nice


---


https://learn.microsoft.com/en-us/azure/storage/queues/storage-queues-introduction
https://learn.microsoft.com/en-us/azure/service-bus-messaging/service-bus-dotnet-multi-tier-app-using-service-bus-queues

---

### deployment :
    if you’re self hosting a web app on Windows and IIS then you need to tweak the IIS application pool to be always on. And you can can use a background task for the processing. Or you can deploy the worker as Windows Service.

---
https://github.com/zeromq/netmq
