using Microsoft.EntityFrameworkCore;
using Mixology.Core.Entities;

namespace Mixology.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Mix> Mixes { get; set; } = null!;
    public DbSet<FavoriteMix> FavoriteMixes { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<FavoriteMix>()
            .HasKey(f => new { f.UserId, f.MixId });
        
        modelBuilder.Entity<FavoriteMix>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<FavoriteMix>()
            .HasOne<Mix>()
            .WithMany()
            .HasForeignKey(f => f.MixId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}