using System;
using System.Collections.Generic;

namespace ITJob.Entity.Entities
{
    public partial class User
    {
        public Guid Id { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public Guid? RoleId { get; set; }
        public string? Password { get; set; }
        public int? Status { get; set; }
        public Guid? CompanyId { get; set; }
        public int? Code { get; set; }

        public virtual Company? Company { get; set; }
        public virtual Role? Role { get; set; }
    }
}
