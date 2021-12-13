using ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Entities
{
    public class ChoosenProduct : IAggregateRoot
    {
        [Obsolete("Only for EFCore init")]
        public ChoosenProduct() { }
        public string Id { get; set; }
        public string ProductId { get; set; }
        public Product Product { get; set; }
        public int Remain { get; set; }
        public int[] Prices { get; set; }
        public List<PropertyItemTuple> PropertyItemTuples { get; set; }
    }

    public class PropertyItemTuple
    {
        public string Id { get; set; } 
        public string PropertyId { get; set; }
        public string PropertyItemId { get; set; }
    }
}
