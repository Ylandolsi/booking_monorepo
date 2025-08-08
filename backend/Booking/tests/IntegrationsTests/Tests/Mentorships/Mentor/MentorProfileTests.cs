/*When Mentor updates profile with valid data, Should save changes and return updated profile
   
   When Mentor updates profile with invalid data, Should fail with 400 Bad Request
   
   When Mentor tries to update profile without authentication, Should fail with 401 Unauthorized
   
   When Mentor tries to update another mentor’s profile, Should fail with 403 Forbidden
   
   When Mentor fetches their own profile, Should return profile data
   
   When Public User fetches mentor profile by slug, Should return public profile data
   
   When Mentor tries to fetch non-existent profile, Should fail with 404 Not Found
   
   When Mentor uploads valid profile picture, Should save image and update profile
   
   When Mentor uploads invalid profile picture format, Should fail with 400 Bad Request
   
   When Mentor removes profile picture, Should update profile to remove picture URL
*/