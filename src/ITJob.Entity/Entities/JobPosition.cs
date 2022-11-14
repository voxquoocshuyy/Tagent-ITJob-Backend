using System;
using System.Collections.Generic;

namespace ITJob.Entity.Entities
{
    public partial class JobPosition
    {
        public JobPosition()
        {
            JobPosts = new HashSet<JobPost>();
            ProfileApplicants = new HashSet<ProfileApplicant>();
            WorkingExperiences = new HashSet<WorkingExperience>();
        }

        public Guid Id { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<JobPost> JobPosts { get; set; }
        public virtual ICollection<ProfileApplicant> ProfileApplicants { get; set; }
        public virtual ICollection<WorkingExperience> WorkingExperiences { get; set; }
    }
}
