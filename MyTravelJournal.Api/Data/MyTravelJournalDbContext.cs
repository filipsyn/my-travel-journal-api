using Microsoft.EntityFrameworkCore;
using MyTravelJournal.Api.Models;

namespace MyTravelJournal.Api.Data;

public class MyTravelJournalDbContext : DbContext
{
    public MyTravelJournalDbContext(DbContextOptions<MyTravelJournalDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseSerialColumns();
    }

    public DbSet<User> Users { get; set; } = null!;
}