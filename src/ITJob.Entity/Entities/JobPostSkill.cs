using System;
using System.Collections.Generic;

namespace ITJob.Entity.Entities
{
    public partial class JobPostSkill
    {
        public Guid Id { get; set; }
        public Guid? JobPostId { get; set; }
        public Guid? SkillId { get; set; }
        public string? SkillLevel { get; set; }

        public virtual JobPost? JobPost { get; set; }
        public virtual Skill? Skill { get; set; }
    }
}
