using AutoMapper;
using EnrollSystem.Constants;
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
    }
}
