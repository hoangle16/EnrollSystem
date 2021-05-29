using AutoMapper;
using EnrollSystem.Constants;
using EnrollSystem.Entities;
using EnrollSystem.Helpers;
using EnrollSystem.Interfaces;
using EnrollSystem.Models.Room;
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
    public class RoomsController : ControllerBase
    {
        private IRoomRepository _roomService;
        private IMapper _mapper;
        public RoomsController(IRoomRepository roomService, IMapper mapper)
        {
            _mapper = mapper;
            _roomService = roomService;
        }
        /// <summary>
        /// Get all room
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAll()
        {
            var rooms = _roomService.GetAll();
            var model = _mapper.Map<IList<RoomModel>>(rooms);
            return Ok(model);
        }
        /// <summary>
        /// Get room by roomId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var room = _roomService.GetById(id);
            if (room == null) return NotFound();
            var model = _mapper.Map<RoomModel>(room);
            return Ok(model);
        }

        /// <summary>
        /// Create new Room
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = ROLE.Admin)]
        [HttpPost]
        public IActionResult Create([FromBody] RoomInputModel model)
        {
            var room = _mapper.Map<Room>(model);
            try
            {
                _roomService.Create(room);
                return Ok(new { message = "Room Created" });
            }
            catch (AppException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        /// <summary>
        /// Update room 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = ROLE.Admin)]
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] RoomInputModel model)
        {
            var room = _mapper.Map<Room>(model);
            try
            {
                _roomService.Update(id, room);
                return Ok(new { message = "Updated" });
            }
            catch (AppException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        /// <summary>
        /// Delete room
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = ROLE.Admin)]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _roomService.Delete(id);
            return Ok(new { message = "Deleted" });
        }
        /// <summary>
        /// Get rooms is not in use
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet("empty-room")]
        public IActionResult GetRoomsNotInUse([FromQuery] ScheduleModel model)
        {
            var rooms = _roomService.GetRoomsNotInUse(model);
            var _model = _mapper.Map<IList<RoomModel>>(rooms);
            return Ok(_model);
        }

        /// <summary>
        /// Get room include Sections
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/sections")]
        public IActionResult GetRoomWithSections(int id)
        {
            var room = _roomService.GetById(id);
            if (room == null)
                return NotFound();
            var roomModel = _mapper.Map<RoomSectionsModel>(room);
            var sectionsModel = _mapper.Map<IList<SectionModel>>(room.Sections.Where(x => x.EndDay.Date >= DateTime.Now.Date).ToList());
            roomModel.SectionList = sectionsModel;
            return Ok(roomModel);
        }
    }
}
