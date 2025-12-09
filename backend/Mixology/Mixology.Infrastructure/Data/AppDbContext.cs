using Microsoft.EntityFrameworkCore;
using Mixology.Core.Entities;
using Mixology.Core.ValueObjects;

namespace Mixology.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Mix> Mixes { get; set; } = null!;
    public DbSet<Brand> Brands { get; set; } = null!;
    public DbSet<RawMaterial> RawMaterials { get; set; } = null!;
    public DbSet<Collection> Collections { get; set; } = null!;
    public DbSet<FavoriteMix> FavoriteMixes { get; set; } = null!;
    public DbSet<Rating> Ratings { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Ignore<MixComposition>();
        modelBuilder.Ignore<FlavorProfile>();
        modelBuilder.Ignore<RatingValue>();

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            
            entity.Property(u => u.Role)
                .HasConversion<string>()
                .HasMaxLength(50);

            entity.Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(255);

            entity.HasIndex(u => u.Email)
                .IsUnique();

            entity.Property(u => u.PasswordHash)
                .IsRequired();
        });
        
        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(b => b.Id);

            entity.Property(b => b.Name)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(b => b.Description)
                .HasMaxLength(1000);
            
            entity.Property(b => b.LogoData)
                .HasColumnType("bytea");

            entity.Property(b => b.LogoContentType)
                .HasMaxLength(100);

            entity.Property(b => b.LogoFileName)
                .HasMaxLength(255);

            entity.HasMany(b => b.RawMaterials)
                .WithOne()
                .HasForeignKey(rm => rm.BrandId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        modelBuilder.Entity<RawMaterial>(entity =>
        {
            entity.HasKey(rm => rm.Id);

            entity.Property(rm => rm.Name)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(rm => rm.Flavor)
                .HasConversion(
                    flavor => string.Join(",", flavor.Tags),
                    str => new FlavorProfile(str.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList())
                )
                .IsRequired();

            entity.Property(rm => rm.Strength)
                .IsRequired();
        });
        
        modelBuilder.Entity<Collection>(entity =>
        {
            entity.HasKey(c => c.Id);

            entity.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(c => c.ImageData)
                .HasColumnType("bytea");

            entity.Property(c => c.ImageContentType)
                .HasMaxLength(100);

            entity.Property(c => c.ImageFileName)
                .HasMaxLength(255);

            entity.HasMany(c => c.Mixes)
                .WithOne()
                .HasForeignKey(m => m.CollectionId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne<User>()
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        modelBuilder.Entity<Mix>(entity =>
        {
            entity.HasKey(m => m.Id);

            entity.Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(m => m.Description)
                .HasMaxLength(1000);
            
            entity.Property(m => m.ImageData)
                .HasColumnType("bytea"); 

            entity.Property(m => m.ImageContentType)
                .HasMaxLength(100);

            entity.Property(m => m.ImageFileName)
                .HasMaxLength(255);

            entity.Property(m => m.Flavor)
                .HasConversion(
                    flavor => string.Join(",", flavor.Tags),
                    str => new FlavorProfile(str.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList())
                )
                .IsRequired();
            
            entity.Property(m => m.Compositions)
                .HasConversion(
                    compositions => System.Text.Json.JsonSerializer.Serialize(compositions, (System.Text.Json.JsonSerializerOptions?)null),
                    json => System.Text.Json.JsonSerializer.Deserialize<List<MixComposition>>(json, (System.Text.Json.JsonSerializerOptions?)null) ?? new List<MixComposition>()
                )
                .IsRequired();

            entity.HasOne<User>()
                .WithMany(u => u.Mixes)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(m => m.RatingAverage);
            entity.HasIndex(m => m.CreatedAt);
        });
        
        modelBuilder.Entity<FavoriteMix>(entity =>
        {
            entity.HasKey(f => new { f.UserId, f.MixId });

            entity.HasOne<User>()
                .WithMany()
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne<Mix>()
                .WithMany()
                .HasForeignKey(f => f.MixId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasKey(r => new { r.UserId, r.MixId });
            
            entity.Property(r => r.Value)
                .HasConversion(
                    value => value.Value,
                    intValue => new RatingValue(intValue)
                )
                .IsRequired();

            entity.HasOne<User>()
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne<Mix>()
                .WithMany()
                .HasForeignKey(r => r.MixId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
