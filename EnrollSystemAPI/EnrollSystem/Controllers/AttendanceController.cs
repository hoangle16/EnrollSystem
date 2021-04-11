﻿using AutoMapper;
using EnrollSystem.Constants;
using EnrollSystem.Entities;
using EnrollSystem.Helpers;
using EnrollSystem.Interfaces;
using EnrollSystem.Models.Attendance;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EnrollSystem.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private IAttendenceRepository _attendenceService;
        private IImageRepository _imageService;
        private IMapper _mapper;
        public AttendanceController(
            IAttendenceRepository attendenceService,
            IImageRepository imageService,
            IMapper mapper)
        {
            _attendenceService = attendenceService;
            _imageService = imageService;
            _mapper = mapper;
        }

        /// <summary>
        /// Add Images for training
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = ROLE.Student)]
        [HttpPost("training")]
        public IActionResult TrainingFace([FromForm] ImagesUploadModel model)
        {
            Random rand = new Random();
            List<Image> imageList = new List<Image>();
            var currentUserId = int.Parse(User.Identity.Name);
            var files = model.Images;
            //
            string basePath = @"wwwroot/trainingImage";
            var supportedTypes = new[] { ".jpg", ".png", ".bmp", ".gif", ".jpeg", "webp" };
            Directory.CreateDirectory(basePath);
            foreach (var image in files)
            {
                if (image.Length > 0)
                {
                    var imageExt = Path.GetExtension(image.FileName).ToLower();
                    if (!supportedTypes.Contains(imageExt))
                        throw new AppException("File Extension Is InValid - Only Upload jpg/png/bmp/gif/jpeg/webp File");
                    string fileName = $"Face_{currentUserId}_{rand.Next().ToString()}{imageExt}";
                    string filePath = Path.Combine(basePath, fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        image.CopyTo(fileStream);
                    }

                    Image imageEntity = new Image();
                    imageEntity.Path = filePath;
                    _imageService.Create(imageEntity);
                    imageList.Add(imageEntity);
                }
            }
            _attendenceService.TraningFace(currentUserId, imageList);
            return Ok(new { message = "Trained"});
        }
        /// <summary>
        /// Attendance by images
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = ROLE.Teacher)]
        [HttpPost]
        public IActionResult Attendance([FromForm] AttendanceImagesUpload model)
        {
            if (model.DateTime == null)
                model.DateTime = DateTime.Now;
            Random rand = new Random();
            List<Image> imageList = new List<Image>();
            var files = model.Images;
            // 
            string basePath = @"wwwroot/attendanceImage/" + model.SectionId + "/" + model.DateTime.ToString("dd-MM-yyyy") + "/";
            var supportedTypes = new[] { ".jpg", ".png", ".bmp", ".gif", ".jpeg", "webp" };
            Directory.CreateDirectory(basePath);
            foreach(var image in files)
            {
                var imageExt = Path.GetExtension(image.FileName).ToLower();
                if (!supportedTypes.Contains(imageExt))
                    throw new AppException("File Extension Is InValid - Only Upload jpg/png/bmp/gif/jpeg/webp File");
                string fileName = $"Section_{model.SectionId}_{rand.Next().ToString()}{imageExt}";
                string filePath = Path.Combine(basePath, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(fileStream);
                }
                Image imageEntity = new Image();
                imageEntity.Path = filePath;
                _imageService.Create(imageEntity);
                imageList.Add(imageEntity);
            }
            var result = _attendenceService.AttendanceFace(model.SectionId, imageList, model.DateTime);
            var respData = _mapper.Map<IList<AttendanceModel>>(result);
            return Ok(respData);
        }
        /// <summary>
        /// Get attendances list by sectionId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("section/{id}")] //sectionId
        public IActionResult GetAttendancesBySectionId(int id)
        {
            var attendances = _attendenceService.GetAttendanceListBySectionId(id);
            var respData = _mapper.Map<IList<AttendanceModel>>(attendances);
            return Ok(respData);
        }
        /// <summary>
        /// Get attendance list by sectionId and Date
        /// </summary>
        /// <param name="id">sectionId</param>
        /// <param name="date">Format date: yyyy-MM-dd</param>
        /// <returns></returns>
        [HttpGet("section/{id}/{date}")]
        public IActionResult GetAttendencesBySectionIdAndDate(int id, string date)
        {
            DateTime _date = DateTime.ParseExact(date, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            var attendaces = _attendenceService.GetAttendanceListBy(id, _date);
            var respDate = _mapper.Map<IList<AttendanceModel>>(attendaces);
            return Ok(respDate);
        }
        /// <summary>
        /// Update attendance
        /// </summary>
        /// <param name="id">sectionId</param>
        /// <param name="date">Format date: yyyy-MM-dd</param>
        /// <param name="studentIdList">list of studentId has attendance</param>
        /// <returns></returns>
        [Authorize(Roles = ROLE.Teacher)]
        [HttpPut("section/{id}/{date}")]
        public IActionResult UpdateAttendanceList(int id, string date,[FromBody] List<int> studentIdList)
        {
            DateTime _date = DateTime.ParseExact(date, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            var attendances = _attendenceService.EditAttendance(id, _date, studentIdList);
            var respDate = _mapper.Map<IList<AttendanceModel>>(attendances);
            return Ok(respDate);
        }
    }
}
