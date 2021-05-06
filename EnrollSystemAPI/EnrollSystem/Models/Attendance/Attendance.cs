using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnrollSystem.Models.Attendance
{
    public class AttendanceNewModel
    {

        public DateTime Date { get; set; }
        public int SectionId { get; set; }
        public List<AttendanceItem> Items { get; set; }
    }
    public class AttendanceItem 
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public bool HasAttendance { get; set; }
    }

}
