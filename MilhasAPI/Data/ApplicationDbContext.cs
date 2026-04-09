using Microsoft.EntityFrameworkCore;
using MilhasAPI.Models;

namespace MilhasAPI.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<CreditCard> CreditCards { get; set; }
    public DbSet<RewardTransaction> RewardTransactions { get; set; }
}
