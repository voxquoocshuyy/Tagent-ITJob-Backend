using System;
using System.Collections.Generic;

namespace ITJob.Entity.Entities
{
    public partial class Product
    {
        public Product()
        {
            Transactions = new HashSet<Transaction>();
        }

        public Guid Id { get; set; }
        public string? Name { get; set; }
        public double? Price { get; set; }
        public int? Quantity { get; set; }
        public int? Status { get; set; }
        public string? Image { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
