using System;
using System.Collections.Generic;

namespace ITJob.Entity.Entities
{
    public partial class Like
    {
        public Guid Id { get; set; }
        public Guid? JobPostId { get; set; }
        public Guid? ProfileApplicantId { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool? IsProfileApplicantLike { get; set; }
        public bool? IsJobPostLike { get; set; }
        public bool? Match { get; set; }
        public DateTime? MatchDate { get; set; }

        public virtual JobPost? JobPost { get; set; }
        public virtual ProfileApplicant? ProfileApplicant { get; set; }
    }
}
