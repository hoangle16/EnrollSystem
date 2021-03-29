using AutoMapper;
using EnrollSystem.Entities;
using EnrollSystem.Interfaces;
using EnrollSystem.Models.Student;
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
    public class StudentsController : ControllerBase
    {
        private IStudentRepository _studentService;
        private IMapper _mapper;
        public StudentsController(IStudentRepository studentService, IMapper mapper)
        {
            _studentService = studentService;
            _mapper = mapper;
        }
        /// <summary>
        /// Get stduent's list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAll()
        {
            var students = _studentService.GetAll();
            var models = _mapper.Map<IList<StudentModel>>(students);
            return Ok(models);
        }
        /// <summary>
        /// Get Student by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var student = _studentService.GetById(id);
            if (student == null) return NotFound();
            var model = _mapper.Map<StudentModel>(student);
            return Ok(model);
        }
    }
}
