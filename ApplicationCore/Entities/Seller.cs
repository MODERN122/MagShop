using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Entities
{
    public class Seller : User
    {
        [Obsolete("Uses only for EF Core generating")]
        public Seller() { }
        public Seller(string sellerId) : base(sellerId) { }
    }
}
