using System;
using System.Collections.Generic;

namespace ITJob.Entity.Entities
{
    public partial class ProfileApplicant
    {
        public ProfileApplicant()
        {
            AlbumImages = new HashSet<AlbumImage>();
            Certificates = new HashSet<Certificate>();
            Likes = new HashSet<Like>();
            ProfileApplicantSkills = new HashSet<ProfileApplicantSkill>();
            Projects = new HashSet<Project>();
            WorkingExperiences = new HashSet<WorkingExperience>();
        }

        public Guid Id { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid? ApplicantId { get; set; }
        public string? Description { get; set; }
        public string? Education { get; set; }
        public string? GithubLink { get; set; }
        public string? LinkedInLink { get; set; }
        public string? FacebookLink { get; set; }
        public Guid? JobPositionId { get; set; }
        public Guid? WorkingStyleId { get; set; }
        public int? Status { get; set; }
        public int? CountLike { get; set; }
        public int? CountShare { get; set; }

        public virtual Applicant? Applicant { get; set; }
        public virtual JobPosition? JobPosition { get; set; }
        public virtual WorkingStyle? WorkingStyle { get; set; }
        public virtual ICollection<AlbumImage> AlbumImages { get; set; }
        public virtual ICollection<Certificate> Certificates { get; set; }
        public virtual ICollection<Like> Likes { get; set; }
        public virtual ICollection<ProfileApplicantSkill> ProfileApplicantSkills { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<WorkingExperience> WorkingExperiences { get; set; }
    }
}
