Added CSRF protection for form-based authentication

secure webhook by :  
HMAC (Hash-based Message Authentication Code) ,every account has one ,
then i compare hmac in request with my account's hmac
and hmac is existing only in webhook (server to server ) not in callback so no one can extract this hmac from paymob request and try to brut force this hash

add retry failure in webhook

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