using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kursfinderparser
{
    public class Topic
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public List<Course> Courses { get; set; }

        public Topic() => Courses = new List<Course>();
    }
}
