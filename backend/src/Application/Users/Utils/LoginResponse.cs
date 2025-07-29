namespace Application.Users.Utils;

public sealed record LoginResponse(string UserSlug,
                                   string FirstName,
                                   string LastName,
                                   string Email,
                                   bool IsMentor,
                                   string? ProfilePictureUrl = null,
                                   bool MentorActive = false);