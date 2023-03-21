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

        public virtual DbSet<ApprovedStatus> ApprovedStatuses { get; set; } = null!;
        public virtual DbSet<Department> Departments { get; set; } = null!;
        public virtual DbSet<Designation> Designations { get; set; } = null!;
        public virtual DbSet<EmployeeModule> EmployeeModules { get; set; } = null!;
        public virtual DbSet<GoalModule> GoalModules { get; set; } = null!;
        public virtual DbSet<ManagersTbl> ManagersTbls { get; set; } = null!;
        public virtual DbSet<Potential> Potentials { get; set; } = null!;
        public virtual DbSet<RequestForApproved> RequestForApproveds { get; set; } = null!;
        public virtual DbSet<ResponseMail> ResponseMails { get; set; } = null!;
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
            modelBuilder.Entity<ApprovedStatus>(entity =>
            {
                entity.HasKey(e => e.ApprovalId)
                    .HasName("PK__Approved__328477D4E2CF56CF");

                entity.ToTable("ApprovedStatus");

                entity.Property(e => e.ApprovalId).HasColumnName("ApprovalID");

                entity.Property(e => e.Iactive).HasColumnName("IActive");

                entity.Property(e => e.ReqId).HasColumnName("ReqID");

                entity.HasOne(d => d.Req)
                    .WithMany(p => p.ApprovedStatuses)
                    .HasForeignKey(d => d.ReqId)
                    .HasConstraintName("FK__ApprovedS__ReqID__6754599E");
            });

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

                entity.HasOne(d => d.FirstLevelReportingManagerNavigation)
                    .WithMany(p => p.EmployeeModuleFirstLevelReportingManagerNavigations)
                    .HasForeignKey(d => d.FirstLevelReportingManager)
                    .HasConstraintName("FK__EmployeeM__First__5EBF139D");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.EmployeeModules)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK__EmployeeM__RoleI__4E88ABD4");

                entity.HasOne(d => d.SecondLevelReportingManagerNavigation)
                    .WithMany(p => p.EmployeeModuleSecondLevelReportingManagerNavigations)
                    .HasForeignKey(d => d.SecondLevelReportingManager)
                    .HasConstraintName("FK__EmployeeM__Secon__5FB337D6");
            });

            modelBuilder.Entity<GoalModule>(entity =>
            {
                entity.HasKey(e => e.GoalId)
                    .HasName("PK__GoalModu__8A4FFFD14378DF24");

                entity.ToTable("GoalModule");

                entity.Property(e => e.AssignedAt).HasColumnType("datetime");

                entity.Property(e => e.DueDate).HasColumnType("date");

                entity.Property(e => e.ModifyAt).HasColumnType("datetime");

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.GoalModules)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK__GoalModul__Emplo__5AEE82B9");
            });

            modelBuilder.Entity<ManagersTbl>(entity =>
            {
                entity.HasKey(e => e.ManagerId)
                    .HasName("PK__Managers__3BA2AA8135F86391");

                entity.ToTable("Managers_tbl");

                entity.Property(e => e.ManagerId).HasColumnName("ManagerID");

                entity.Property(e => e.ContactNumber).HasMaxLength(100);

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.ManagersTbls)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK__Managers___Emplo__60A75C0F");
            });

            modelBuilder.Entity<Potential>(entity =>
            {
                entity.ToTable("Potential");
            });

            modelBuilder.Entity<RequestForApproved>(entity =>
            {
                entity.HasKey(e => e.ReqId)
                    .HasName("PK__RequestF__28A9A3A2A398B24B");

                entity.ToTable("RequestForApproved");

                entity.Property(e => e.ReqId).HasColumnName("ReqID");

                entity.Property(e => e.ModifiedAt).HasColumnType("datetime");

                entity.Property(e => e.RequestCreatedAt).HasColumnType("datetime");

                entity.Property(e => e.RequestCreatedById).HasColumnName("RequestCreatedByID");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.RequestForApproveds)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK__RequestFo__Emplo__6383C8BA");

                entity.HasOne(d => d.RequestCreatedByNavigation)
                    .WithMany(p => p.RequestForApproveds)
                    .HasForeignKey(d => d.RequestCreatedById)
                    .HasConstraintName("FK__RequestFo__Reque__6477ECF3");
            });

            modelBuilder.Entity<ResponseMail>(entity =>
            {
                entity.HasKey(e => e.ResId)
                    .HasName("PK__Response__29788216DFD29A07");

                entity.ToTable("ResponseMail");

                entity.Property(e => e.ResId).HasColumnName("ResID");

                entity.Property(e => e.ApprovalId).HasColumnName("ApprovalID");

                entity.Property(e => e.DeliverdAt).HasColumnType("datetime");

                entity.Property(e => e.MailCc).HasColumnName("Mail_CC");

                entity.Property(e => e.MailFrom).HasColumnName("Mail_From");

                entity.Property(e => e.MailTo).HasColumnName("Mail_TO");

                entity.Property(e => e.ReqId).HasColumnName("ReqID");

                entity.HasOne(d => d.Approval)
                    .WithMany(p => p.ResponseMails)
                    .HasForeignKey(d => d.ApprovalId)
                    .HasConstraintName("FK__ResponseM__Appro__6B24EA82");

                entity.HasOne(d => d.Req)
                    .WithMany(p => p.ResponseMails)
                    .HasForeignKey(d => d.ReqId)
                    .HasConstraintName("FK__ResponseM__ReqID__6A30C649");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.RollId)
                    .HasName("PK__Roles__7886EE5F5618632A");
            });

            modelBuilder.Entity<UserLevel>(entity =>
            {
                entity.ToTable("UserLevel");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.UserLevels)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK__UserLevel__Emplo__5070F446");

                entity.HasOne(d => d.Skill)
                    .WithMany(p => p.UserLevels)
                    .HasForeignKey(d => d.SkillId)
                    .HasConstraintName("FK__UserLevel__Skill__5165187F");
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
