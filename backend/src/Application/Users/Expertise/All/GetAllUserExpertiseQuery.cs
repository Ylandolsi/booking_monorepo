using Application.Abstractions.Messaging;

namespace Application.Users.Expertise.All;

public record GetAllExpertiseQuery() : IQuery<List<ExpertiseResponse>>;