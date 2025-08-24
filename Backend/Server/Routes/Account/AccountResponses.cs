namespace Backend.Server.Routes.Account;

public record LoginResponseDto
{
    public required string Token { get; init; }
    public required string Username { get; init; }
}