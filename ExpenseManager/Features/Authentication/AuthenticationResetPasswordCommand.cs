using ExpenseManager.Data;
using ExpenseManager.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManager.Features.Authentication;

public sealed class AuthenticationResetPasswordCommand
{
    record Command(string Email, string Code, string Password);
    record Response(bool IsSuccessful);
    public static void Endpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/authentication/reset-password", async ([FromBody] Command command, [FromServices] ApplicationDbContext context, CancellationToken ct) =>
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Email == command.Email, ct);
            if (user == null)
                return Results.NotFound();

            var verification = await context.VerificationCodes.FirstOrDefaultAsync(x => x.UserId == user.Id && x.Receiver == command.Email && !x.IsUsed && (x.ExpirationDate == null || x.ExpirationDate > DateTime.UtcNow));
            
            if (verification == null)
                return Results.NotFound();

            if (verification.TryCount >= 5)
            {
                return Results.Forbid();
            }

            if (!VerificationCode.Verify(verification, command.Code))
            {
                await context.SaveChangesAsync(ct);
                return Results.Unauthorized();
            }

            User.SetPassword(user, command.Password);

            await context.SaveChangesAsync(ct);

            return Results.Ok(new Response(true));
        });
    }
}