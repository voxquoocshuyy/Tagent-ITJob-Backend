using System;
using System.Collections.Generic;

namespace ITJob.Entity.Entities
{
    public partial class Transaction
    {
        public Transaction()
        {
            TransactionJobPosts = new HashSet<TransactionJobPost>();
        }

        public Guid Id { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? TypeOfTransaction { get; set; }
        public Guid? CreateBy { get; set; }
        public Guid? WalletId { get; set; }
        public double? Total { get; set; }
        public Guid? ProductId { get; set; }
        public int? Quantity { get; set; }

        public virtual Product? Product { get; set; }
        public virtual Wallet? Wallet { get; set; }
        public virtual ICollection<TransactionJobPost> TransactionJobPosts { get; set; }
    }
}
