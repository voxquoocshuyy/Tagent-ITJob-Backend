using System;
using System.Collections.Generic;

namespace ITJob.Entity.Entities
{
    public partial class Company
    {
        public Company()
        {
            Blocks = new HashSet<Block>();
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
        public int? Premium { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? CompanyType { get; set; }
        public string? TaxCode { get; set; }

        public virtual ICollection<Block> Blocks { get; set; }
        public virtual ICollection<JobPost> JobPosts { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Wallet> Wallets { get; set; }
    }
}
