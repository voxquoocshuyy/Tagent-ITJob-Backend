using System;
using System.Collections.Generic;

namespace ITJob.Entity.Entities
{
    public partial class AlbumImage
    {
        public Guid Id { get; set; }
        public string UrlImage { get; set; } = null!;
        public Guid? ProfileApplicantId { get; set; }
        public Guid? JobPostId { get; set; }
        public DateTime? CreateDate { get; set; }
        public Guid? ApplicantId { get; set; }

        public virtual Applicant? Applicant { get; set; }
        public virtual JobPost? JobPost { get; set; }
        public virtual ProfileApplicant? ProfileApplicant { get; set; }
    }
}
