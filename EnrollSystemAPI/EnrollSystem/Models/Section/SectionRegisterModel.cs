using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EnrollSystem.Models.Section
{
    public class SectionRegisterModel
    {
        [Required]
        public int TeacherId { get; set; }
        [Required]
        public int CourseId { get; set; }
        [Required]
        public DateTime StartDay { get; set; } // Ngày bắt đầu
        [Required]
        public DateTime EndDay { get; set; } // Ngày kết thúc
        [Range(1,10)]
        public int StartTime { get; set; } //ex: 1 (tiết 1)
        [Range(1,10)]
        public int EndTime { get; set; } //ex: 3 (tiết 3)
        [Required]
        public string Schedule { get; set; } // 0,2 (Thứ 2 + Thứ 4)
        public int MaxSlot { get; set; }
        [Required]
        public int RoomId { get; set; }
    }
}
