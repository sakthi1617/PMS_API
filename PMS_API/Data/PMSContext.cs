using System;
using System.Collections.Generic;
using Hangfire.Common;
using Microsoft.AspNetCore.Hosting.Server;
using System.Data.Common;
using System.Diagnostics.Metrics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Org.BouncyCastle.Utilities.Collections;
using PMS_API.Models;
using PMS_API.ViewModels;
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

        public virtual DbSet<AggregatedCounter> AggregatedCounters { get; set; } = null!;
        public virtual DbSet<Counter> Counters { get; set; } = null!;
        public virtual DbSet<Department> Departments { get; set; } = null!;
        public virtual DbSet<Designation> Designations { get; set; } = null!;
        public virtual DbSet<Designation1> Designations1 { get; set; } = null!;
        public virtual DbSet<EmployeeAttachment> EmployeeAttachments { get; set; } = null!;
        public virtual DbSet<EmployeeGoalReview> EmployeeGoalReviews { get; set; } = null!;
        public virtual DbSet<EmployeeModule> EmployeeModules { get; set; } = null!;
        public virtual DbSet<GoalModule> GoalModules { get; set; } = null!;
        public virtual DbSet<GoalRating> GoalRatings { get; set; } = null!;
        public virtual DbSet<Hash> Hashes { get; set; } = null!;
       // public virtual DbSet<Job> Jobs { get; set; } = null!;
        public virtual DbSet<JobParameter> JobParameters { get; set; } = null!;
        public virtual DbSet<JobQueue> JobQueues { get; set; } = null!;
        public virtual DbSet<List> Lists { get; set; } = null!;
        public virtual DbSet<ManagerGoalReview> ManagerGoalReviews { get; set; } = null!;
        public virtual DbSet<ManagersTbl> ManagersTbls { get; set; } = null!;
        public virtual DbSet<ManangerAttachment> ManangerAttachments { get; set; } = null!;
        public virtual DbSet<Stage> Stages { get; set; } = null!;
        public virtual DbSet<RequestForApproved> RequestForApproveds { get; set; } = null!;
        public virtual DbSet<ResponseEmail> ResponseEmails { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Schema> Schemas { get; set; } = null!;
        public virtual DbSet<Server> Servers { get; set; } = null!;
        public virtual DbSet<Set> Sets { get; set; } = null!;
        public virtual DbSet<Skill> Skills { get; set; } = null!;
        public virtual DbSet<State> States { get; set; } = null!;
        public virtual DbSet<TimeSettingTbl> TimeSettingTbls { get; set; } = null!;
        public virtual DbSet<UserLevel> UserLevels { get; set; } = null!;
        public virtual DbSet<Weightage> Weightages { get; set; } = null!;
        public virtual DbSet<DelayedGoal> DelayedGoals { get; set; } = null!;
        public virtual DbSet<Team> Teams { get; set; } = null!;

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
            modelBuilder.Entity<AggregatedCounter>(entity =>
            {
                entity.HasKey(e => e.Key)
                    .HasName("PK_HangFire_CounterAggregated");

                entity.ToTable("AggregatedCounter", "HangFire");

                entity.HasIndex(e => e.ExpireAt, "IX_HangFire_AggregatedCounter_ExpireAt")
                    .HasFilter("([ExpireAt] IS NOT NULL)");

                entity.Property(e => e.Key).HasMaxLength(100);

                entity.Property(e => e.ExpireAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<Counter>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Counter", "HangFire");

                entity.HasIndex(e => e.Key, "CX_HangFire_Counter")
                    .IsClustered();

                entity.Property(e => e.ExpireAt).HasColumnType("datetime");

                entity.Property(e => e.Key).HasMaxLength(100);
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

            modelBuilder.Entity<Designation1>(entity =>
            {
                entity.HasKey(e => e.DesignationId)
                    .HasName("PK__Designat__BABD60DE780CAE1E");

                entity.ToTable("Designations");

                entity.Property(e => e.AddTime).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTime).HasColumnType("datetime");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.Designation1s)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("FK__Designati__Depar__5D95E53A");
            });

            modelBuilder.Entity<EmployeeAttachment>(entity =>
            {
                entity.HasKey(e => e.AttachmentId)
                    .HasName("PK__Employee__442C64BEAB3EF546");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.EmpReviewId).HasColumnName("EmpReviewID");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.HasOne(d => d.EmpReview)
                    .WithMany(p => p.EmployeeAttachments)
                    .HasForeignKey(d => d.EmpReviewId)
                    .HasConstraintName("FK__EmployeeA__EmpRe__2EDAF651");
            });

            modelBuilder.Entity<EmployeeGoalReview>(entity =>
            {
                entity.HasKey(e => e.EmpReviewId)
                    .HasName("PK__Employee__3BB9902F15D51F87");

                entity.ToTable("EmployeeGoalReview");

                entity.Property(e => e.EmpReviewId).HasColumnName("EmpReviewID");

                entity.Property(e => e.AssingedManagerId).HasColumnName("AssingedManagerID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
                entity.Property(e => e.GoalRating).HasColumnType("decimal(18, 1)");

                entity.HasOne(d => d.AssingedManager)
                    .WithMany(p => p.EmployeeGoalReviews)
                    .HasForeignKey(d => d.AssingedManagerId)
                    .HasConstraintName("FK__EmployeeG__Assin__2B0A656D");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.EmployeeGoalReviews)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK__EmployeeG__Emplo__2A164134");

                entity.HasOne(d => d.Goal)
                    .WithMany(p => p.EmployeeGoalReviews)
                    .HasForeignKey(d => d.GoalId)
                    .HasConstraintName("FK__EmployeeG__GoalI__2BFE89A6");
            });

            modelBuilder.Entity<EmployeeModule>(entity =>
            {
                entity.HasKey(e => e.EmployeeId)
                    .HasName("PK__Employee__7AD04F11E03755A7");

                entity.ToTable("EmployeeModule");

                entity.Property(e => e.AddTime).HasColumnType("datetime");
                entity.Property(e => e.TotalExperience).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.CurrentExperience).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.PriviousExperience).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.DateOfBirth).HasColumnType("date");

                entity.Property(e => e.DateOfJoining).HasColumnType("date");

                entity.Property(e => e.FirstLevelReportingManager).HasColumnName("First Level Reporting Manager");

                entity.Property(e => e.ModifiedTime).HasColumnType("datetime");

                entity.Property(e => e.PersonalEmail).HasColumnName("personalEmail");

                entity.Property(e => e.Salary).HasColumnType("money");

                entity.Property(e => e.SecondLevelReportingManager).HasColumnName("Second Level Reporting Manager");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.EmployeeModules)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("FK__EmployeeM__Depar__4CA06362");

                entity.HasOne(d => d.Designation)
                    .WithMany(p => p.EmployeeModules)
                    .HasForeignKey(d => d.DesignationId)
                    .HasConstraintName("FK__EmployeeM__Desig__4D94879B");

                entity.HasOne(d => d.DesignationNavigation)
                    .WithMany(p => p.EmployeeModules)
                    .HasForeignKey(d => d.DesignationId)
                    .HasConstraintName("FK__EmployeeM__Desig__5E8A0973");

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

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.AssingedManagerId).HasColumnName("AssingedManagerID");

                entity.Property(e => e.DueDate).HasColumnType("date");

                entity.Property(e => e.ModifyAt).HasColumnType("datetime");

                entity.Property(e => e.StartDate).HasColumnType("date");
                

                entity.HasOne(d => d.AssingedManager)
                    .WithMany(p => p.GoalModules)
                    .HasForeignKey(d => d.AssingedManagerId)
                    .HasConstraintName("FK__GoalModul__Assin__2739D489");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.GoalModules)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK__GoalModul__Emplo__5AEE82B9");
            });


            modelBuilder.Entity<GoalRating>(entity =>
            {
                entity.HasKey(e => e.RatingId)
                    .HasName("PK__GoalRati__FCCDF85C3838DB56");

                entity.ToTable("GoalRating");

                entity.Property(e => e.RatingId).HasColumnName("RatingID");

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.ManagerId).HasColumnName("ManagerID");

                entity.Property(e => e.RatingbyEmployeeCalculatedAt).HasColumnType("datetime");

                entity.Property(e => e.RatingbyManagerCalculatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("RatingbyManagerCalculatedAT");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.GoalRatings)
                    .HasForeignKey(d => d.EmployeeId)


                    .HasConstraintName("FK__GoalRatin__Emplo__3A4CA8FD");
            });


            modelBuilder.Entity<Hash>(entity =>
            {
                entity.HasKey(e => new { e.Key, e.Field })
                    .HasName("PK_HangFire_Hash");

                entity.ToTable("Hash", "HangFire");

                entity.HasIndex(e => e.ExpireAt, "IX_HangFire_Hash_ExpireAt")
                    .HasFilter("([ExpireAt] IS NOT NULL)");

                entity.Property(e => e.Key).HasMaxLength(100);

                entity.Property(e => e.Field).HasMaxLength(100);
            });

            //modelBuilder.Entity<Job>(entity =>
            //{
            //    entity.ToTable("Job", "HangFire");

            //    entity.HasIndex(e => e.ExpireAt, "IX_HangFire_Job_ExpireAt")
            //        .HasFilter("([ExpireAt] IS NOT NULL)");

            //    entity.HasIndex(e => e.StateName, "IX_HangFire_Job_StateName")
            //        .HasFilter("([StateName] IS NOT NULL)");

            //    entity.Property(e => e.CreatedAt).HasColumnType("datetime");

            //    entity.Property(e => e.ExpireAt).HasColumnType("datetime");

            //    entity.Property(e => e.StateName).HasMaxLength(20);
            //});

            modelBuilder.Entity<JobParameter>(entity =>
            {
                entity.HasKey(e => new { e.JobId, e.Name })
                    .HasName("PK_HangFire_JobParameter");

                entity.ToTable("JobParameter", "HangFire");

                entity.Property(e => e.Name).HasMaxLength(40);

                entity.HasOne(d => d.Job)
                    .WithMany(p => p.JobParameters)
                    .HasForeignKey(d => d.JobId)
                    .HasConstraintName("FK_HangFire_JobParameter_Job");
            });

            modelBuilder.Entity<JobQueue>(entity =>
            {
                entity.HasKey(e => new { e.Queue, e.Id })
                    .HasName("PK_HangFire_JobQueue");

                entity.ToTable("JobQueue", "HangFire");

                entity.Property(e => e.Queue).HasMaxLength(50);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.FetchedAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<List>(entity =>
            {
                entity.HasKey(e => new { e.Key, e.Id })
                    .HasName("PK_HangFire_List");

                entity.ToTable("List", "HangFire");

                entity.HasIndex(e => e.ExpireAt, "IX_HangFire_List_ExpireAt")
                    .HasFilter("([ExpireAt] IS NOT NULL)");

                entity.Property(e => e.Key).HasMaxLength(100);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.ExpireAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<ManagerGoalReview>(entity =>
            {
                entity.HasKey(e => e.ManagerReviewId)
                    .HasName("PK__ManagerG__FDA69B421D4B6F29");

                entity.ToTable("ManagerGoalReview");

                entity.Property(e => e.ManagerReviewId).HasColumnName("ManagerReviewID");

                entity.Property(e => e.AssingedManagerId).HasColumnName("AssingedManagerID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.GoalRating).HasColumnType("decimal(18, 1)");

                entity.HasOne(d => d.AssingedManager)
                    .WithMany(p => p.ManagerGoalReviews)
                    .HasForeignKey(d => d.AssingedManagerId)
                    .HasConstraintName("FK__ManagerGo__Assin__32AB8735");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.ManagerGoalReviews)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK__ManagerGo__Emplo__31B762FC");

                entity.HasOne(d => d.Goal)
                    .WithMany(p => p.ManagerGoalReviews)
                    .HasForeignKey(d => d.GoalId)
                    .HasConstraintName("FK__ManagerGo__GoalI__339FAB6E");

                entity.HasOne(d => d.EmpReview)
                    .WithMany(p => p.ManagerGoalReviews)
                    .HasForeignKey(d => d.EmpReviewId)
                    .HasConstraintName("FK__ManagerGo__EmpRe__37703C52");
            });

            modelBuilder.Entity<ManagersTbl>(entity =>
            {
                entity.HasKey(e => e.ManagerId)
                    .HasName("PK__Managers__3BA2AA8135F86391");

                entity.ToTable("Managers_tbl");

                entity.Property(e => e.ManagerId).HasColumnName("ManagerID");
                entity.Property(e => e.Reporting1Person).HasColumnName("Reporting1Person");
                entity.Property(e => e.Reporting2Person).HasColumnName("Reporting2Person");

                entity.Property(e => e.ContactNumber).HasMaxLength(100);

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.ManagersTbls)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK__Managers___Emplo__60A75C0F");
            });

            modelBuilder.Entity<ManangerAttachment>(entity =>
            {
                entity.HasKey(e => e.AttachmentId)
                    .HasName("PK__Mananger__442C64BE4C14E318");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ManagerReviewId).HasColumnName("ManagerReviewID");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.HasOne(d => d.ManagerReview)
                    .WithMany(p => p.ManangerAttachments)
                    .HasForeignKey(d => d.ManagerReviewId)
                    .HasConstraintName("FK__ManangerA__Manag__367C1819");
            });

            //modelBuilder.Entity<Potential>(entity =>
            //{
            //    entity.ToTable("Potential");
            //});

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

                entity.HasOne(d => d.Skill)
                    .WithMany(p => p.RequestForApproveds)
                    .HasForeignKey(d => d.Skillid)
                    .HasConstraintName("FK__RequestFo__Skill__1332DBDC");
            });

            modelBuilder.Entity<ResponseEmail>(entity =>
            {
                entity.HasKey(e => e.ResponseId)
                    .HasName("PK__Response__1AAA640C84BE8313");

                entity.ToTable("Response_Email");

                entity.Property(e => e.ResponseId).HasColumnName("ResponseID");

                entity.Property(e => e.DeliverdAt).HasColumnType("datetime");

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.NotifiedAt).HasColumnType("datetime");

                entity.Property(e => e.ReqId).HasColumnName("ReqID");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.HasOne(d => d.Req)
                    .WithMany(p => p.ResponseEmails)
                    .HasForeignKey(d => d.ReqId)
                    .HasConstraintName("FK__Response___ReqID__18EBB532");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.RollId)
                    .HasName("PK__Roles__7886EE5F5618632A");
            });

            modelBuilder.Entity<Schema>(entity =>
            {
                entity.HasKey(e => e.Version)
                    .HasName("PK_HangFire_Schema");

                entity.ToTable("Schema", "HangFire");

                entity.Property(e => e.Version).ValueGeneratedNever();
            });

            modelBuilder.Entity<Server>(entity =>
            {
                entity.ToTable("Server", "HangFire");

                entity.HasIndex(e => e.LastHeartbeat, "IX_HangFire_Server_LastHeartbeat");

                entity.Property(e => e.Id).HasMaxLength(200);

                entity.Property(e => e.LastHeartbeat).HasColumnType("datetime");
            });

            modelBuilder.Entity<Set>(entity =>
            {
                entity.HasKey(e => new { e.Key, e.Value })
                    .HasName("PK_HangFire_Set");

                entity.ToTable("Set", "HangFire");

                entity.HasIndex(e => e.ExpireAt, "IX_HangFire_Set_ExpireAt")
                    .HasFilter("([ExpireAt] IS NOT NULL)");

                entity.HasIndex(e => new { e.Key, e.Score }, "IX_HangFire_Set_Score");

                entity.Property(e => e.Key).HasMaxLength(100);

                entity.Property(e => e.Value).HasMaxLength(256);

                entity.Property(e => e.ExpireAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<State>(entity =>
            {
                entity.HasKey(e => new { e.JobId, e.Id })
                    .HasName("PK_HangFire_State");

                entity.ToTable("State", "HangFire");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(20);

                entity.Property(e => e.Reason).HasMaxLength(100);

                entity.HasOne(d => d.Job)
                    .WithMany(p => p.States)
                    .HasForeignKey(d => d.JobId)
                    .HasConstraintName("FK_HangFire_State_Job");
            });

            modelBuilder.Entity<TimeSettingTbl>(entity =>
            {
                entity.ToTable("TimeSettingTbl");
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

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.Weightages)
                    .HasForeignKey(d => d.TeamId)
                    .HasConstraintName("FK__Weightage__TeamI__690797E6");
           

        });

            modelBuilder.Entity<DelayedGoal>(entity =>
            {
                entity.ToTable("Delayed_Goals");

                entity.Property(e => e.AdminApprovedAt).HasColumnType("datetime");

                entity.Property(e => e.AssignedAt).HasColumnType("datetime");

                entity.Property(e => e.AssingedManagerId).HasColumnName("AssingedManagerID");

                entity.Property(e => e.DueDate).HasColumnType("date");

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.HasOne(d => d.AssingedManager)
                    .WithMany(p => p.DelayedGoals)
                    .HasForeignKey(d => d.AssingedManagerId)
                    .HasConstraintName("FK__Delayed_G__Assin__56E8E7AB");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.DelayedGoals)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK__Delayed_G__Emplo__55F4C372");
            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity.ToTable("Team");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.Teams)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("FK__Team__Department__662B2B3B");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
