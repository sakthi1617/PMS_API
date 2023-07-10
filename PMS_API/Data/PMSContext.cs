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
        public virtual DbSet<DelayedGoal> DelayedGoals { get; set; } = null!;
        public virtual DbSet<Department> Departments { get; set; } = null!;
        public virtual DbSet<Designation> Designations { get; set; } = null!;
        public virtual DbSet<DocumentTypeAnswer> DocumentTypeAnswers { get; set; } = null!;
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
        public virtual DbSet<MonthwiseRating> MonthwiseRatings { get; set; } = null!;
        public virtual DbSet<QuestionBank> QuestionBanks { get; set; } = null!;
        public virtual DbSet<QuestionMarkType> QuestionMarkTypes { get; set; } = null!;
        public virtual DbSet<QuestionPaper> QuestionPapers { get; set; } = null!;
        public virtual DbSet<QuestionPaperIdentity> QuestionPaperIdentities { get; set; } = null!;
        public virtual DbSet<QuestionType> QuestionTypes { get; set; } = null!;
        public virtual DbSet<QuetionLevel> QuetionLevels { get; set; } = null!;
        public virtual DbSet<RequestForApproved> RequestForApproveds { get; set; } = null!;
        public virtual DbSet<ResponseEmail> ResponseEmails { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Schema> Schemas { get; set; } = null!;
        public virtual DbSet<Server> Servers { get; set; } = null!;
        public virtual DbSet<Set> Sets { get; set; } = null!;
        public virtual DbSet<Skill> Skills { get; set; } = null!;
        public virtual DbSet<Stage> Stages { get; set; } = null!;
        public virtual DbSet<State> States { get; set; } = null!;
        public virtual DbSet<Team> Teams { get; set; } = null!;
        public virtual DbSet<TimeSettingTbl> TimeSettingTbls { get; set; } = null!;
        public virtual DbSet<UserLevel> UserLevels { get; set; } = null!;
        public virtual DbSet<Weightage> Weightages { get; set; } = null!;
        public virtual DbSet<TestStatus> TestStatuses { get; set; } = null!;

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

            modelBuilder.Entity<Department>(entity =>
            {
                entity.ToTable("Department");

                entity.Property(e => e.AddTime).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<Designation>(entity =>
            {
                entity.Property(e => e.AddTime).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTime).HasColumnType("datetime");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.Designations)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("FK__Designati__Depar__5D95E53A");
            });

            modelBuilder.Entity<DocumentTypeAnswer>(entity =>
            {
                entity.HasKey(e => e.DocumentId)
                    .HasName("PK__Document__1ABEEF0FED6A9795");

                entity.Property(e => e.AssigndAt).HasColumnType("datetime");

                entity.Property(e => e.DocumentUrl).HasColumnName("DocumentURL");

                entity.Property(e => e.SubmittedAt).HasColumnType("datetime");

                entity.Property(e => e.ValidatedAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<EmployeeAttachment>(entity =>
            {
                entity.HasKey(e => e.AttachmentId)
                    .HasName("PK__Employee__442C64BE71E21521");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.EmpReviewId).HasColumnName("EmpReviewID");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.HasOne(d => d.EmpReview)
                    .WithMany(p => p.EmployeeAttachments)
                    .HasForeignKey(d => d.EmpReviewId)
                    .HasConstraintName("FK__EmployeeA__EmpRe__4A8310C6");
            });

            modelBuilder.Entity<EmployeeGoalReview>(entity =>
            {
                entity.HasKey(e => e.EmpReviewId)
                    .HasName("PK__Employee__3BB9902FF9EAC844");

                entity.ToTable("EmployeeGoalReview");

                entity.Property(e => e.EmpReviewId).HasColumnName("EmpReviewID");

                entity.Property(e => e.AssingedManagerId).HasColumnName("AssingedManagerID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.GoalRating).HasColumnType("decimal(18, 1)");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.HasOne(d => d.AssingedManager)
                    .WithMany(p => p.EmployeeGoalReviewAssingedManagers)
                    .HasForeignKey(d => d.AssingedManagerId)
                    .HasConstraintName("FK__EmployeeG__Assin__7A3223E8");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.EmployeeGoalReviewEmployees)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK__EmployeeG__Emplo__46B27FE2");

                entity.HasOne(d => d.Goal)
                    .WithMany(p => p.EmployeeGoalReviews)
                    .HasForeignKey(d => d.GoalId)
                    .HasConstraintName("FK__EmployeeG__GoalI__47A6A41B");
            });

            modelBuilder.Entity<EmployeeModule>(entity =>
            {
                entity.HasKey(e => e.EmployeeId)
                    .HasName("PK__Employee__7AD04F11E03755A7");

                entity.ToTable("EmployeeModule");

                entity.Property(e => e.AddTime).HasColumnType("datetime");

                entity.Property(e => e.CurrentExperience).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.DateOfBirth).HasColumnType("date");

                entity.Property(e => e.DateOfJoining).HasColumnType("date");

                entity.Property(e => e.FirstLevelReportingManager).HasColumnName("First Level Reporting Manager");

                entity.Property(e => e.IsPublished).HasColumnName("isPublished");

                entity.Property(e => e.IsWantToPublish).HasColumnName("isWantToPublish");

                entity.Property(e => e.ModifiedTime).HasColumnType("datetime");

                entity.Property(e => e.PerformanceLevel).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PersonalEmail).HasColumnName("personalEmail");

                entity.Property(e => e.PriviousExperience).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Salary).HasColumnType("money");

                entity.Property(e => e.SecondLevelReportingManager).HasColumnName("Second Level Reporting Manager");

                entity.Property(e => e.TotalExperience).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.EmployeeModules)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("FK__EmployeeM__Depar__4CA06362");

                entity.HasOne(d => d.Designation)
                    .WithMany(p => p.EmployeeModules)
                    .HasForeignKey(d => d.DesignationId)
                    .HasConstraintName("FK__EmployeeM__Desig__16CE6296");

                entity.HasOne(d => d.FirstLevelReportingManagerNavigation)
                    .WithMany(p => p.InverseFirstLevelReportingManagerNavigation)
                    .HasForeignKey(d => d.FirstLevelReportingManager)
                    .HasConstraintName("FK__EmployeeM__First__625A9A57");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.EmployeeModules)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK__EmployeeM__RoleI__4E88ABD4");

                entity.HasOne(d => d.SecondLevelReportingManagerNavigation)
                    .WithMany(p => p.InverseSecondLevelReportingManagerNavigation)
                    .HasForeignKey(d => d.SecondLevelReportingManager)
                    .HasConstraintName("FK__EmployeeM__Secon__634EBE90");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.EmployeeModules)
                    .HasForeignKey(d => d.TeamId)
                    .HasConstraintName("FK__EmployeeM__TeamI__671F4F74");
            });

            modelBuilder.Entity<GoalModule>(entity =>
            {
                entity.HasKey(e => e.GoalId)
                    .HasName("PK__GoalModu__8A4FFFD14378DF24");

                entity.ToTable("GoalModule");

                entity.Property(e => e.AssingedManagerId).HasColumnName("AssingedManagerID");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.DueDate).HasColumnType("date");

                entity.Property(e => e.IsEmpExtentionApprovedAt).HasColumnType("datetime");

                entity.Property(e => e.IsManagerExtentionApprovedAt).HasColumnType("datetime");

                entity.Property(e => e.ModifyAt).HasColumnType("datetime");

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.HasOne(d => d.AssingedManager)
                    .WithMany(p => p.GoalModuleAssingedManagers)
                    .HasForeignKey(d => d.AssingedManagerId)
                    .HasConstraintName("FK__GoalModul__Assin__7849DB76");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.GoalModuleEmployees)
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

                entity.Property(e => e.RatingbyEmployee).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.RatingbyEmployeeCalculatedAt).HasColumnType("datetime");

                entity.Property(e => e.RatingbyManager).HasColumnType("decimal(18, 2)");

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
                    .HasName("PK__ManagerG__FDA69B42C7D4CE98");

                entity.ToTable("ManagerGoalReview");

                entity.Property(e => e.ManagerReviewId).HasColumnName("ManagerReviewID");

                entity.Property(e => e.AssingedManagerId).HasColumnName("AssingedManagerID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.EmpReviewId).HasColumnName("EmpReviewID");

                entity.Property(e => e.GoalRating).HasColumnType("decimal(18, 1)");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.HasOne(d => d.AssingedManager)
                    .WithMany(p => p.ManagerGoalReviewAssingedManagers)
                    .HasForeignKey(d => d.AssingedManagerId)
                    .HasConstraintName("FK__ManagerGo__Assin__7B264821");

                entity.HasOne(d => d.EmpReview)
                    .WithMany(p => p.ManagerGoalReviews)
                    .HasForeignKey(d => d.EmpReviewId)
                    .HasConstraintName("FK__ManagerGo__EmpRe__4F47C5E3");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.ManagerGoalReviewEmployees)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK__ManagerGo__Emplo__4E53A1AA");

                entity.HasOne(d => d.Goal)
                    .WithMany(p => p.ManagerGoalReviews)
                    .HasForeignKey(d => d.GoalId)
                    .HasConstraintName("FK__ManagerGo__GoalI__503BEA1C");
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

            modelBuilder.Entity<ManangerAttachment>(entity =>
            {
                entity.HasKey(e => e.AttachmentId)
                    .HasName("PK__Mananger__442C64BE8D9D1CDF");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ManagerReviewId).HasColumnName("ManagerReviewID");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.HasOne(d => d.ManagerReview)
                    .WithMany(p => p.ManangerAttachments)
                    .HasForeignKey(d => d.ManagerReviewId)
                    .HasConstraintName("FK__ManangerA__Manag__531856C7");
            });

            modelBuilder.Entity<MonthwiseRating>(entity =>
            {
                entity.ToTable("MonthwiseRating");

                entity.Property(e => e.April).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.August).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CalculatedAt).HasColumnType("datetime");

                entity.Property(e => e.December).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.February).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.January).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.July).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.June).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.March).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.May).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.November).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.October).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.OverallRating).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.September).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<QuestionBank>(entity =>
            {
                entity.HasKey(e => e.QuestionId)
                    .HasName("PK__Question__0DC06F8C1ED405DB");

                entity.ToTable("QuestionBank");

                entity.Property(e => e.QuestionId).HasColumnName("QuestionID");

                entity.Property(e => e.CorrectOption).HasColumnName("Correct_option");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.OptionA).HasColumnName("Option_A");

                entity.Property(e => e.OptionB).HasColumnName("Option_B");

                entity.Property(e => e.OptionC).HasColumnName("Option_C");

                entity.Property(e => e.OptionD).HasColumnName("Option_D");

                entity.Property(e => e.SkillLevel).HasColumnName("skillLevel");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.HasOne(d => d.MarksNavigation)
                    .WithMany(p => p.QuestionBanks)
                    .HasForeignKey(d => d.Marks)
                    .HasConstraintName("FK__QuestionB__Marks__13F1F5EB");

                entity.HasOne(d => d.QuestionLevelNavigation)
                    .WithMany(p => p.QuestionBanks)
                    .HasForeignKey(d => d.QuestionLevel)
                    .HasConstraintName("FK__QuestionB__Quest__12FDD1B2");

                entity.HasOne(d => d.QuestionTypeNavigation)
                    .WithMany(p => p.QuestionBanks)
                    .HasForeignKey(d => d.QuestionType)
                    .HasConstraintName("FK__QuestionB__Quest__078C1F06");

                entity.HasOne(d => d.Skill)
                    .WithMany(p => p.QuestionBanks)
                    .HasForeignKey(d => d.SkillId)
                    .HasConstraintName("FK__QuestionB__Skill__02C769E9");
            });

            modelBuilder.Entity<QuestionMarkType>(entity =>
            {
                entity.ToTable("QuestionMarkType");

                entity.Property(e => e.QuestionMarkTypeId).HasColumnName("QuestionMarkTypeID");
            });

            modelBuilder.Entity<QuestionPaper>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.OptionA).HasColumnName("Option_A");

                entity.Property(e => e.OptionB).HasColumnName("Option_B");

                entity.Property(e => e.OptionC).HasColumnName("Option_C");

                entity.Property(e => e.OptionD).HasColumnName("Option_D");

                entity.Property(e => e.QuestionPaperId).HasColumnName("QuestionPaperID");

                entity.HasOne(d => d.QuestionNavigation)
                    .WithMany(p => p.QuestionPapers)
                    .HasForeignKey(d => d.QuestionId)
                    .HasConstraintName("FK__QuestionP__Quest__0D44F85C");

                entity.HasOne(d => d.QuestionPaperNavigation)
                    .WithMany(p => p.QuestionPapers)
                    .HasForeignKey(d => d.QuestionPaperId)
                    .HasConstraintName("FK__QuestionP__Quest__0C50D423");

                entity.HasOne(d => d.QuestionTypeNavigation)
                    .WithMany(p => p.QuestionPapers)
                    .HasForeignKey(d => d.QuestionType)
                    .HasConstraintName("FK__QuestionP__Quest__0E391C95");
            });

            modelBuilder.Entity<QuestionPaperIdentity>(entity =>
            {
                entity.HasKey(e => e.QuestionPaperId)
                    .HasName("PK__Question__BBB4378D6994C246");

                entity.ToTable("QuestionPaperIdentity");

                entity.Property(e => e.QuestionPaperId).HasColumnName("QuestionPaperID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");

                entity.Property(e => e.SkillId).HasColumnName("skillId");

                entity.Property(e => e.SkillLevel).HasColumnName("skillLevel");
            });

            modelBuilder.Entity<QuestionType>(entity =>
            {
                entity.HasKey(e => e.TypeId)
                    .HasName("PK__Question__516F0395D67A0A45");

                entity.ToTable("QuestionType");

                entity.Property(e => e.TypeId).HasColumnName("TypeID");

                entity.Property(e => e.QuestionType1).HasColumnName("QuestionType");
            });

            modelBuilder.Entity<QuetionLevel>(entity =>
            {
                entity.ToTable("QuetionLevel");

                entity.Property(e => e.QuetionLevelId).HasColumnName("QuetionLevelID");
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

            modelBuilder.Entity<Team>(entity =>
            {
                entity.ToTable("Team");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.Teams)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("FK__Team__Department__662B2B3B");
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
                    .HasConstraintName("FK__Weightage__Desig__17C286CF");

                entity.HasOne(d => d.Skill)
                    .WithMany(p => p.Weightages)
                    .HasForeignKey(d => d.SkillId)
                    .HasConstraintName("FK__Weightage__Skill__46E78A0C");
            });

            modelBuilder.Entity<TestStatus>(entity =>
            {
                entity.HasKey(e => e.StatusId)
                    .HasName("PK__TestStat__C8EE2043FCC057BC");

                entity.ToTable("TestStatus");

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.Property(e => e.AssignedAt).HasColumnType("datetime");

                entity.Property(e => e.CompletedAt).HasColumnType("datetime");

                entity.Property(e => e.ItHaveADocument).HasColumnName("It_Have_a_Document");

                entity.Property(e => e.OpenedAt).HasColumnType("datetime");

                entity.Property(e => e.TestStatus1).HasColumnName("TestStatus");

                entity.Property(e => e.ValidatedAt).HasColumnType("datetime");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
