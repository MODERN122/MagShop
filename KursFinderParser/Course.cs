using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kursfinderparser
{
    public class Course
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public string Filters { get; set; }
        public byte[] Image { get; set; }
        public string OuterUrl { get; set; }
        public string Company { get; set; }
        public string Duration { get; set; }
        public string InnerUrl { get; set; }
        public string Tags { get; set; }
        public bool IsLoaded { get; set; }
    }
}
