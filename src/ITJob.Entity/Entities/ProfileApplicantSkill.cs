using System;
using System.Collections.Generic;

namespace ITJob.Entity.Entities
{
    public partial class ProfileApplicantSkill
    {
        public Guid Id { get; set; }
        public Guid? ProfileApplicantId { get; set; }
        public Guid? SkillId { get; set; }
        public string? SkillLevel { get; set; }

        public virtual ProfileApplicant? ProfileApplicant { get; set; }
        public virtual Skill? Skill { get; set; }
    }
}
