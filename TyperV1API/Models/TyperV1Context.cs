using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TyperV1API.Models;

public partial class TyperV1Context : DbContext
{
    public TyperV1Context()
    {
    }

    public TyperV1Context(DbContextOptions<TyperV1Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(Secret.url);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("images_imageid_pk");

            entity.Property(e => e.ImagePath).HasMaxLength(1000);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("users_userid_pk");

            entity.Property(e => e.Active)
                .IsRequired()
                .HasDefaultValueSql("('1')");
            entity.Property(e => e.FirstName).HasMaxLength(40);
            entity.Property(e => e.LastName).HasMaxLength(40);
            entity.Property(e => e.Password).HasMaxLength(30);
            entity.Property(e => e.UserName).HasMaxLength(40);
            entity.Property(e => e.Wpm)
                .HasDefaultValueSql("('0')")
                .HasColumnName("WPM");

            entity.HasOne(d => d.Image).WithMany(p => p.Users)
                .HasForeignKey(d => d.ImageId)
                .HasConstraintName("users_imageid_fk");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
