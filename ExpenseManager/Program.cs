using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseHttpsRedirection();
}

app.MapGet("/", async ([FromQuery] string? name, CancellationToken token) =>
{
    var message = "Merhaba, {0}";

    return Results.Ok(new
    {
        Message = string.Format(message, name ?? "World")
    });
});

await app.RunAsync();
