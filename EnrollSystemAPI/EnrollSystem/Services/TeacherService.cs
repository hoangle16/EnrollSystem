using AutoMapper;
using EnrollSystem.Data;
using EnrollSystem.Entities;
using EnrollSystem.Interfaces;
using EnrollSystem.Models.Section;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnrollSystem.Services
{
    public class TeacherService : ITeacherRepository
    {
        private EnrollContext _context;
        private IMapper _mapper;
        public TeacherService(EnrollContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public IEnumerable<Teacher> GetAll()
        {
            return _context.Teachers;
        }

        public Teacher GetById(int teacherId)
        {
            return _context.Teachers.Find(teacherId);
        }

        public IEnumerable<Teacher> GetListTeacherFree(ScheduleModel schedule)
        {
            //get teachers
            var teachers = _context.Teachers.ToList();
            /*check teacher overlap schedule*/
            string[] dayOfWeek = schedule.Schedule.Split(",");
            var dbSection = _context.Sections.Where(e => schedule.StartDay.Date <= e.EndDay.Date && e.StartDay.Date <= schedule.EndDay.Date)
                .Where(e => schedule.StartTime <= e.EndTime && e.StartTime <= schedule.EndTime);
            //overlap schedule
            List<Section> newSection = new List<Section>();
            foreach(var day in dayOfWeek)
            {
                var overlapSectionList = dbSection.Where(e => e.Schedule.Contains(day)).ToList();
                newSection.AddRange(overlapSectionList);
            }
            var _teachersInSection = _mapper.Map<IList<Teacher>>(newSection);
            //remove duplicate
            _teachersInSection = _teachersInSection.GroupBy(e => e.Id).Select(x => x.First()).ToList();
            foreach( var overlapTeacher in _teachersInSection)
            {
                teachers.RemoveAll(e => e.Id == overlapTeacher.Id);
            }
            return teachers;
        }
    }
}
