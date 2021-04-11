using EnrollSystem.Data;
using EnrollSystem.Entities;
using EnrollSystem.Helpers;
using EnrollSystem.IdentityFaces;
using EnrollSystem.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnrollSystem.Services
{
    public class AttendenceService : IAttendenceRepository
    {
        private EnrollContext _context;
        private IdentityFace _identityFace;
        public AttendenceService(EnrollContext context)
        {
            _context = context;
        }
        public void TraningFace(int userId, IEnumerable<Image> images)
        {
            _identityFace = new IdentityFace();
            List<string> imagesPath = images.Select(e => e.Path).ToList();
            var student = _context.Students.Where(e => e.UserId == userId).Single();
            List<string> trainedPath = _identityFace.Train(imagesPath, student.Id, student.User.UserName);

            //create Image
            List<Image> imgs = new List<Image>();
            for (int i = 0; i < trainedPath.Count; i++)
            {
                Image img = new Image();
                img.Path = trainedPath[i];
                imgs.Add(img);
            }
            _context.Images.AddRange(imgs);
            _context.SaveChanges();
            //create TrainingImage
            List<TrainingImage> trainingImages = new List<TrainingImage>();
            for (int i = 0; i < imgs.Count; i++)
            {
                TrainingImage trainingImage = new TrainingImage();
                trainingImage.ImageId = imgs[i].Id;
                trainingImage.StudentId = student.Id;
                trainingImages.Add(trainingImage);
            }
            _context.TrainingImages.AddRange(trainingImages);
            _context.SaveChanges();
        }

        public IEnumerable<Attendance> AttendanceFace(int sectionId, IEnumerable<Image> images, DateTime dateTime)
        {
            //check is_dateTime in section's schedule
            //
            _identityFace = new IdentityFace();
            var trainingImages = _context.TrainingImages.Where(e => e.Student.StudentSection.Any(e => e.SectionId == sectionId)).ToList();
            List<string> imagesPath = images.Select(e => e.Path).ToList();
            //recognition
            List<string> recognitionResult = _identityFace.Recognition(trainingImages, imagesPath);
            //create attendanceImage 
            CreateAttendanceImage(sectionId, images, dateTime);
            //attendance studentId list
            List<int> hasAttendanceList = new List<int>();
            foreach (var item in recognitionResult)
            {
                if (item != "Unknown")
                {
                    //format recognition result: studentId_username
                    var Ar = item.Split("_");
                    //add studentId into list
                    hasAttendanceList.Add(int.Parse(Ar[0]));
                }
            }

            List<Attendance> attendances = new List<Attendance>();
            //find all StudentSection
            var studentSections = _context.StudentSections.Where(e => e.SectionId == sectionId);
            if (studentSections == null)
                throw new AppException("Not found any Student in Section!");
            bool isCreated = _context.Attendances.Where(e => e.Date.Date == dateTime.Date).Any(e => e.StudentSectionId == studentSections.First().Id);
            //Create Attendances record for everyone in section

            if (isCreated) //update
            {
                foreach (var ss in studentSections)
                {
                    var att = _context.Attendances.Where(e => e.StudentSectionId == ss.Id)
                        .Where(e => e.Date.Date == dateTime.Date)
                        .SingleOrDefault();
                    if (att.HasAttendance == false && hasAttendanceList.Any(e => e == ss.StudentId))
                    {
                        att.HasAttendance = true;
                        _context.Attendances.Update(att);
                    }
                    attendances.Add(att);
                }

            }
            else //create 
            {
                foreach (var ss in studentSections)
                {
                    Attendance att = new Attendance();
                    att.StudentSectionId = ss.Id;
                    att.Date = dateTime;
                    //check hasAttendance
                    if (hasAttendanceList.Any(e => e == ss.StudentId))
                        att.HasAttendance = true;
                    else att.HasAttendance = false;
                    attendances.Add(att);
                }
                _context.Attendances.AddRange(attendances);
            }
            _context.SaveChanges();
            return attendances;
        }

        public IEnumerable<Attendance> GetAttendanceListBySectionId(int sectionId)
        {
            var result = _context.Attendances.Where(e => e.StudentSection.SectionId == sectionId).ToList();
            return result;
        }
        public IEnumerable<Attendance> GetAttendanceListBy(int sectionId, DateTime dateTime)
        {
            var result = _context.Attendances.Where(e => e.StudentSection.SectionId == sectionId)
                .Where(e => e.Date.Date == dateTime.Date).ToList();
            return result;
        }
        public IEnumerable<Attendance> EditAttendance(int sectionId, DateTime dateTime, List<int> studentIdList)
        {
            List<Attendance> attendances = new List<Attendance>();
            var attendanceList = _context.Attendances.Where(e => e.StudentSection.SectionId == sectionId)
                .Where(e => e.Date.Date == dateTime.Date);
            //set
            foreach(var attendance in attendanceList)
            {
                attendance.HasAttendance = false;
            }
            foreach(var studentId in studentIdList)
            {
                var att = attendanceList.Where(e => e.StudentSection.StudentId == studentId).SingleOrDefault();
                if (att != null)
                    att.HasAttendance = true;
                attendances.Add(att);
            }
            _context.Attendances.UpdateRange(attendances);
            _context.SaveChanges();
            return attendances;

        }
        //private
        private void CreateAttendanceImage(int sectionId, IEnumerable<Image> Images, DateTime date)
        {
            if (!_context.Sections.Any(e => e.Id == sectionId))
                throw new AppException("Section not found!");
            foreach (var image in Images)
            {
                if (!_context.Images.Any(e => e.Id == image.Id))
                    throw new AppException("Image path error!");
                AttendanceImage _img = new AttendanceImage();
                _img.SectionId = sectionId;
                _img.ImageId = image.Id;
                _img.Date = date;

                _context.AttendanceImages.Add(_img);
            }
            _context.SaveChanges();

        }
    }
}
