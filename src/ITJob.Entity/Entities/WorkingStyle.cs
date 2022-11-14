using System;
using System.Collections.Generic;

namespace ITJob.Entity.Entities
{
    public partial class WorkingStyle
    {
        public WorkingStyle()
        {
            JobPosts = new HashSet<JobPost>();
            ProfileApplicants = new HashSet<ProfileApplicant>();
        }

        public Guid Id { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<JobPost> JobPosts { get; set; }
        public virtual ICollection<ProfileApplicant> ProfileApplicants { get; set; }
    }
}
