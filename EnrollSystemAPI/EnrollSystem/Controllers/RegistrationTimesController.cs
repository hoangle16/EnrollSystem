using EnrollSystem.Constants;
using EnrollSystem.Interfaces;
using EnrollSystem.Models.RegistrationTime;
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
    public class RegistrationTimesController : ControllerBase
    {
        private IRegistrationTimeRepository _service;
        public RegistrationTimesController(IRegistrationTimeRepository service)
        {
            _service = service;
        }
        /// <summary>
        /// Get Registration time
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetRegistrationTime()
        {
            var regis = _service.GetRegistrationTime();
            if (regis == null) 
            {
                return Ok(new { mesage = "Now is not the time to enroll!" });
            }
            return Ok(new
            {
                StartDateTime = regis.StartDateTime,
                EndDateTime = regis.EndDateTime
            });
        }
        /// <summary>
        /// Set Registration time
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = ROLE.Admin)]
        [HttpPut]
        public IActionResult SetTime([FromBody] RegistrationTimeInputModel model)
        {
            try
            {
                _service.SetTime(model.StartDateTime, model.EndDateTime);
                return Ok();
            }catch(Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        /// <summary>
        /// Close Registration time
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = ROLE.Admin)]
        [HttpPut("close")]
        public IActionResult Close()
        {
            _service.CloseRegistration();
            return Ok();
        }
    }
}
