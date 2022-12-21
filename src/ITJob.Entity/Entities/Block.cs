using System;
using System.Collections.Generic;

namespace ITJob.Entity.Entities
{
    public partial class Block
    {
        public Guid Id { get; set; }
        public Guid? CompanyId { get; set; }
        public Guid? ApplicantId { get; set; }
        public Guid? BlockBy { get; set; }
        public DateTime? CreateDate { get; set; }

        public virtual Applicant? Applicant { get; set; }
        public virtual Company? Company { get; set; }
    }
}
