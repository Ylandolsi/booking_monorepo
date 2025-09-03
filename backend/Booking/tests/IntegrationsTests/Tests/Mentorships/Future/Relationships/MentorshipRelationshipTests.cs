/*📌 Request Mentorship
   When authenticated user sends valid request, Should create mentorship request
   
   When unauthenticated user sends request, Should return 401 Unauthorized
   
   When MentorSlug is empty, Should return 400 Bad Request
   
   When Message is empty, Should return 400 Bad Request
   
   When Message exceeds maximum length, Should return 400 Bad Request
   
   When mentor does not exist, Should return 404 Not Found
   
   When user tries to request themselves as mentor, Should return 400 Bad Request
   
   When mentorship is already requested, Should return 409 Conflict
   
   When requesting different mentors, Should allow multiple requests
   
   📌 Accept Mentorship
   When authenticated mentor accepts existing request, Should accept request
   
   When unauthenticated user accepts, Should return 401 Unauthorized
   
   When relationship does not exist, Should return 404 Not Found
   
   When user is not the mentor, Should return 403 Forbidden
   
   When relationship is already accepted, Should return 400 Bad Request
   
   📌 Reject Mentorship
   When authenticated mentor rejects existing request, Should reject request
   
   When unauthenticated user rejects, Should return 401 Unauthorized
   
   When relationship does not exist, Should return 404 Not Found
   
   When user is not the mentor, Should return 403 Forbidden
   
   When relationship was already processed (accepted or rejected), Should return 400 Bad Request
   
   📌 End Mentorship
   When authenticated user ends active relationship, Should end relationship
   
   When unauthenticated user ends, Should return 401 Unauthorized
   
   When relationship does not exist, Should return 404 Not Found
   
   When user is not part of relationship, Should return 403 Forbidden
   
   When relationship is not active, Should return 400 Bad Request
   
   When relationship was already ended, Should return 400 Bad Request
   
   📌 Get Mentorship Relationships
   When authenticated user retrieves relationships, Should return relationships
   
   When unauthenticated user retrieves relationships, Should return 401 Unauthorized
   
   When user has no relationships, Should return empty list
   
   📌 Full Workflow
   When mentee requests, mentor accepts, mentee checks, and ends relationship, Should complete happy path successfully*/