using AutoMapper;
using EnrollSystem.Constants;
using EnrollSystem.Helpers;
using EnrollSystem.Interfaces;
using EnrollSystem.Models.Section;
using EnrollSystem.Models.Teacher;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnrollSystem.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TeachersController : ControllerBase
    {
        private ITeacherRepository _teacherService;
        private IMapper _mapper;
        public TeachersController(ITeacherRepository teacherService, IMapper mapper)
        {
            _teacherService = teacherService;
            _mapper = mapper;
        }
        /// <summary>
        /// Get all Teacher
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAll()
        {
            var teachers = _teacherService.GetAll();
            var models = _mapper.Map<IList<TeacherModel>>(teachers);
            return Ok(models);
        }
        /// <summary>
        /// Get teacher by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var teacher = _teacherService.GetById(id);
            if (teacher == null) return NotFound();
            var model = _mapper.Map<TeacherModel>(teacher);
            return Ok(model);
        }
        /// <summary>
        /// Get list of available teachers at schedule
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet("ready")]
        public IActionResult GetTeacherReady([FromQuery] ScheduleModel model)
        {
            var teaches = _teacherService.GetListTeacherFree(model);
            var _model = _mapper.Map<IList<TeacherModel>>(teaches);
            return Ok(_model);
        }
        /// <summary>
        /// Get Teacher's sections by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = ROLE.Admin)]
        [HttpGet("sections/{id}")]
        public IActionResult GetTeacherSections(int id)
        {
            var sections = _teacherService.GetMySections(id);
            if (sections == null) return NotFound();
            var models = _mapper.Map<IList<SectionModel>>(sections);
            return Ok(models);
        }
        /// <summary>
        /// Get My Sections
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = ROLE.Teacher)]
        [HttpGet("sections")]
        public IActionResult GetMySections()
        {
            var currentUserId = int.Parse(User.Identity.Name);
            int teacherId = _teacherService.GetTeacherIdViaUserId(currentUserId);
            var sections = _teacherService.GetMySections(teacherId);
            if (sections == null) return NotFound();
            var models = _mapper.Map<IList<SectionModel>>(sections);
            return Ok(models);
        }
        /// <summary>
        /// Get calendar item 
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = ROLE.Teacher)]
        [HttpGet("calendar")]
        public IActionResult GetCalendar()
        {
            var currentUserId = int.Parse(User.Identity.Name);
            int teacherId = _teacherService.GetTeacherIdViaUserId(currentUserId);
            var sections = _teacherService.GetMySections(teacherId);
            if (sections == null) return NotFound();
            var models = _mapper.Map<IList<CalendarModel>>(sections);
            int index = 0;
            foreach(var section in sections)
            {
                List<Event> eventList = new List<Event>();
                var schedule = _mapper.Map<ScheduleModel>(section);
                var listDate = CheckSchedule.ListDate(schedule);
                foreach(var _date in listDate)
                {
                    Event newEvent = new Event();
                    newEvent.Name = $"{section.Course.Name} | {section.Room.Name}";
                    if (schedule.StartTime > 5)
                    {
                        newEvent.Start = _date.Date.AddHours(schedule.StartTime + 7);
                        newEvent.End = _date.Date.AddHours(schedule.EndTime + 8);
                    }
                    else
                    {
                        newEvent.Start = _date.Date.AddHours(schedule.StartTime + 6);
                        newEvent.End = _date.Date.AddHours(schedule.EndTime + 7);
                    }
                    eventList.Add(newEvent);
                }
                models[index].Events = eventList;
                index++;
            }
            
            return Ok(models);
        }
    }
}
