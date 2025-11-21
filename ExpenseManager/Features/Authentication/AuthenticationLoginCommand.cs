using ExpenseManager.Data;
using ExpenseManager.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManager.Features.Authentication;

public sealed class AuthenticationLoginCommand
{
    record Command(string Email, string Password);
    record Response(string Name, string LastName);

    public static void Endpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/authentication/login", async ([FromBody] AuthenticationLoginCommand.Command command, [FromServices] ApplicationDbContext context, CancellationToken ct) =>
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Email == command.Email, ct);

            if (user == null)
                return Results.NotFound();

            if (!User.VerifyPassword(user, command.Password))
                return Results.Unauthorized();

            return Results.Ok(new Response(user.Name, user.LastName));
        });
    }
}

