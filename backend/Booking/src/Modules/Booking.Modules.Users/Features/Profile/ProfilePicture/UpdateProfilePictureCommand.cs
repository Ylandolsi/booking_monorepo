using Booking.Common.Messaging;
using Microsoft.AspNetCore.Http;

namespace Booking.Modules.Users.Features.Profile.ProfilePicture;

public record UpdateProfilePictureCommand(int UserId, IFormFile File) : ICommand<ProfilePictureRespone>;