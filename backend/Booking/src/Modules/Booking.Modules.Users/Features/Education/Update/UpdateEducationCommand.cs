using Booking.Common.Messaging;

namespace Booking.Modules.Users.Features.Education.Update;

public sealed record UpdateEducationCommand(int EducationId,
                                            int UserId,
                                            string Field,
                                            string University,
                                            DateTime StartDate,
                                            DateTime? EndDate,
                                            string? Description) : ICommand;