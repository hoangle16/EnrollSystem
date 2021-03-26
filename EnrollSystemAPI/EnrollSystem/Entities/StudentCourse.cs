using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnrollSystem.Entities
{
    public class StudentCourse
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public virtual Student Student { get; set; }
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }
        public virtual ICollection<Attendance> Attendances { get; set; }
    }
}
