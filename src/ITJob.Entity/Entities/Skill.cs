using System;
using System.Collections.Generic;

namespace ITJob.Entity.Entities
{
    public partial class Skill
    {
        public Skill()
        {
            JobPostSkills = new HashSet<JobPostSkill>();
            ProfileApplicantSkills = new HashSet<ProfileApplicantSkill>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public Guid? SkillGroupId { get; set; }

        public virtual SkillGroup? SkillGroup { get; set; }
        public virtual ICollection<JobPostSkill> JobPostSkills { get; set; }
        public virtual ICollection<ProfileApplicantSkill> ProfileApplicantSkills { get; set; }
    }
}
