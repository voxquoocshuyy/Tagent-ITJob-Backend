using System;
using System.Collections.Generic;

namespace ITJob.Entity.Entities
{
    public partial class Company
    {
        public Company()
        {
            Blocks = new HashSet<Block>();
            Employees = new HashSet<Employee>();
            JobPosts = new HashSet<JobPost>();
            Users = new HashSet<User>();
            Wallets = new HashSet<Wallet>();
        }

        public Guid Id { get; set; }
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string? Logo { get; set; }
        public string Website { get; set; } = null!;
        public int Status { get; set; }
        public bool? IsPremium { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? CompanyType { get; set; }
        public string? TaxCode { get; set; }
        public string? Reason { get; set; }
        public string? Password { get; set; }
        public int? Code { get; set; }

        public virtual ICollection<Block> Blocks { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<JobPost> JobPosts { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Wallet> Wallets { get; set; }
    }
}
