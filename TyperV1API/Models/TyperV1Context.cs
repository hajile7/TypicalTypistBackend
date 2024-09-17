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

    public virtual DbSet<Bigraph> Bigraphs { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserBigraphStat> UserBigraphStats { get; set; }

    public virtual DbSet<UserKeyStat> UserKeyStats { get; set; }

    public virtual DbSet<UserStat> UserStats { get; set; }

    public virtual DbSet<UserTypingTest> UserTypingTests { get; set; }

    public virtual DbSet<Word> Words { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer(Secret.url);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bigraph>(entity =>
        {
            entity.HasKey(e => e.BigraphId).HasName("bigraphs_bigraphid_pk");

            entity.Property(e => e.Bigraph1)
                .HasMaxLength(2)
                .HasColumnName("Bigraph");

            entity.HasOne(d => d.Word).WithMany(p => p.Bigraphs)
                .HasForeignKey(d => d.WordId)
                .HasConstraintName("bigraphs_wordid_fk");
        });

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
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(40);
            entity.Property(e => e.LastName).HasMaxLength(40);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.UserName).HasMaxLength(40);

            entity.HasOne(d => d.Image).WithMany(p => p.Users)
                .HasForeignKey(d => d.ImageId)
                .HasConstraintName("users_imageid_fk");
        });

        modelBuilder.Entity<UserBigraphStat>(entity =>
        {
            entity.HasKey(e => e.BigraphStatId).HasName("userbigraphstats_bigraphstatid_pk");

            entity.Property(e => e.Accuracy).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.Bigraph).HasMaxLength(2);
            entity.Property(e => e.Speed).HasColumnType("decimal(5, 3)");
            entity.Property(e => e.StartingKey)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.TotalTyped).HasDefaultValue(0);

            entity.HasOne(d => d.User).WithMany(p => p.UserBigraphStats)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__UserBigra__UserI__75F77EB0");
        });

        modelBuilder.Entity<UserKeyStat>(entity =>
        {
            entity.HasKey(e => e.KeyStatId).HasName("userkeystats_keystatid_pk");

            entity.Property(e => e.Accuracy)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(5, 2)");
            entity.Property(e => e.Key)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.Speed)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(5, 3)");
            entity.Property(e => e.TotalTyped).HasDefaultValue(0);

            entity.HasOne(d => d.User).WithMany(p => p.UserKeyStats)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("userkeystats_userid_fk");
        });

        modelBuilder.Entity<UserStat>(entity =>
        {
            entity.HasKey(e => e.StatId).HasName("userstats_statid_pk");

            entity.Property(e => e.Accuracy)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(5, 2)");
            entity.Property(e => e.CharsTyped).HasDefaultValue(0);
            entity.Property(e => e.Cpm)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("CPM");
            entity.Property(e => e.TimeTyped).HasDefaultValue(0);
            entity.Property(e => e.TopAccuracy)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(5, 2)");
            entity.Property(e => e.TopCpm)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("TopCPM");
            entity.Property(e => e.TopWpm)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("TopWPM");
            entity.Property(e => e.Wpm)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("WPM");

            entity.HasOne(d => d.User).WithMany(p => p.UserStats)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("userstats_userid_fk");
        });

        modelBuilder.Entity<UserTypingTest>(entity =>
        {
            entity.HasKey(e => e.TestId).HasName("usertypingtests_testid_pk");

            entity.Property(e => e.Accuracy).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.CharCount).HasDefaultValue(0);
            entity.Property(e => e.IncorrectCount).HasDefaultValue(0);
            entity.Property(e => e.Mode).HasMaxLength(20);
            entity.Property(e => e.Wpm)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("WPM");

            entity.HasOne(d => d.User).WithMany(p => p.UserTypingTests)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("usertypingtests_userid_fk");
        });

        modelBuilder.Entity<Word>(entity =>
        {
            entity.HasKey(e => e.WordId).HasName("words_wordid_pk");

            entity.Property(e => e.StartsWith)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.Word1)
                .HasMaxLength(100)
                .HasColumnName("Word");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
