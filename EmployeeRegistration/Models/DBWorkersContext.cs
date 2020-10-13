using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EmployeeRegistration
{
    public partial class DBWorkersContext : DbContext
    {
        public DBWorkersContext()
        {
        }

        public DBWorkersContext(DbContextOptions<DBWorkersContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Picture> Picture { get; set; }
        public virtual DbSet<Workers> Workers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=DESKTOP-MOGI2N1\\SQLEXPRESS; Database=DBWorkers; Trusted_Connection=True; ");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Picture>(entity =>
            {
                entity.Property(e => e.PicturePath)
                    .HasMaxLength(200)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Workers>(entity =>
            {
                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsFixedLength();

                entity.Property(e => e.FirstName)
                    .HasMaxLength(100)
                    .IsFixedLength();

                entity.Property(e => e.LastName)
                    .HasMaxLength(100)
                    .IsFixedLength();

                entity.Property(e => e.Patronymic)
                    .HasMaxLength(100)
                    .IsFixedLength();

                entity.HasOne(d => d.PicturePath)
                    .WithMany(p => p.Workers)
                    .HasForeignKey(d => d.PicturePathId)
                    .HasConstraintName("FK_Workers_Picture");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
