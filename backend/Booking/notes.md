Added CSRF protection for form-based authentication

secure webhook by :  
HMAC (Hash-based Message Authentication Code) ,every account has one ,
then i compare hmac in request with my account's hmac
and hmac is existing only in webhook (server to server ) not in callback so no one can extract this hmac from paymob request and try to brut force this hash

add retry failure in webhook

---
TODO : 
- intgerate with calendar  ( avoid user alraedy integrated with another account to switch it !)
- need to test refresh token with real behavior after expiration !! 
- review next meets 
- review previous meets/mentors 
- simulation for the gateway
- Fix availability / booking 
- Admin dashboard 
- save tokens 
- refactor refresh tokens 
- manage auth ( string? currentIp,string? currentUserAgent ) 
- toggle mentor activity  , to disable getting meets 
     
send notif problem: https://www.linkedin.com/posts/mohamed--thabet_%D8%A3%D9%88%D9%84-%D8%A8%D9%88%D8%B3%D8%AA-%D9%81%D9%8A-%D8%B3%D9%84%D8%B3%D9%84%D8%A9-%D9%85%D8%B4%D8%A7%D9%83%D9%84-backend-%D9%85%D9%86-%D8%A3%D8%B1%D8%B6-activity-7361008063882891264-rpyW?utm_source=share&utm_medium=member_desktop&rcm=ACoAAEUmzPsBllTvX8bXt7YIrmvtM-d7ZO0Tmxg                                        



--
callback vs webhook : callback : gatway -> front -> backend ->
-----
read about locks more , especially in migrations 
concurrency and 

add booking :

Escrow

escrow_id (PK)
booking_id (FK to Bookings)
diamond_amount (decimal)
status (held, released, refunded)
created_at, updated_at


Disputes

dispute_id (PK)
booking_id (FK to Bookings)
raised_by (mentee/mentor)
reason (text)
status (open, resolved, closed)
resolution (refund, release, partial, ban)
created_at, updated_at


Feedback

feedback_id (PK)
booking_id (FK to Bookings)
mentee_id (FK to Users)
rating (integer, e.g., 1-5)
comment (text)
created_at


Transactions

transaction_id (PK)
user_id (FK to Users)
escrow_id (FK to Escrow, nullable)
amount (decimal)
type (deduction, refund, release)
status (pending, completed, failed)
created_at


So your model looks something like:

Day (e.g. Monday, Tuesday…)

TimeOnly (e.g. 14:00)

TimeZoneId (e.g. "Africa/Tunis")

This describes availability, not a concrete date.
To actually book a slot, you need to combine:

The chosen date (e.g. 2025-08-25 → Monday)

The saved time (e.g. 14:00)

The mentor’s time zone

Then convert that into an Instant so it’s globally consistent.

Example with NodaTime
using NodaTime;

public Instant ToInstant(DateOnly date, TimeOnly time, string timeZoneId)
{
// 1. Convert DateOnly + TimeOnly into LocalDateTime
var localDateTime = new LocalDateTime(
date.Year, date.Month, date.Day,
time.Hour, time.Minute, time.Second);

    // 2. Get the time zone
    var tz = DateTimeZoneProviders.Tzdb[timeZoneId];

    // 3. Map to a ZonedDateTime and then to Instant
    var zoned = tz.AtStrictly(localDateTime); // or AtLeniently if you want DST-safe fallback
    return zoned.ToInstant();
}


Usage:

var date = new DateOnly(2025, 8, 25); // chosen Monday
var time = new TimeOnly(14, 0);       // availability
var timeZone = "Africa/Tunis";

var instant = ToInstant(date, time, timeZone);
// Save instant to DB when a booking happens

Why this works

You only store TimeOnly + TimeZoneId + DayOfWeek for availability.

When a mentee books, you get the actual DateOnly of that week and combine them.

Convert once to Instant → store it in DB as the actual booking time.

Clients in different time zones just convert back from Instant → their local time zone.