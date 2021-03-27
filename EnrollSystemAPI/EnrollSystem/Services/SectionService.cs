using AutoMapper;
using EnrollSystem.Data;
using EnrollSystem.Entities;
using EnrollSystem.Helpers;
using EnrollSystem.Interfaces;
using EnrollSystem.Models.Section;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnrollSystem.Services
{
    public class SectionService : ISectionRepository
    {
        private EnrollContext _context;
        private IMapper _mapper;
        public SectionService(EnrollContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public Section Create(Section section)
        {
            // validate schedule
            ValidateSection(section);
            //check conflict schedule, room
            var dbSection = _context.Sections.Where(e => e.RoomId == section.RoomId);
            var dbSchedule = _mapper.Map<IList<ScheduleModel>>(dbSection);
            var currentSchedule = _mapper.Map<ScheduleModel>(section);
            foreach(var db in dbSchedule)
            {
                if (CheckSchedule.IsConflict(currentSchedule, db))
                    throw new AppException("Schedule Conflict! Please choose another room!");
            }
            //check conflict schedule, teacher
            dbSection = _context.Sections.Where(e => e.TeacherId == section.TeacherId);
            dbSchedule = _mapper.Map<IList<ScheduleModel>>(dbSection);
            currentSchedule = _mapper.Map<ScheduleModel>(section);
            foreach (var db in dbSchedule)
            {
                if (CheckSchedule.IsConflict(currentSchedule, db))
                    throw new AppException("Schedule teacher Conflict! Please choose another teacher!");
            }
            // Add new Section
            _context.Sections.Add(section);
            _context.SaveChanges();
            return section;
        }

        public IEnumerable<Section> GetAll()
        {
            return _context.Sections;
        }

        public Section GetById(int sectionId)
        {
            return _context.Sections.Find(sectionId);
        }

        public void Update(int sectionId, Section sectionParams)
        {
            var section = _context.Sections.Find(sectionId);
            if (section == null)
                throw new AppException("Section not found!");
            //validate update info
            ValidateSection(sectionParams);
            //Check conflic schedule, room
            if (sectionParams.RoomId == 0)
                sectionParams.RoomId = section.RoomId;
            var dbSection = _context.Sections.Where(e => e.Id != sectionId).Where(e => e.RoomId == sectionParams.RoomId);
            var dbSchedule = _mapper.Map<IList<ScheduleModel>>(dbSection);
            var currentSchedule = _mapper.Map<ScheduleModel>(sectionParams);
            foreach (var db in dbSchedule)
            {
                if (CheckSchedule.IsConflict(currentSchedule, db))
                    throw new AppException("Schedule Conflict! Please choose another room!");
            }
            //check conflict schedule, teacher
            dbSection = _context.Sections.Where(e => e.Id != sectionId)
                .Where(e => e.TeacherId == sectionParams.TeacherId);
            dbSchedule = _mapper.Map<IList<ScheduleModel>>(dbSection);
            currentSchedule = _mapper.Map<ScheduleModel>(sectionParams);
            foreach (var db in dbSchedule)
            {
                if (CheckSchedule.IsConflict(currentSchedule, db))
                    throw new AppException("Schedule teacher Conflict! Please choose another teacher!");
                
            }
            //update info
            if (sectionParams.TeacherId != 0)
                section.TeacherId = sectionParams.TeacherId;
            if (sectionParams.CourseId != 0)
                section.CourseId = sectionParams.CourseId;
            if (sectionParams.StartDay != null)
                section.StartDay = sectionParams.StartDay;
            if (sectionParams.EndDay != null)
                section.EndDay = sectionParams.EndDay;
            if (sectionParams.StartTime != 0)
                section.StartTime = sectionParams.StartTime;
            if (sectionParams.EndTime != 0)
                section.EndTime = sectionParams.EndTime;
            if (!string.IsNullOrWhiteSpace(sectionParams.Schedule))
                section.Schedule = sectionParams.Schedule;
            if (sectionParams.RoomId != 0)
                section.RoomId = sectionParams.RoomId;

            _context.Sections.Update(section);
            _context.SaveChanges();
        }

        public void Delete(int sectionId)
        {
            var section = _context.Sections.Find(sectionId);
            if (section != null)
            {
                _context.Sections.Remove(section);
                _context.SaveChanges();
            }
        }
        private static void ValidateSection(Section section)
        {
            if (DateTime.Compare(section.StartDay, section.EndDay) >= 0)
                throw new AppException("StartDay and EndDay error!");
            if (section.StartTime > section.EndTime)
                throw new AppException("StartTime and EndTime error!");
            if (section.StartTime < 0 || section.StartTime > 10)
                throw new AppException("StartTime must greater than 0 and less than or equal to 10");
            if (section.EndTime < 0 || section.EndTime > 10)
                throw new AppException("EndTime must greater than 0 and less than or equal to 10");
            string[] schedules = section.Schedule.Split(',');
            foreach (var schedule in schedules)
            {
                int sche = int.Parse(schedule);
                if (sche < 0 || sche > 6)
                    throw new AppException("Schedule error!");
            }
            if (section.MaxSlot < 10)
                throw new AppException("Maxslot mush greater than or equal 10");
        }
    }
}
