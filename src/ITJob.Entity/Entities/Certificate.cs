using System;
using System.Collections.Generic;

namespace ITJob.Entity.Entities
{
    public partial class Certificate
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public Guid? SkillGroupId { get; set; }
        public Guid? ProfileApplicantId { get; set; }
        public DateTime? GrantDate { get; set; }
        public DateTime? ExpiryDate { get; set; }

        public virtual ProfileApplicant? ProfileApplicant { get; set; }
        public virtual SkillGroup? SkillGroup { get; set; }
    }
}
