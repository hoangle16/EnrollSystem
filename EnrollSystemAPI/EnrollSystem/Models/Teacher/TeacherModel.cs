using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnrollSystem.Models.Teacher
{
    public class TeacherModel
    {
        public int TeacherId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Avatar { get; set; }
        public string Name { get; set; }
        public bool Gender { get; set; }
        public string IdNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }
    }
}
