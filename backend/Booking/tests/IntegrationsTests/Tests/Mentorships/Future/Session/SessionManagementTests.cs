/*Session Management Test Cases
   When authenticated user books a session with valid data, Should create the session successfully.
   
   When unauthenticated user tries to book a session, Should return 401 Unauthorized.
   
   When user books a session with empty mentor slug, Should return 400 Bad Request.
   
   When user books a session with scheduled time in the past, Should return 400 Bad Request.
   
   When user books a session with invalid duration (0 or negative), Should return 400 Bad Request.
   
   When user books a session for a non-existent mentor, Should return 404 Not Found.
   
   When user books a session with a time slot that overlaps with an existing session, Should return 409 Conflict.
   
   When user books a session with valid durations that are multiples of 30 minutes (30, 60, 90, 120), Should accept the session successfully.
   
   When user books a session without providing optional notes, Should create the session successfully.
   
   When user books consecutive sessions ignoring mentor buffer time, Should return 409 Conflict.
   
   When authenticated user cancels an existing session, Should cancel the session successfully.
   
   When unauthenticated user tries to cancel a session, Should return 401 Unauthorized.
   
   When user cancels a non-existent session, Should return 404 Not Found.
   
   When user tries to cancel a session already cancelled, Should return 400 Bad Request.
   
   When user tries to cancel a session that already occurred in the past, Should return 400 Bad Request.
   
   When authenticated user requests an existing session by ID, Should return the session successfully.
   
   When unauthenticated user tries to get a session, Should return 401 Unauthorized.
   
   When user requests a non-existent session, Should return 404 Not Found.
   
   When user is unauthorized to view a session, Should return 403 Forbidden.
   
   When authenticated mentor requests their sessions, Should return the list of sessions successfully.
   
   When unauthenticated user tries to get mentor sessions, Should return 401 Unauthorized.
   
   When user who is not a mentor tries to get mentor sessions, Should return 403 Forbidden.
   
   When authenticated mentee requests their sessions, Should return the list of sessions successfully.
   
   When unauthenticated user tries to get mentee sessions, Should return 401 Unauthorized.*/