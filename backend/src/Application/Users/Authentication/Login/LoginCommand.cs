using Application.Abstractions.Messaging;
using Application.Users.Authentication.Utils;
using Application.Users.Utils;

namespace Application.Users.Login;

public sealed record LoginCommand(string Email, string Password) : ICommand<LoginResponse>;

