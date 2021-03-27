using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnrollSystem.Entities
{
    public class StudentSection
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public virtual Student Student { get; set; }
        public int SectionId { get; set; }
        public virtual Section Section { get; set; }
        public virtual ICollection<Attendance> Attendances { get; set; }
    }
}
