using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NotificationsMicroservice.Entities;

namespace NotificationsMicroservice.Data;

public partial class NotificationsDbContext : DbContext
{
    public NotificationsDbContext()
    {
    }

    public NotificationsDbContext(DbContextOptions<NotificationsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<NotificationAttachment> NotificationAttachments { get; set; }

    public virtual DbSet<NotificationAttempt> NotificationAttempts { get; set; }

    public virtual DbSet<NotificationLog> NotificationLogs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NotificationAttachment>(entity =>
        {
            entity.HasKey(e => e.IdAttachment).HasName("PK__Notifica__DC8FA6141CEF5D76");

            entity.ToTable("NotificationAttachment");

            entity.Property(e => e.ContentType).HasMaxLength(100);
            entity.Property(e => e.FileName).HasMaxLength(255);
            entity.Property(e => e.FileUrl).HasMaxLength(500);

            entity.HasOne(d => d.IdNotificationNavigation).WithMany(p => p.NotificationAttachments)
                .HasForeignKey(d => d.IdNotification)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Attachment_Log");
        });

        modelBuilder.Entity<NotificationAttempt>(entity =>
        {
            entity.HasKey(e => e.IdAttempt).HasName("PK__Notifica__A1146BC8E245C9F0");

            entity.ToTable("NotificationAttempt");

            entity.Property(e => e.AttemptDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.IdNotificationNavigation).WithMany(p => p.NotificationAttempts)
                .HasForeignKey(d => d.IdNotification)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Attempt_Log");
        });

        modelBuilder.Entity<NotificationLog>(entity =>
        {
            entity.HasKey(e => e.IdNotification).HasName("PK__Notifica__950094B1A80FB83A");

            entity.ToTable("NotificationLog");

            entity.Property(e => e.Channel).HasMaxLength(50);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.RecipientContact).HasMaxLength(150);
            entity.Property(e => e.Subject).HasMaxLength(200);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
