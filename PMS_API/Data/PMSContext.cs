using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using PMS_API.Models;

namespace PMS_API.Data
{
    public partial class PMSContext : DbContext
    {
        public PMSContext()
        {
        }

        public PMSContext(DbContextOptions<PMSContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Department> Departments { get; set; } = null!;
        public virtual DbSet<EmployeeModule> EmployeeModules { get; set; } = null!;
        public virtual DbSet<Skillset> Skillsets { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=CIPL1246DOTNET;Database=PMS;user=sa;password=Colan123;Encrypt=False;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(e => e.DesignationId)
                    .HasName("PK__Departme__BABD603E4239F7C3");

                entity.ToTable("Department");

                entity.Property(e => e.DesignationId).HasColumnName("DesignationID");

                entity.Property(e => e.DesignationName).HasColumnName("Designation Name");
            });

            modelBuilder.Entity<EmployeeModule>(entity =>
            {
                entity.HasKey(e => e.EmployeeId)
                    .HasName("PK__Employee__7AD04F11529D5D85");

                entity.ToTable("EmployeeModule");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DateOfBirth).HasColumnType("date");

                entity.Property(e => e.DateOfJoining).HasColumnType("date");

                entity.Property(e => e.DesignationId).HasColumnName("DesignationID");

                entity.Property(e => e.EmployeeName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.FirstLevelReportingManager).HasColumnName("First level Reporting Manager");

                entity.Property(e => e.ProfilePicture).HasColumnName("Profile Picture");

                entity.Property(e => e.SecondLevelReportingManager).HasColumnName("Second level Reporting Manager");

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Designation)
                    .WithMany(p => p.EmployeeModules)
                    .HasForeignKey(d => d.DesignationId)
                    .HasConstraintName("FK__EmployeeM__Desig__4BAC3F29");
            });

            modelBuilder.Entity<Skillset>(entity =>
            {
                entity.ToTable("Skillset");

                entity.Property(e => e.DesignationId).HasColumnName("DesignationID");

                entity.HasOne(d => d.Designation)
                    .WithMany(p => p.Skillsets)
                    .HasForeignKey(d => d.DesignationId)
                    .HasConstraintName("FK__Skillset__Design__5070F446");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
