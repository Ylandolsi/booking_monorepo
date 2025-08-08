/*When authenticated user adds a review with valid data, Should create the review successfully.
   
   When unauthenticated user tries to add a review, Should return 401 Unauthorized.
   
   When user adds a review with invalid rating (0, -1, 6, 10), Should return 400 Bad Request.
   
   When user adds a review with empty mentor slug, Should return 400 Bad Request.
   
   When user adds a review with empty comment, Should return 400 Bad Request.
   
   When user adds a review with comment exceeding max length, Should return 400 Bad Request.
   
   When user adds a review for a non-existent mentor, Should return 404 Not Found.
   
   When user adds a review for a non-existent session, Should return 404 Not Found.
   
   When user adds a review for a session that is not completed yet, Should return 400 Bad Request.
   
   When user adds a duplicate review for the same session, Should return 409 Conflict.
   
   When authenticated user updates an existing review with valid data, Should update the review successfully.
   
   When unauthenticated user tries to update a review, Should return 401 Unauthorized.
   
   When user updates a non-existent review, Should return 404 Not Found.
   
   When user who does not own the review tries to update it, Should return 403 Forbidden.
   
   When user updates a review with invalid rating, Should return 400 Bad Request.
   
   When authenticated user deletes an existing review, Should delete the review successfully.
   
   When unauthenticated user tries to delete a review, Should return 401 Unauthorized.
   
   When user tries to delete a non-existent review, Should return 404 Not Found.
   
   When user who does not own the review tries to delete it, Should return 403 Forbidden.
   
   When user requests reviews for an existing mentor, Should return list of reviews successfully.
   
   When user requests reviews for a non-existent mentor, Should return 404 Not Found.
   
   When user requests reviews for a mentor with no reviews, Should return an empty list.
   
   When user requests reviews for a mentor with multiple reviews, Should return reviews ordered descending by creation date.
   
   When user requests reviews, Should include reviewer information in the response.
   
   When user adds reviews with valid ratings between 1 and 5, Should accept all ratings successfully.
   
   When user adds review providing only required fields (without optional comment), Should create review successfully.*/