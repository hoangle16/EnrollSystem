using EnrollSystem.Data;
using EnrollSystem.Entities;
using EnrollSystem.Helpers;
using EnrollSystem.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnrollSystem.Services
{
    public class StudentSectionRegistrationService : IStudentSectionRegistrationRepository
    {
        private EnrollContext _context;
        public StudentSectionRegistrationService(EnrollContext context)
        {
            _context = context;
        }
        public void Approval(int sectionId)
        {
            var RegistrationList = _context.StudentSectionRegistrations.Where(e => e.SectionId == sectionId);
            foreach(var el in RegistrationList)
            {
                el.HasApproval = true;
                //create studentsection
                StudentSection temp = new StudentSection();
                temp.StudentId = el.StudentId;
                temp.SectionId = el.SectionId;
                _context.StudentSections.Add(temp);

            }
            _context.StudentSectionRegistrations.UpdateRange(RegistrationList);
            _context.SaveChanges();
        }

        public void DeleteRegister(int studentId, int sectionId)
        {
            var reg = _context.StudentSectionRegistrations.SingleOrDefault(e => e.StudentId == studentId && e.SectionId == sectionId);
            if (reg != null)
                _context.StudentSectionRegistrations.Remove(reg);
            var section = _context.Sections.Find(sectionId);
            section.Slot--;
            _context.Sections.Update(section);
            _context.SaveChanges();
        }

        public IEnumerable<StudentSectionRegistration> GetListBySectionId(int sectionId)
        {
            return _context.StudentSectionRegistrations.Where(e => e.SectionId == sectionId);
        }

        public IEnumerable<Section> GetSectionForRegistration(int studentId)
        {
            if (studentId == 0)
            {
                return _context.Sections.Where(e => e.StartDay.Date > DateTime.Now.Date);
            }
            var sections = _context.Sections.Where(e => e.StartDay.Date > DateTime.Now.Date)
               .Where(e => !e.StudentSectionRegistrations.Any(ec => ec.StudentId == studentId));
            return sections;
        }

        public IEnumerable<StudentSectionRegistration> GetListByStudentId(int studentId)
        {
            return _context.StudentSectionRegistrations.Where(e => e.StudentId == studentId);
        }
        public StudentSectionRegistration Registration(int studentId, int sectionId)
        {
            //check registration time
            var registrationTime = _context.RegistrationTimes.FirstOrDefault();
            if (registrationTime == null || DateTime.Compare(DateTime.Now, registrationTime.StartDateTime)<0 || DateTime.Compare(DateTime.Now, registrationTime.EndDateTime)>0)
            {
                throw new AppException("Now is not the time to enroll!");
            }
            //
            if (!_context.Students.Any(e => e.Id == studentId))
                throw new AppException("Student not found!");
            if (!_context.Sections.Any(e => e.Id == sectionId))
                throw new AppException("Section not found!");
            var section = _context.Sections.Find(sectionId);
            if (_context.StudentSectionRegistrations.Where(e => e.StudentId == studentId && e.SectionId == sectionId).Any())
                throw new AppException("Section already registered");
            if (section.MaxSlot == section.Slot)
                throw new AppException("Section has full!!!");
            //check overlap schedule

            string[] dayOfWeek = section.Schedule.Split(",");
            var dbRegistrationList = _context.StudentSectionRegistrations.Where(e => e.StudentId == studentId).Where(e => section.StartDay.Date <= e.Section.EndDay.Date && e.Section.StartDay.Date <= section.EndDay.Date)
                .Where(e => section.StartTime <= e.Section.EndTime && e.Section.StartTime <= section.EndTime);
            foreach(var day in dayOfWeek)
            {
                if (dbRegistrationList.Any(e => e.Section.Schedule.Contains(day)))
                    throw new AppException("Schedule Confilict!");
            }
            //registration
            StudentSectionRegistration newReg = new StudentSectionRegistration();
            newReg.StudentId = studentId;
            newReg.SectionId = sectionId;
            newReg.HasApproval = false;
            _context.StudentSectionRegistrations.Add(newReg);
            section.Slot++;
            _context.Sections.Update(section);
            _context.SaveChanges();
            return newReg;
        }
        public int GetUserIdByStudentId(int studentId)
        {
            var student = _context.Students.Find(studentId);
            if (student != null)
            {
                return student.UserId;
            }
            return -1;
        }
    }
}
