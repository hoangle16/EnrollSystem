using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnrollSystem.Entities
{
    public class Teacher
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public  virtual ICollection<Course> Courses { get; set; }
    }
}
