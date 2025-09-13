using Booking.Common.Domain.Entity;

namespace Booking.Modules.Mentorships.Domain.Entities.Stores;

public class Store : Entity
{
    public int Id { get; private set; }
    public string Slug { get; private set; }

    public string Name { get; private set; }
    public string Description { get; private set; }

    public Links Links { get; private set; }
    public ProfilePicture ProfilePicture { get; private set; }
    public bool Active { get; private set; } = true;

    // STEP 1 : Title/Slug/Description 
    // STEP 2 : Picture 
    // STEP 3 : Links 
    public int Step { get; private set; } = 0;
}