using ExpenseManager.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManager.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<VerificationCode> VerificationCodes { get; set; }
}
