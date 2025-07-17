using Microsoft.EntityFrameworkCore;
using ReviewService.Domain.Models;

namespace ReviewService.Infrastructure.Persistence;

public partial class ReviewDbContext : DbContext
{
    public ReviewDbContext()
    {
    }

    public ReviewDbContext(DbContextOptions<ReviewDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Review> Reviews { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite("Data Source=../main.sqlite");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Review>(entity =>
        {
            entity.ToTable("review");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasColumnType("text(2000)")
                .HasColumnName("description");
            entity.Property(e => e.MbId).HasColumnName("mb_id");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.TrackRatings).HasColumnName("track_ratings");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}