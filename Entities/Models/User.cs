namespace Entities.Models;

public sealed class User : IdentityUser
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
}