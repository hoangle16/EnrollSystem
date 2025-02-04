﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnrollSystem.Entities
{
    public class Section
    {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public virtual Teacher Teacher { get; set; }
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }
        public DateTime StartDay { get; set; } // Ngày bắt đầu
        public DateTime EndDay { get; set; } // Ngày kết thúc
        public int StartTime { get; set; } //ex: 1 (tiết 1)
        public int EndTime { get; set; } //ex: 3 (tiết 3)
        public string Schedule { get; set; } // 0,2 (CN + Thứ 3)
        public int Slot { get; set; }
        public int MaxSlot { get; set; }
        public int RoomId { get; set; }
        public virtual Room Room { get; set; }
        public virtual ICollection<StudentSection> StudentSections { get; set; }
        public virtual ICollection<AttendanceImage> AttendanceImages { get; set; }
        public virtual ICollection<StudentSectionRegistration> StudentSectionRegistrations { get; set; }
    }
}
