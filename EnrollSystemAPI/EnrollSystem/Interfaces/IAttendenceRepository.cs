using EnrollSystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnrollSystem.Interfaces
{
    public interface IAttendenceRepository
    {
        void TraningFace(int userId, IEnumerable<Image> images);
        IEnumerable<Attendance> AttendanceFace(int sectionId, IEnumerable<Image> images, DateTime dateTime);
    }
}
