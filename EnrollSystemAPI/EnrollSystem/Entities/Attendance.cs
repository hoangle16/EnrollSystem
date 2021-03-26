using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnrollSystem.Entities
{
    public class Attendance
    {
        public int Id { get; set; }
        public int StudentCourseId { get; set; }
        public virtual StudentCourse StudentCourse { get; set; }
        public DateTime Date { get; set; }
        public bool HasAttendance { get; set; }
    }
}
