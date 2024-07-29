namespace Service;

internal sealed class AuthenticationService(
    ILoggerManager logger,
    IMapper mapper,
    // Provides the APIs for managing users in a persistence store
    UserManager<User> userManager,
    IConfiguration configuration) : IAuthenticationService
{
    public async Task<IdentityResult> RegisterUser(UserRegistrationRequest userRegistration)
    {
        var user = mapper.Map<User>(userRegistration);
        var result = await userManager.CreateAsync(user, userRegistration.Password!);
        if (result.Succeeded)
        {
            await userManager.AddToRolesAsync(user, userRegistration.Roles!);
        }
        return result;
    }
}