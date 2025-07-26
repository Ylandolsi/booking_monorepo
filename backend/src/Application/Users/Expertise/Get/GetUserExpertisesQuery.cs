using Application.Abstractions.Messaging;
namespace Application.Users.Expertise.Get;

public record GetUserExpertisesQuery(string UserSlug) : IQuery<List<ExpertiseResponse>>;