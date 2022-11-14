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
        public int? IsApplicantLike { get; set; }
        public int? IsJobPostLike { get; set; }
        public int? Match { get; set; }

        public virtual JobPost? JobPost { get; set; }
        public virtual ProfileApplicant? ProfileApplicant { get; set; }
    }
}
