using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnrollSystem.Models.StudentSectionRegistration
{
    public class StudentSectionRegModel
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string StudentUserName { get; set; }
        public string StudentName { get; set; }
        public int SectionId { get; set; }
        public string CourseName { get; set; }
        public string SectionTeacherName { get; set; }
        public DateTime StartDay { get; set; } // Ngày bắt đầu
        public DateTime EndDay { get; set; } // Ngày kết thúc
        public int StartTime { get; set; } //ex: 1 (tiết 1)
        public int EndTime { get; set; } //ex: 3 (tiết 3)
        public string Schedule { get; set; } // 0,2 (CN + Thứ 3)
        public int Slot { get; set; }
        public int MaxSlot { get; set; }
        public string Room { get; set; }
        public bool HasApproval { get; set; }
    }
}
