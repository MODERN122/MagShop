using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kursfinderparser
{
    public class Category
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public List<Topic> Topics { get; set; }

        public Category() => Topics = new List<Topic>();
    }
}
