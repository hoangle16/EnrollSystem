using EnrollSystem.Data;
using EnrollSystem.Entities;
using EnrollSystem.Helpers;
using EnrollSystem.IdentityFaces;
using EnrollSystem.Interfaces;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.IO;
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
            var result = _context.Attendances.Where(e => e.StudentSection.SectionId == sectionId)
                .OrderBy(e => e.Date.Date).ThenBy(e => e.StudentSection.Student.User.UserName).ToList();
            //var result = _context.Attendances.Where(e => e.StudentSection.SectionId == sectionId)
            //   .OrderBy(e => e.StudentSection.Student.User.UserName).ThenBy(e => e.Date.Date).ToList();
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
        public IEnumerable<TrainingImage> GetTrainingImage(int studentId)
        {
            var result = _context.TrainingImages.Where(e => e.StudentId == studentId).ToList();
            return result;
        }
        public void DeleteTrainingImage(int trainingImageId)
        {
            var trainingImage = _context.TrainingImages.Find(trainingImageId);
            if (trainingImage != null)
            {
                var _image = _context.Images.Find(trainingImage.ImageId);
                _context.TrainingImages.Remove(trainingImage);
                _context.Images.Remove(_image);
                _context.SaveChanges();
            }
        }
        public IEnumerable<AttendanceImage> GetAttendanceImages(int sectionId, DateTime dateTime)
        {
            var _images = _context.AttendanceImages.Where(e => e.SectionId == sectionId)
                .Where(e => e.Date.Date == dateTime.Date);
            return _images;
        }
        public IEnumerable<AttendanceImage> GetAttendanceImages(int sectionId)
        {
            var _images = _context.AttendanceImages.Where(e => e.SectionId == sectionId);
            return _images;
        }
        public Attendance ChangeAttendance(int attendanceId)
        {
            var attendance = _context.Attendances.Find(attendanceId);
            if (attendance == null)
                throw new AppException("Not Found!");
            attendance.HasAttendance = !attendance.HasAttendance;
            _context.Attendances.Update(attendance);
            _context.SaveChanges();
            return attendance;
        }
        public IEnumerable<Attendance> GetMyAttendanceList(int userId, int sectionId)
        {
            var attendances = _context.Attendances.Where(e => e.StudentSection.Student.UserId == userId && e.StudentSection.SectionId == sectionId).OrderByDescending(e => e.Date);
            return attendances;
        }
        //export report
        public FileContentResult ExportAttendanceReport(int sectionId)
        {
            //order by username and date
            var attendanceList = _context.Attendances.Where(e => e.StudentSection.SectionId == sectionId)
                .OrderBy(e => e.StudentSection.Student.User.UserName).ThenBy(e => e.Date.Date).ToList();

            //create excel instance
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var memoryStream = new MemoryStream();
            ExcelPackage excel = new ExcelPackage(memoryStream);

            string courseName = attendanceList.First().StudentSection.Section.Course.Name;
            var section = attendanceList.First().StudentSection.Section;
            var attendanceDate = attendanceList.GroupBy(e => e.Date)
                .Select(e => e.First()).Select(e => e.Date).ToList();
            //set title 
            excel.Workbook.Properties.Title = $"{sectionId} - {courseName}";

            //name of the sheet
            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
            

            //setting the properties of the work sheet
            workSheet.TabColor = System.Drawing.Color.Black;
            workSheet.DefaultRowHeight = 12;

            //sheet info
            workSheet.Cells[1, 1].Value = "Học phần";
            workSheet.Cells[1, 2].Value = courseName;
            workSheet.Cells[2, 1].Value = "Mã học phần";
            workSheet.Cells[2, 2].Value = sectionId.ToString();
            workSheet.Cells[3, 1].Value = "Giáo viên";
            workSheet.Cells[3, 2].Value = section.Teacher.User.Name;
            workSheet.Row(1).Style.Font.Bold = true;
            workSheet.Row(2).Style.Font.Bold = true;
            workSheet.Row(3).Style.Font.Bold = true;
            workSheet.Cells["A1:B3"].Style.Font.Color.SetColor(System.Drawing.Color.Red);

            workSheet.Column(1).AutoFit();
            workSheet.Column(2).AutoFit();
            workSheet.Column(3).AutoFit();

            //sheet header
            workSheet.Row(4).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells["A4:C4"].Style.Fill.SetBackground(System.Drawing.Color.LightGray);
            workSheet.Row(4).Style.Font.Bold = true;
            workSheet.Cells[4, 1].Value = "STT";
            workSheet.Cells[4, 2].Value = "MSSV";
            workSheet.Cells[4, 3].Value = "Họ tên";
            int colIndex = 4;
            foreach(var date in attendanceDate)
            {
                //workSheet.Cells[4, colIndex].Style.Numberformat.Format = "dd/MM/yyyy";
                workSheet.Cells[4, colIndex].Style.Fill.SetBackground(System.Drawing.Color.LightGray);
                workSheet.Cells[4, colIndex].Value = date.ToString("dd/MM/yy");
                colIndex++;
            }

            //insert date
            int rowIndex = 5;
            int stt = 1;
            int numOfStudent = _context.Sections.Find(sectionId).Slot;
            int numOfCol = attendanceDate.Count+3;
            int iol = 0; //indexOfAttendanceList
            for (int i = 0;i<numOfStudent; i++)
            {
                colIndex = 1;
                workSheet.Cells[rowIndex, colIndex++].Value = stt;
                stt++;
                workSheet.Cells[rowIndex, colIndex++].Value = attendanceList[iol].StudentSection.Student.User.UserName;
                workSheet.Cells[rowIndex, colIndex++].Value = attendanceList[iol].StudentSection.Student.User.Name;
                for (int j = 3; j < numOfCol; j++)
                {
                    workSheet.Cells[rowIndex, colIndex++].Value = attendanceList[iol].HasAttendance ? "C" : "V";
                    iol++;
                }
                rowIndex++;
            }
            //set boder 
            workSheet.Cells[4, 1, rowIndex - 1, numOfCol].Style.Border.BorderAround(ExcelBorderStyle.Thin);

            //total by date
            rowIndex++;
            colIndex = 2;

            workSheet.Row(rowIndex).Style.Font.Color.SetColor(System.Drawing.Color.DarkBlue);
            workSheet.Row(rowIndex).Style.Font.Bold = true;
            workSheet.Row(rowIndex+1).Style.Font.Color.SetColor(System.Drawing.Color.DarkBlue);
            workSheet.Row(rowIndex+1).Style.Font.Bold = true;

            workSheet.Cells[rowIndex, colIndex].Style.Fill.SetBackground(System.Drawing.Color.Yellow);
            var startCell = workSheet.Cells[rowIndex, colIndex];
            workSheet.Cells[rowIndex, colIndex++].Value = "Tổng";
            workSheet.Cells[rowIndex, colIndex].Style.Fill.SetBackground(System.Drawing.Color.Yellow);
            workSheet.Cells[rowIndex, colIndex].Value = "Có mặt";
            workSheet.Cells[rowIndex+1, colIndex].Style.Fill.SetBackground(System.Drawing.Color.Yellow);
            workSheet.Cells[rowIndex+1, colIndex++].Value = "Vắng";

            foreach (var date in attendanceDate)
            {
                workSheet.Cells[rowIndex, colIndex].Style.Fill.SetBackground(System.Drawing.Color.Yellow);
                workSheet.Cells[rowIndex, colIndex].Value = attendanceList.Where(e => e.Date.Date == date.Date).Where(e => e.HasAttendance).ToList().Count;
                workSheet.Cells[rowIndex+1, colIndex].Style.Fill.SetBackground(System.Drawing.Color.Yellow);
                workSheet.Cells[rowIndex+1, colIndex++].Value = attendanceList.Where(e => e.Date.Date == date.Date).Where(e => !e.HasAttendance).ToList().Count;
            }

            //total by student
            colIndex = numOfCol + 2;
            rowIndex = 4;
            workSheet.Column(colIndex).Style.Font.Color.SetColor(System.Drawing.Color.DarkBlue);
            workSheet.Cells[rowIndex, colIndex].Value = "Vắng";
            workSheet.Cells[rowIndex++, colIndex].Style.Fill.SetBackground(System.Drawing.Color.Yellow);

            int _col = 4;
            for(int i =0; i<numOfStudent; i++)
            {
                workSheet.Cells[rowIndex, colIndex].Style.Fill.SetBackground(System.Drawing.Color.Yellow);
                workSheet.Cells[rowIndex, colIndex].Style.Font.Bold = true;
                workSheet.Cells[rowIndex, colIndex].Formula = $"COUNTIF({workSheet.Cells[rowIndex, _col].Address}:{workSheet.Cells[rowIndex, _col+attendanceDate.Count]}, \"V\")";
                rowIndex++;

            }

            excel.Save();
            var result = excel.GetAsByteArray();
            var excelFile = new FileContentResult(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = "diemdanh.xlsx"
            };
            return excelFile;
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
