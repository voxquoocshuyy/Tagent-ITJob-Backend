using System;
using System.Collections.Generic;

namespace ITJob.Entity.Entities
{
    public partial class SkillLevel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public Guid? SkillGroupId { get; set; }

        public virtual SkillGroup? SkillGroup { get; set; }
    }
}
