using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace converter.Models
{
    public partial class convertContext : DbContext
    {
        public convertContext()
        {
        }

        public convertContext(DbContextOptions<convertContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Convert> Converts { get; set; } = null!;
        public virtual DbSet<Result> Results { get; set; } = null!;
        public virtual DbSet<Status> Statuses { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Filename=convert.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Convert>(entity =>
            {
                entity.ToTable("convert");

                entity.HasIndex(e => e.Id, "IX_convert_id")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ContentType).HasColumnName("content_type");

                entity.Property(e => e.FileName).HasColumnName("file_name");
            });

            modelBuilder.Entity<Result>(entity =>
            {
                entity.ToTable("result");

                entity.HasIndex(e => e.Id, "IX_result_id")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Title)
                    .HasColumnType("VARCHAR (255)")
                    .HasColumnName("title");
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.ToTable("status");

                entity.HasIndex(e => e.Id, "IX_status_id")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ConvertId).HasColumnName("convert_id");

                entity.Property(e => e.DateTime).HasColumnName("date_time");

                entity.Property(e => e.ResultId).HasColumnName("result_id");

                entity.HasOne(d => d.Convert)
                    .WithMany(p => p.Statuses)
                    .HasForeignKey(d => d.ConvertId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Result)
                    .WithMany(p => p.Statuses)
                    .HasForeignKey(d => d.ResultId);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
