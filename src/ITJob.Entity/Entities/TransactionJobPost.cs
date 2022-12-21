using System;
using System.Collections.Generic;

namespace ITJob.Entity.Entities
{
    public partial class TransactionJobPost
    {
        public Guid Id { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? Quantity { get; set; }
        public string? TypeOfTransaction { get; set; }
        public Guid? JobPostId { get; set; }
        public Guid? TransactionId { get; set; }
        public double? Total { get; set; }
        public Guid? MessageId { get; set; }
        public Guid? CreateBy { get; set; }
        public Guid? Receiver { get; set; }

        public virtual JobPost? JobPost { get; set; }
        public virtual Transaction? Transaction { get; set; }
    }
}
