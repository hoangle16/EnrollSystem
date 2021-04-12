using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnrollSystem.Models.Attendance
{
    public class AttendanceModel
    {
        public int Id { get; set; }
        public int SectionId { get; set; }
        public int StudentId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; } 
        public DateTime Date { get; set; }
        public bool HasAttendance { get; set; }
    }
}
