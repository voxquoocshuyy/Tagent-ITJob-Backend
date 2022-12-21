using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ITJob.Entity.Entities
{
    public partial class ITJobContext : DbContext
    {
        public ITJobContext()
        {
        }

        public ITJobContext(DbContextOptions<ITJobContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AlbumImage> AlbumImages { get; set; } = null!;
        public virtual DbSet<Applicant> Applicants { get; set; } = null!;
        public virtual DbSet<Block> Blocks { get; set; } = null!;
        public virtual DbSet<Certificate> Certificates { get; set; } = null!;
        public virtual DbSet<Company> Companies { get; set; } = null!;
        public virtual DbSet<Employee> Employees { get; set; } = null!;
        public virtual DbSet<JobPosition> JobPositions { get; set; } = null!;
        public virtual DbSet<JobPost> JobPosts { get; set; } = null!;
        public virtual DbSet<JobPostSkill> JobPostSkills { get; set; } = null!;
        public virtual DbSet<Like> Likes { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<ProfileApplicant> ProfileApplicants { get; set; } = null!;
        public virtual DbSet<ProfileApplicantSkill> ProfileApplicantSkills { get; set; } = null!;
        public virtual DbSet<Project> Projects { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Skill> Skills { get; set; } = null!;
        public virtual DbSet<SkillGroup> SkillGroups { get; set; } = null!;
        public virtual DbSet<SkillLevel> SkillLevels { get; set; } = null!;
        public virtual DbSet<SystemWallet> SystemWallets { get; set; } = null!;
        public virtual DbSet<Transaction> Transactions { get; set; } = null!;
        public virtual DbSet<TransactionJobPost> TransactionJobPosts { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Wallet> Wallets { get; set; } = null!;
        public virtual DbSet<WorkingExperience> WorkingExperiences { get; set; } = null!;
        public virtual DbSet<WorkingStyle> WorkingStyles { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=13.232.213.53,1433;Initial Catalog=ITJob;User ID=sa;Password=Loyalty@Program");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AlbumImage>(entity =>
            {
                entity.ToTable("AlbumImage");

                entity.HasIndex(e => e.Id, "AlbumImage_Id_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UrlImage).IsUnicode(false);

                entity.HasOne(d => d.Applicant)
                    .WithMany(p => p.AlbumImages)
                    .HasForeignKey(d => d.ApplicantId)
                    .HasConstraintName("AlbumImage_Applicant_Id_fk");

                entity.HasOne(d => d.JobPost)
                    .WithMany(p => p.AlbumImages)
                    .HasForeignKey(d => d.JobPostId)
                    .HasConstraintName("AlbumImage_JobPost_Id_fk");

                entity.HasOne(d => d.ProfileApplicant)
                    .WithMany(p => p.AlbumImages)
                    .HasForeignKey(d => d.ProfileApplicantId)
                    .HasConstraintName("AlbumImage_ProfileApplicant_Id_fk");
            });

            modelBuilder.Entity<Applicant>(entity =>
            {
                entity.ToTable("Applicant");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Address).HasMaxLength(1000);

                entity.Property(e => e.Avatar).IsUnicode(false);

                entity.Property(e => e.Dob).HasColumnType("date");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Otp).HasColumnName("OTP");

                entity.Property(e => e.Password).IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Reason).HasMaxLength(1000);
            });

            modelBuilder.Entity<Block>(entity =>
            {
                entity.ToTable("Block");

                entity.HasIndex(e => e.Id, "Block_Id_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Applicant)
                    .WithMany(p => p.Blocks)
                    .HasForeignKey(d => d.ApplicantId)
                    .HasConstraintName("Block_Applicant_Id_fk");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Blocks)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("Block_Company_Id_fk");
            });

            modelBuilder.Entity<Certificate>(entity =>
            {
                entity.ToTable("Certificate");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.ExpiryDate).HasColumnType("date");

                entity.Property(e => e.GrantDate).HasColumnType("date");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.HasOne(d => d.ProfileApplicant)
                    .WithMany(p => p.Certificates)
                    .HasForeignKey(d => d.ProfileApplicantId)
                    .HasConstraintName("Certificate_ProfileApplicant_Id_fk");

                entity.HasOne(d => d.SkillGroup)
                    .WithMany(p => p.Certificates)
                    .HasForeignKey(d => d.SkillGroupId)
                    .HasConstraintName("Certificate_SkillGroup_Id_fk");
            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.ToTable("Company");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Logo).IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Password).IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Reason).HasMaxLength(1000);

                entity.Property(e => e.TaxCode)
                    .HasMaxLength(13)
                    .IsUnicode(false);

                entity.Property(e => e.Website)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("Employee");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Password).IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Reason).HasMaxLength(1000);

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("Employee_Company_null_fk");
            });

            modelBuilder.Entity<JobPosition>(entity =>
            {
                entity.ToTable("JobPosition");

                entity.HasIndex(e => e.Id, "JobPosition_Id_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<JobPost>(entity =>
            {
                entity.ToTable("JobPost");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.ApproveDate).HasColumnType("datetime");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EndTime).HasColumnType("datetime");

                entity.Property(e => e.Reason).HasMaxLength(1000);

                entity.Property(e => e.StartTime).HasColumnType("datetime");

                entity.Property(e => e.Title).HasMaxLength(100);

                entity.Property(e => e.WorkingPlace).HasMaxLength(1000);

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.JobPosts)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("JobPost_Company_Id_fk");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.JobPosts)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("JobPost_Employee_null_fk");

                entity.HasOne(d => d.JobPosition)
                    .WithMany(p => p.JobPosts)
                    .HasForeignKey(d => d.JobPositionId)
                    .HasConstraintName("JobPost_JobPosition_Id_fk");

                entity.HasOne(d => d.WorkingStyle)
                    .WithMany(p => p.JobPosts)
                    .HasForeignKey(d => d.WorkingStyleId)
                    .HasConstraintName("JobPost_WorkingStyle_Id_fk");
            });

            modelBuilder.Entity<JobPostSkill>(entity =>
            {
                entity.ToTable("JobPostSkill");

                entity.HasIndex(e => e.Id, "JobPostSkill_Id_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.SkillLevel).HasMaxLength(100);

                entity.HasOne(d => d.JobPost)
                    .WithMany(p => p.JobPostSkills)
                    .HasForeignKey(d => d.JobPostId)
                    .HasConstraintName("JobPostSkill_JobPost_Id_fk");

                entity.HasOne(d => d.Skill)
                    .WithMany(p => p.JobPostSkills)
                    .HasForeignKey(d => d.SkillId)
                    .HasConstraintName("JobPostSkill_Skill_Id_fk");
            });

            modelBuilder.Entity<Like>(entity =>
            {
                entity.ToTable("Like");

                entity.HasIndex(e => e.Id, "Liked_Id_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.MatchDate).HasColumnType("datetime");

                entity.HasOne(d => d.JobPost)
                    .WithMany(p => p.Likes)
                    .HasForeignKey(d => d.JobPostId)
                    .HasConstraintName("Liked_JobPost_Id_fk");

                entity.HasOne(d => d.ProfileApplicant)
                    .WithMany(p => p.Likes)
                    .HasForeignKey(d => d.ProfileApplicantId)
                    .HasConstraintName("Liked_ProfileApplicant_Id_fk");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Image).IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<ProfileApplicant>(entity =>
            {
                entity.ToTable("ProfileApplicant");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.Education).HasMaxLength(1000);

                entity.Property(e => e.FacebookLink)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.GithubLink)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.LinkedInLink)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.HasOne(d => d.Applicant)
                    .WithMany(p => p.ProfileApplicants)
                    .HasForeignKey(d => d.ApplicantId)
                    .HasConstraintName("FK__Profile__Applica__30F848ED");

                entity.HasOne(d => d.JobPosition)
                    .WithMany(p => p.ProfileApplicants)
                    .HasForeignKey(d => d.JobPositionId)
                    .HasConstraintName("ProfileApplicant_JobPosition_Id_fk");

                entity.HasOne(d => d.WorkingStyle)
                    .WithMany(p => p.ProfileApplicants)
                    .HasForeignKey(d => d.WorkingStyleId)
                    .HasConstraintName("ProfileApplicant_WorkingStyle_Id_fk");
            });

            modelBuilder.Entity<ProfileApplicantSkill>(entity =>
            {
                entity.ToTable("ProfileApplicantSkill");

                entity.HasIndex(e => e.Id, "ProfileApplicantSkill_Id_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.SkillLevel).HasMaxLength(100);

                entity.HasOne(d => d.ProfileApplicant)
                    .WithMany(p => p.ProfileApplicantSkills)
                    .HasForeignKey(d => d.ProfileApplicantId)
                    .HasConstraintName("ProfileApplicantSkill_ProfileApplicant_Id_fk");

                entity.HasOne(d => d.Skill)
                    .WithMany(p => p.ProfileApplicantSkills)
                    .HasForeignKey(d => d.SkillId)
                    .HasConstraintName("ProfileApplicantSkill_Skill_Id_fk");
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.ToTable("Project");

                entity.HasIndex(e => e.Id, "Project_Id_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.EndTime).HasColumnType("datetime");

                entity.Property(e => e.JobPosition).HasMaxLength(100);

                entity.Property(e => e.Link).HasMaxLength(1000);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Skill).HasMaxLength(100);

                entity.Property(e => e.StartTime).HasColumnType("datetime");

                entity.HasOne(d => d.ProfileApplicant)
                    .WithMany(p => p.Projects)
                    .HasForeignKey(d => d.ProfileApplicantId)
                    .HasConstraintName("Project_ProfileApplicant_Id_fk");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.HasIndex(e => e.Id, "Role_Id_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<Skill>(entity =>
            {
                entity.ToTable("Skill");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.HasOne(d => d.SkillGroup)
                    .WithMany(p => p.Skills)
                    .HasForeignKey(d => d.SkillGroupId)
                    .HasConstraintName("FK__Skill__SkillGrou__1ED998B2");
            });

            modelBuilder.Entity<SkillGroup>(entity =>
            {
                entity.ToTable("SkillGroup");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<SkillLevel>(entity =>
            {
                entity.ToTable("SkillLevel");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.HasOne(d => d.SkillGroup)
                    .WithMany(p => p.SkillLevels)
                    .HasForeignKey(d => d.SkillGroupId)
                    .HasConstraintName("SkillLevel_SkillGroup_null_fk");
            });

            modelBuilder.Entity<SystemWallet>(entity =>
            {
                entity.ToTable("SystemWallet");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.ToTable("Transaction");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.TypeOfTransaction).HasMaxLength(100);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("Transaction_Product_null_fk");

                entity.HasOne(d => d.Wallet)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.WalletId)
                    .HasConstraintName("Transaction_Wallet_null_fk");
            });

            modelBuilder.Entity<TransactionJobPost>(entity =>
            {
                entity.ToTable("TransactionJobPost");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.TypeOfTransaction).HasMaxLength(100);

                entity.HasOne(d => d.JobPost)
                    .WithMany(p => p.TransactionJobPosts)
                    .HasForeignKey(d => d.JobPostId)
                    .HasConstraintName("TransactionJobPost_JobPost_null_fk");

                entity.HasOne(d => d.Transaction)
                    .WithMany(p => p.TransactionJobPosts)
                    .HasForeignKey(d => d.TransactionId)
                    .HasConstraintName("TransactionJobPost_Transaction_null_fk");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Password).IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Reason).HasMaxLength(1000);

                entity.HasOne(d => d.Applicant)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.ApplicantId)
                    .HasConstraintName("User_Applicant_null_fk");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("User_Company_null_fk");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("User_Employee_null_fk");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("User_Role_null_fk");
            });

            modelBuilder.Entity<Wallet>(entity =>
            {
                entity.ToTable("Wallet");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("date")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Applicant)
                    .WithMany(p => p.Wallets)
                    .HasForeignKey(d => d.ApplicantId)
                    .HasConstraintName("Wallet_Applicant_null_fk");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Wallets)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("Wallet_Company_null_fk");
            });

            modelBuilder.Entity<WorkingExperience>(entity =>
            {
                entity.ToTable("WorkingExperience");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CompanyName).HasMaxLength(100);

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.HasOne(d => d.JobPosition)
                    .WithMany(p => p.WorkingExperiences)
                    .HasForeignKey(d => d.JobPositionId)
                    .HasConstraintName("WorkingExperience_JobPosition_Id_fk");

                entity.HasOne(d => d.ProfileApplicant)
                    .WithMany(p => p.WorkingExperiences)
                    .HasForeignKey(d => d.ProfileApplicantId)
                    .HasConstraintName("WorkingExperience_ProfileApplicant_Id_fk");
            });

            modelBuilder.Entity<WorkingStyle>(entity =>
            {
                entity.ToTable("WorkingStyle");

                entity.HasIndex(e => e.Id, "WorkingStyle_Id_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
