using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnrollSystem.Entities
{
    public class Attendance
    {
        public int Id { get; set; }
        public int StudentClassId { get; set; }
        public virtual StudentClass StudentClass { get; set; }
        public DateTime Date { get; set; }
        public bool HasAttendance { get; set; }
    }
}
