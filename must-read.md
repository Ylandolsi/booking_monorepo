# MUST Read: Database

- interceptors in ef core

- [LinkedIn Post on Database Mentorship (Arabic)](https://www.linkedin.com/posts/ahmed-emad-abdelall_%D9%81%D9%8A-%D8%A7%D9%84%D9%85%D9%86%D8%AA%D9%88%D8%B1%D8%B4%D9%8A%D8%A8-%D9%81%D9%8A-%D8%A7%D9%84%D8%AC%D8%B2%D8%A1-%D8%A7%D9%84%D8%AE%D8%A7%D8%B5-%D8%A8%D8%A7%D9%84%D9%80-database-activity-7368965607087607810-qdrH?originalSubdomain=ae) -[db migration add new coloumn](https://www.linkedin.com/posts/muhammedatif_%D8%B3%D8%A4%D8%A7%D9%84-%D8%A7%D9%86%D8%AA%D8%B1%D9%81%D9%8A%D9%88-%D9%84%D9%88-%D8%A7%D8%AD%D9%86%D8%A7-%D8%B9%D9%86%D8%AF%D9%86%D8%A7-table-%D9%83%D8%A8%D9%8A%D8%B1-%D8%AC%D8%AF%D8%A7-activity-7371112657350897664-S83B?utm_source=share&utm_medium=member_ios&rcm=ACoAAEUmzPsBllTvX8bXt7YIrmvtM-d7ZO0Tmxg)

---

- [notificaiton] (https://www.linkedin.com/posts/ahmed-emad-abdelall_%D8%B4%D9%83%D8%B1%D8%A7-%D9%84%D9%83%D9%84-%D8%AA%D8%B9%D9%84%D9%8A%D9%82%D8%A7%D8%AA%D9%83%D9%85-%D9%88%D8%A7%D8%AC%D8%A7%D8%A8%D8%AA%D9%83%D9%85-%D8%B9%D9%84%D9%8A-%D8%A7%D9%84%D8%B3%D9%88%D8%A7%D9%84-activity-7364981642525847555-hs0f?utm_source=share&utm_medium=member_desktop&rcm=ACoAAEUmzPsBllTvX8bXt7YIrmvtM-d7ZO0Tmxg)
- [notifcationqueue](https://www.linkedin.com/feed/update/urn:li:activity:7369750790292303872/)

---

https://www.linkedin.com/posts/tannika-majumder-424a5040_i-gave-this-problem-to-30-candidates-in-mock-activity-7369714329434189825-e2Eb?utm_source=share&utm_medium=member_desktop&rcm=ACoAAEUmzPsBllTvX8bXt7YIrmvtM-d7ZO0Tmxg

---

### .net

- [IHostEnv] (https://andrewlock.net/ihostingenvironment-vs-ihost-environment-obsolete-types-in-net-core-3/)

## .net abstracton https://www.milanjovanovic.tech/blog/the-real-cost-of-abstractions-in-dotnet?utm_source=X&utm_medium=social&utm_campaign=01.09.2025

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
- [check this type of init ](https://github.com/iayti/CleanArchitecture/blob/master/tests/CleanArchitecture.Application.IntegrationTests/Testing.cs)

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
