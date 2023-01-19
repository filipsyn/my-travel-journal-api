using Microsoft.EntityFrameworkCore;
using MyTravelJournal.Api.Models;

namespace MyTravelJournal.Api.Data;

public class MtjDbContext : DbContext
{
    public MtjDbContext(DbContextOptions<MtjDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseSerialColumns();
    }

    public DbSet<User>? Users { get; set; }
}