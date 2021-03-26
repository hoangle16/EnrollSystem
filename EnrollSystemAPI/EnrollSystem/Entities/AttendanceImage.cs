using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnrollSystem.Entities
{
    public class AttendanceImage
    {
        public int Id { get; set; }
        public int ImageId { get; set; }
        public virtual Image Image { get; set; }
        public int ClassId { get; set; }
        public virtual Class Class { get; set; }
        public DateTime Date { get; set; }
    }
}
