using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnrollSystem.Models.Attendance
{
    public class TrainingImageModel
    {
        public int Id { get; set; }
        public int ImageId { get; set; }
        public string Path { get; set; }
        public int StudentId { get; set; }
        public string StudentUserName { get; set; }
    }
}
