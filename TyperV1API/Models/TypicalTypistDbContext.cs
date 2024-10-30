﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TypicalTypistAPI.Models;

public partial class TypicalTypistDbContext : DbContext
{
    public TypicalTypistDbContext()
    {
    }

    public TypicalTypistDbContext(DbContextOptions<TypicalTypistDbContext> options)
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

            entity.Property(e => e.ImagePath).HasMaxLength(300);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("users_userid_pk");

            entity.HasIndex(e => e.Email, "idx_users_email");

            entity.HasIndex(e => e.UserName, "idx_users_username");

            entity.HasIndex(e => e.Email, "users_email_unique").IsUnique();

            entity.HasIndex(e => e.UserName, "users_username_unique").IsUnique();

            entity.Property(e => e.Active)
                .IsRequired()
                .HasDefaultValueSql("('1')");
            entity.Property(e => e.Email).HasMaxLength(75);
            entity.Property(e => e.FirstName).HasMaxLength(30);
            entity.Property(e => e.ImageId).HasDefaultValue(1);
            entity.Property(e => e.Joined).HasDefaultValueSql("(CONVERT([date],getdate()))");
            entity.Property(e => e.LastName).HasMaxLength(30);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.UserName).HasMaxLength(30);

            entity.HasOne(d => d.Image).WithMany(p => p.Users)
                .HasForeignKey(d => d.ImageId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("users_imageid_fk");
        });

        modelBuilder.Entity<UserBigraphStat>(entity =>
        {
            entity.HasKey(e => e.BigraphStatId).HasName("userbigraphstats_bigraphstatid_pk");

            entity.HasIndex(e => e.Bigraph, "idx_userbigraphstats_bigraph");

            entity.HasIndex(e => e.UserId, "idx_userbigraphstats_userid");

            entity.Property(e => e.Accuracy)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(5, 2)");
            entity.Property(e => e.Bigraph).HasMaxLength(2);
            entity.Property(e => e.Speed)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(5, 3)");
            entity.Property(e => e.StartingKey)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.TotalTyped).HasDefaultValue(0);

            entity.HasOne(d => d.User).WithMany(p => p.UserBigraphStats)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__UserBigra__UserI__0A338187");
        });

        modelBuilder.Entity<UserKeyStat>(entity =>
        {
            entity.HasKey(e => e.KeyStatId).HasName("userkeystats_keystatid_pk");

            entity.HasIndex(e => e.UserId, "idx_userkeystats_userid");

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
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("userkeystats_userid_fk");
        });

        modelBuilder.Entity<UserStat>(entity =>
        {
            entity.HasKey(e => e.StatId).HasName("userstats_statid_pk");

            entity.HasIndex(e => e.UserId, "idx_userstats_userid");

            entity.Property(e => e.Accuracy)
                .HasComputedColumnSql("((CONVERT([decimal](5,2),[Correct])/nullif([Correct]+[Incorrect],(0)))*(100))", true)
                .HasColumnType("decimal(20, 13)");
            entity.Property(e => e.CharCorrect).HasDefaultValue(0);
            entity.Property(e => e.CharIncorrect).HasDefaultValue(0);
            entity.Property(e => e.Correct).HasDefaultValue(0);
            entity.Property(e => e.Cpm)
                .HasDefaultValue(0)
                .HasColumnName("CPM");
            entity.Property(e => e.Incorrect).HasDefaultValue(0);
            entity.Property(e => e.InitialLetter)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.Wpm)
                .HasDefaultValue(0)
                .HasColumnName("WPM");

            entity.HasOne(d => d.Bigraph).WithMany(p => p.UserStats)
                .HasForeignKey(d => d.BigraphId)
                .HasConstraintName("userstats_bigraphid_fk");

            entity.HasOne(d => d.User).WithMany(p => p.UserStats)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("userstats_userid_fk");

            entity.HasOne(d => d.Word).WithMany(p => p.UserStats)
                .HasForeignKey(d => d.WordId)
                .HasConstraintName("userstats_wordid_fk");
        });

        modelBuilder.Entity<UserTypingTest>(entity =>
        {
            entity.HasKey(e => e.TestId).HasName("usertypingtests_testid_pk");

            entity.HasIndex(e => e.UserId, "idx_usertypingtests_userid");

            entity.Property(e => e.Accuracy)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(5, 2)");
            entity.Property(e => e.CharCount).HasDefaultValue(0);
            entity.Property(e => e.IncorrectCount).HasDefaultValue(0);
            entity.Property(e => e.Mode)
                .HasMaxLength(25)
                .HasDefaultValue("random");
            entity.Property(e => e.TestDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Wpm)
                .HasDefaultValue(0m)
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

            entity.HasIndex(e => e.Length, "idx_words_length");

            entity.HasIndex(e => e.StartsWith, "idx_words_startswith");

            entity.HasIndex(e => e.Word1, "idx_words_word");

            entity.Property(e => e.StartsWith)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.Word1)
                .HasMaxLength(20)
                .HasColumnName("Word");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
