using System;
using System.Collections.Generic;

namespace ITJob.Entity.Entities
{
    public partial class WorkingExperience
    {
        public Guid Id { get; set; }
        public Guid? ProfileApplicantId { get; set; }
        public string? CompanyName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid? JobPositionId { get; set; }

        public virtual JobPosition? JobPosition { get; set; }
        public virtual ProfileApplicant? ProfileApplicant { get; set; }
    }
}
