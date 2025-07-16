using AlbumService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AlbumService.Infrastructure.Context;

public partial class AlbumDbContext : DbContext
{
    public AlbumDbContext()
    {
    }

    public AlbumDbContext(DbContextOptions<AlbumDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Album> Albums { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite("Data Source=../main.sqlite");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Album>(entity =>
        {
            entity.ToTable("album");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CoverLink).HasColumnName("cover_link");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.Title)
                .HasColumnType("string")
                .HasColumnName("title");
        });
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}