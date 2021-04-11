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
        IEnumerable<Attendance> GetAttendanceListBySectionId(int sectionId);
        IEnumerable<Attendance> GetAttendanceListBy(int sectionId, DateTime dateTime);
        IEnumerable<Attendance> EditAttendance(int sectionId, DateTime dateTime, List<int> studentIdList);
        IEnumerable<TrainingImage> GetTrainingImage(int studentId);
        void DeleteTrainingImage(int trainingImageId); // trainingImageId
        IEnumerable<AttendanceImage> GetAttendanceImages(int sectionId, DateTime dateTime);
        IEnumerable<AttendanceImage> GetAttendanceImages(int sectionId);
    }
}
