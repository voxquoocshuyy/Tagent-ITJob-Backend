using System;
using System.Collections.Generic;

namespace ITJob.Entity.Entities
{
    public partial class Wallet
    {
        public Wallet()
        {
            Transactions = new HashSet<Transaction>();
        }

        public Guid Id { get; set; }
        public double? Balance { get; set; }
        public int? Status { get; set; }
        public DateTime? CreateDate { get; set; }
        public Guid? CompanyId { get; set; }
        public Guid? ApplicantId { get; set; }

        public virtual Applicant? Applicant { get; set; }
        public virtual Company? Company { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
