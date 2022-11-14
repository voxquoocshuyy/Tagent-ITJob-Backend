using System;
using System.Collections.Generic;

namespace ITJob.Entity.Entities
{
    public partial class Applicant
    {
        public Applicant()
        {
            AlbumImages = new HashSet<AlbumImage>();
            Blocks = new HashSet<Block>();
            ProfileApplicants = new HashSet<ProfileApplicant>();
            Wallets = new HashSet<Wallet>();
        }

        public Guid Id { get; set; }
        public string Phone { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Email { get; set; }
        public int? Status { get; set; }
        public string? Name { get; set; }
        public string? Avatar { get; set; }
        public int? Gender { get; set; }
        public DateTime? Dob { get; set; }
        public string? Address { get; set; }
        public string? Otp { get; set; }
        public int? IsEarningMoney { get; set; }

        public virtual ICollection<AlbumImage> AlbumImages { get; set; }
        public virtual ICollection<Block> Blocks { get; set; }
        public virtual ICollection<ProfileApplicant> ProfileApplicants { get; set; }
        public virtual ICollection<Wallet> Wallets { get; set; }
    }
}
