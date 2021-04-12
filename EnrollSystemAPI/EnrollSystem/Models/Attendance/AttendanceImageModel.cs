using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnrollSystem.Models.Attendance
{
    public class AttendanceImageModel
    {
        public int Id { get; set; }
        public int ImageId { get; set; }
        public string Path { get; set; }
        public int SectionId { get; set; }
        public DateTime Date { get; set; }
    }
}
