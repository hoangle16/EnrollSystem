using AutoMapper;
using EnrollSystem.Constants;
using EnrollSystem.Interfaces;
using EnrollSystem.Models.Section;
using EnrollSystem.Models.StudentSectionRegistration;
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
    public class SectionRegistersController : ControllerBase
    {
        private IStudentSectionRegistrationRepository _service;
        private IMapper _mapper;
        public SectionRegistersController(IStudentSectionRegistrationRepository service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }
        /// <summary>
        /// Get Register list by studentId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("students/{id}")]
        public IActionResult GetRegisterListByStudentId(int id)
        {
            var list = _service.GetListByStudentId(id);
            var models = _mapper.Map<IList<StudentSectionRegModel>>(list);
            return Ok(models);
        }
        /// <summary>
        /// Get Register list by sectionId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("sections/{id}")]
        public IActionResult GetRegisterListBySectionId(int id)
        {
            var list = _service.GetListBySectionId(id);
            var models = _mapper.Map<IList<StudentSectionRegModel>>(list);
            return Ok(models);
        }
        /// <summary>
        /// Get Sections for register by studentId. If studentId = 0, return all section for register.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("sectionslist/{id}")]
        public IActionResult GetSectionForRegistration(int id) //studentId
        {
            var sections = _service.GetSectionForRegistration(id);
            var models = _mapper.Map<IList<SectionModel>>(sections);
            return Ok(models);
        }
        /// <summary>
        /// Register section
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = ROLE.Admin+","+ROLE.Student)]
        [HttpPost]
        public IActionResult Registration([FromBody] RegistrationModel model)
        {
            if (User.IsInRole(ROLE.Student))
            {
                var currentUserId = int.Parse(User.Identity.Name);
                var studentId = _service.GetUserIdByStudentId(currentUserId);
                model.StudentId = studentId;
            }
            try
            {
                _service.Registration(model.StudentId, model.SectionId);
                return Ok(new { message = "Registration" });
            }
            catch(Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        /// <summary>
        /// Delete student from section registration
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="sectionId"></param>
        /// <returns></returns>
        [Authorize(Roles =ROLE.Admin+","+ROLE.Student)]
        [HttpDelete("{studentId}/{sectionId}")]
        public IActionResult DeleteRegister(int studentId, int sectionId)
        {
            if (User.IsInRole(ROLE.Student))
            {
                var currentUserId = int.Parse(User.Identity.Name);
                var userId = _service.GetUserIdByStudentId(studentId);
                if (currentUserId != userId)
                    return Forbid();
            }
            _service.DeleteRegister(studentId, sectionId);
            return Ok(new { message = "Deleted" });
        }
        /// <summary>
        /// Approval by sectionId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = ROLE.Admin)] 
        [HttpPut("approval/{id}")] //sectionId
        public IActionResult Approval(int id)
        {
            _service.Approval(id);
            return Ok();
        }
    }
}
