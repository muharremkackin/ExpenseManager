using ExpenseManager.Data;
using ExpenseManager.Entities;
using ExpenseManager.Features.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), sqlServerOptions => {
        sqlServerOptions.EnableRetryOnFailure();
    });

    if (builder.Environment.IsProduction()) return;

    options.EnableSensitiveDataLogging();
    options.EnableDetailedErrors();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseHttpsRedirection();
    app.MapScalarApiReference();
}

app.MapAuthenticationEndpoints();

await app.RunAsync();



