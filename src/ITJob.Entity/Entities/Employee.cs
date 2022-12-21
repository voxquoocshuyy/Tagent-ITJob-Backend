using System;
using System.Collections.Generic;

namespace ITJob.Entity.Entities
{
    public partial class Employee
    {
        public Employee()
        {
            JobPosts = new HashSet<JobPost>();
            Users = new HashSet<User>();
        }

        public Guid Id { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Name { get; set; }
        public int? Status { get; set; }
        public string? Reason { get; set; }
        public Guid? CompanyId { get; set; }

        public virtual Company? Company { get; set; }
        public virtual ICollection<JobPost> JobPosts { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
