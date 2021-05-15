using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kursfinderparser
{
    public class CourseLibrary
    {
        public List<Category> Categories { get; set; }

        public CourseLibrary() => Categories = new List<Category>();
    }
}
