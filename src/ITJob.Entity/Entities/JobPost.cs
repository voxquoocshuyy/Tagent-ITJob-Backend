using System;
using System.Collections.Generic;

namespace ITJob.Entity.Entities
{
    public partial class JobPost
    {
        public JobPost()
        {
            AlbumImages = new HashSet<AlbumImage>();
            JobPostSkills = new HashSet<JobPostSkill>();
            Likes = new HashSet<Like>();
            TransactionJobPosts = new HashSet<TransactionJobPost>();
        }

        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public DateTime CreateDate { get; set; }
        public string Description { get; set; } = null!;
        public int Quantity { get; set; }
        public int Status { get; set; }
        public Guid? CompanyId { get; set; }
        public Guid? JobPositionId { get; set; }
        public Guid? WorkingStyleId { get; set; }
        public string? WorkingPlace { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? Reason { get; set; }
        public double? Money { get; set; }
        public DateTime? ApproveDate { get; set; }
        public Guid? EmployeeId { get; set; }

        public virtual Company? Company { get; set; }
        public virtual Employee? Employee { get; set; }
        public virtual JobPosition? JobPosition { get; set; }
        public virtual WorkingStyle? WorkingStyle { get; set; }
        public virtual ICollection<AlbumImage> AlbumImages { get; set; }
        public virtual ICollection<JobPostSkill> JobPostSkills { get; set; }
        public virtual ICollection<Like> Likes { get; set; }
        public virtual ICollection<TransactionJobPost> TransactionJobPosts { get; set; }
    }
}
