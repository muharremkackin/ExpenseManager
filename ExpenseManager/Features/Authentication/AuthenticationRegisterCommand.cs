using ExpenseManager.Data;
using ExpenseManager.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManager.Features.Authentication;

public sealed class AuthenticationRegisterCommand
{
    record Command(string Name, string LastName, string Email, string Password);
    record Response(Guid userId);

    public static void Endpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/authentication/register", async ([FromBody] Command command, [FromServices] ApplicationDbContext context, CancellationToken ct) => {

            var exist = await context.Users.AnyAsync(x => x.Email == command.Email, ct);
            if (exist)
                return Results.UnprocessableEntity();

            var user = User.Create(command.Name, command.LastName, command.Email);

            User.SetPassword(user, command.Password);

            await context.Users.AddAsync(user, ct);
            await context.SaveChangesAsync(ct);

            return Results.Ok(new Response(user.Id));
        });
    }
}
