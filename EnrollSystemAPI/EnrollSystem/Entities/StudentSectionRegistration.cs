using EnrollSystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnrollSystem.Entities
{
    public class StudentSectionRegistration
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public virtual Student Student { get; set; }
        public int SectionId { get; set; }
        public virtual Section Section { get; set; }
        public DateTime Deadline { get; set; }
        public bool HasApproval { get; set; }
    }
}
