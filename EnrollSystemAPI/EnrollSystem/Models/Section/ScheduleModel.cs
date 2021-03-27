using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnrollSystem.Models.Section
{
    public class ScheduleModel
    {
        public DateTime StartDay { get; set; } // Ngày bắt đầu
        public DateTime EndDay { get; set; } // Ngày kết thúc
        public int StartTime { get; set; } //ex: 1 (tiết 1)
        public int EndTime { get; set; } //ex: 3 (tiết 3)
        public string Schedule { get; set; } // 0,2 (CN + Thứ 3)
    }
}
