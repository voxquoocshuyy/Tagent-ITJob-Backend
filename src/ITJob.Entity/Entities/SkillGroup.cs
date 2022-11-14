using System;
using System.Collections.Generic;

namespace ITJob.Entity.Entities
{
    public partial class SkillGroup
    {
        public SkillGroup()
        {
            Certificates = new HashSet<Certificate>();
            SkillLevels = new HashSet<SkillLevel>();
            Skills = new HashSet<Skill>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Certificate> Certificates { get; set; }
        public virtual ICollection<SkillLevel> SkillLevels { get; set; }
        public virtual ICollection<Skill> Skills { get; set; }
    }
}
