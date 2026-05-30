namespace Acme.Center.Platform.IAM.Interfaces.REST.Resources;

public record AuthenticatedUserResource(int Id, string Username, string Token);