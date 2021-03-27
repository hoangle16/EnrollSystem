using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnrollSystem.Entities
{
    public class Student
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<StudentSection> StudentSection { get; set; }
        public virtual ICollection<TrainingImage> TrainingImages { get; set; }
        public virtual ICollection<StudentSectionRegistration> StudentSectionRegistrations { get; set; }
    }
}
