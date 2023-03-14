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
        public virtual DbSet<Designation> Designations { get; set; } = null!;
        public virtual DbSet<EmployeeModule> EmployeeModules { get; set; } = null!;
        public virtual DbSet<Potential> Potentials { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Skill> Skills { get; set; } = null!;
        public virtual DbSet<UserLevel> UserLevels { get; set; } = null!;
        public virtual DbSet<Weightage> Weightages { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=CIPL1246DOTNET;Database=PMS_DB;user=sa;password=Colan123;Encrypt=False;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>(entity =>
            {
                entity.ToTable("Department");

                entity.Property(e => e.AddTime).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<Designation>(entity =>
            {
                entity.ToTable("Designation");

                entity.Property(e => e.AddTime).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<EmployeeModule>(entity =>
            {
                entity.HasKey(e => e.EmployeeId)
                    .HasName("PK__Employee__7AD04F11E03755A7");

                entity.ToTable("EmployeeModule");

                entity.Property(e => e.AddTime).HasColumnType("datetime");

                entity.Property(e => e.DateOfBirth).HasColumnType("date");

                entity.Property(e => e.DateOfJoining).HasColumnType("date");

                entity.Property(e => e.FirstLevelReportingManager).HasColumnName("First Level Reporting Manager");

                entity.Property(e => e.ModifiedTime).HasColumnType("datetime");

                entity.Property(e => e.PersonalEmail).HasColumnName("personalEmail");

                entity.Property(e => e.SecondLevelReportingManager).HasColumnName("Second Level Reporting Manager");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.EmployeeModules)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("FK__EmployeeM__Depar__4CA06362");

                entity.HasOne(d => d.Designation)
                    .WithMany(p => p.EmployeeModules)
                    .HasForeignKey(d => d.DesignationId)
                    .HasConstraintName("FK__EmployeeM__Desig__4D94879B");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.EmployeeModules)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK__EmployeeM__RoleI__4E88ABD4");
            });

            modelBuilder.Entity<Potential>(entity =>
            {
                entity.ToTable("Potential");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.RollId)
                    .HasName("PK__Roles__7886EE5F5618632A");
            });

            modelBuilder.Entity<UserLevel>(entity =>
            {
                entity.ToTable("UserLevel");

                entity.HasOne(d => d.Employee);

            });

            modelBuilder.Entity<Weightage>(entity =>
            {
                entity.ToTable("Weightage");

                entity.Property(e => e.Weightage1).HasColumnName("Weightage");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.Weightages)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("FK__Weightage__Depar__44FF419A");

                entity.HasOne(d => d.Designation)
                    .WithMany(p => p.Weightages)
                    .HasForeignKey(d => d.DesignationId)
                    .HasConstraintName("FK__Weightage__Desig__45F365D3");

                entity.HasOne(d => d.Skill)
                    .WithMany(p => p.Weightages)
                    .HasForeignKey(d => d.SkillId)
                    .HasConstraintName("FK__Weightage__Skill__46E78A0C");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
