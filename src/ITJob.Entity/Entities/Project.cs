using System;
using System.Collections.Generic;

namespace ITJob.Entity.Entities
{
    public partial class Project
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Link { get; set; }
        public string? Description { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public Guid? ProfileApplicantId { get; set; }
        public string? Skill { get; set; }
        public string? JobPosition { get; set; }

        public virtual ProfileApplicant? ProfileApplicant { get; set; }
    }
}
