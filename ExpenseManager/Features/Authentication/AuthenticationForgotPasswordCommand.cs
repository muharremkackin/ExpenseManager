using ExpenseManager.Data;
using ExpenseManager.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManager.Features.Authentication;

public sealed class AuthenticationForgotPasswordCommand
{
    record Command(string Email);
    record Response(string Email);
    public static void Endpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/authentication/forgot-password", async ([FromBody] Command command, [FromServices] ApplicationDbContext context, CancellationToken ct) =>
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Email == command.Email);

            if (user == null)
                return Results.NotFound();
            var code = VerificationCode.GenerateCode();

            var verification = VerificationCode.Create(user.Id, command.Email, code.Item2);
            await context.VerificationCodes.AddAsync(verification, ct);
            await context.SaveChangesAsync(ct);

            // EmailSendSimulation
            Console.WriteLine("{0}'e gönderilen kod {1}", command.Email, code.Item1);

            return Results.Ok(new Response(command.Email));
        });
    }
}
