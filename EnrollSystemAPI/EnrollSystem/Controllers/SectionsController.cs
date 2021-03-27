using AutoMapper;
using EnrollSystem.Constants;
using EnrollSystem.Entities;
using EnrollSystem.Helpers;
using EnrollSystem.Interfaces;
using EnrollSystem.Models.Section;
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
    public class SectionsController : ControllerBase
    {
        private ISectionRepository _sectionService;
        private IMapper _mapper;
        public SectionsController(ISectionRepository service, IMapper mapper)
        {
            _sectionService = service;
            _mapper = mapper;
        }
        /// <summary>
        /// Get all sections
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAll()
        {
            var sections = _sectionService.GetAll();
            var model = _mapper.Map<IList<SectionModel>>(sections);
            return Ok(model);
        }
        /// <summary>
        /// Get section by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var section = _sectionService.GetById(id);
            var model = _mapper.Map<SectionModel>(section);
            return Ok(model);
        }
        /// <summary>
        /// Create new Section
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = ROLE.Admin)]
        [HttpPost]
        public IActionResult Create([FromBody] SectionRegisterModel model)
        {
            var section = _mapper.Map<Section>(model);
            try
            {
                _sectionService.Create(section);
                return Ok(new { message = "Section Created" });
            }
            catch(AppException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        /// <summary>
        /// Update section
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = ROLE.Admin)]
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] SectionUpdateModel model)
        {
            var section = _mapper.Map<Section>(model);
            try
            {
                _sectionService.Update(id, section);
                return Ok(new { message = "Section updated" });
            }
            catch(AppException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        /// <summary>
        /// Delete Section by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = ROLE.Admin)]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _sectionService.Delete(id);
            return Ok(new { message = "Section deleted" });
        }
    }
}
