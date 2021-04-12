using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EnrollSystem.Models.Attendance
{
    public class AttendanceImagesUpload
    {
        [Required]
        public int SectionId { get; set; }
        [Required]
        public List<IFormFile> Images { get; set; }
        public DateTime DateTime { get; set; }
    }
}
