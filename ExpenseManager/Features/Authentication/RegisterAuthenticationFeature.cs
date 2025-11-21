namespace ExpenseManager.Features.Authentication;

public static class RegisterAuthenticationFeature
{
    public static void MapAuthenticationEndpoints(this IEndpointRouteBuilder endpoints)
    {
        AuthenticationRegisterCommand.Endpoint(endpoints);
        AuthenticationLoginCommand.Endpoint(endpoints);
        AuthenticationForgotPasswordCommand.Endpoint(endpoints);
        AuthenticationResetPasswordCommand.Endpoint(endpoints);
    }
}
