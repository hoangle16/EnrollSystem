using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnrollSystem.Entities
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
    }
}
