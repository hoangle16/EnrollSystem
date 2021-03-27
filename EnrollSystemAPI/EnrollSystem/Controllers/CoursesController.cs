using AutoMapper;
using EnrollSystem.Constants;
using EnrollSystem.Entities;
using EnrollSystem.Helpers;
using EnrollSystem.Interfaces;
using EnrollSystem.Models.Course;
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
    public class CoursesController : ControllerBase
    {
        private ICourseRepository _courseServie;
        private IMapper _mapper;
        public CoursesController(ICourseRepository service, IMapper mapper)
        {
            _mapper = mapper;
            _courseServie = service;
        }
        /// <summary>
        /// Get All Courses
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAll()
        {
            var courses = _courseServie.GetAll();
            var model = _mapper.Map<IList<CourseModel>>(courses);
            return Ok(model);
        }
        /// <summary>
        /// Get course by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var course = _courseServie.GetById(id);
            if (course == null) return NotFound();
            var model = _mapper.Map<CourseModel>(course);
            return Ok(model);
        }
        /// <summary>
        /// Create new Course
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = ROLE.Admin)]
        [HttpPost]
        public IActionResult Create([FromBody] CourseInputModel model)
        {
            var course = _mapper.Map<Course>(model);
            try
            {
                _courseServie.Create(course);
                return Ok(new { message = "Course Created" });
            }
            catch (AppException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        /// <summary>
        /// Update course
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = ROLE.Admin)]
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] CourseInputModel model)
        {
            var course = _mapper.Map<Course>(model);
            try
            {
                _courseServie.Update(id, course);
                return Ok(new { message = "Updated"});
            }
            catch(AppException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        /// <summary>
        /// Delete course
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = ROLE.Admin)]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _courseServie.Delete(id);
            return Ok(new { mesage = "Deleted" });
        }
    }
}
