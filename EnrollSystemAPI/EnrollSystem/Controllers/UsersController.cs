using AutoMapper;
using EnrollSystem.Constants;
using EnrollSystem.Entities;
using EnrollSystem.Helpers;
using EnrollSystem.Interfaces;
using EnrollSystem.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EnrollSystem.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserRepository _userService;
        private IImageRepository _imageService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        /// <summary>
        /// UserController
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="imageService"></param>
        /// <param name="mapper"></param>
        /// <param name="appSettings"></param>
        public UsersController(
            IUserRepository userService,
            IImageRepository imageService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _userService = userService;
            _imageService = imageService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }
        /// <summary>
        /// Login
        /// </summary>
        /// <param name="model"></param>
        /// <returns>User's Info and token</returns>
        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            var user = _userService.Login(model.UserName, model.Password);

            if (user == null)
                return BadRequest(new { error = "Username or password is incorrect" });
            if (!user.IsActive)
                return BadRequest(new { error = "the User has been disabled. Please contact the administrator" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            var userModel = _mapper.Map<UserModel>(user);
            switch (user.Role)
            {
                case ROLE.Admin:
                    userModel.AdminId = user.Admin.Id;
                    break;
                case ROLE.Teacher:
                    userModel.TeacherId = user.Teacher.Id;
                    break;
                case ROLE.Student:
                    userModel.StudentId = user.Student.Id;
                    break;
            }
            return Ok(new 
            {
                userInfo = userModel,
                Token = tokenString
            });
        }
        /// <summary>
        /// Create admin account
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("create-admin")]
        public IActionResult CreateAdmin([FromBody] UserRegisterModel model)
        {
            //string filePath = "wwwroot/avatars\\defaultAvatar.png"; //default avatar

            var user = _mapper.Map<User>(model);
            user.Role = ROLE.Admin;
            //create default avatar
            Image newImage = new Image();
            newImage.Path = "wwwroot/avatars\\defaultAvatar.png";
            var savedImage = _imageService.Create(newImage);
            user.AvatarId = savedImage.Id;
            try
            {
                //create user
                _userService.Create(user, model.Password);
                return Ok(new { message = "Registration successfully!" });
            }
            catch(AppException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        /// <summary>
        /// Create new User
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = ROLE.Admin)]
        [HttpPost]
        public IActionResult Create([FromBody] UserRegisterModel model)
        {
            var user = _mapper.Map<User>(model);
            Image newImage = new Image();
            newImage.Path = "wwwroot/avatars\\defaultAvatar.png";
            var savedImage = _imageService.Create(newImage);
            user.AvatarId = savedImage.Id;
            try
            {
                //create user
                _userService.Create(user, model.Password);
                return Ok(new { message = "Registration successfully!" });
            }
            catch (AppException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        /// <summary>
        /// Get all user info
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = ROLE.Admin+","+ROLE.Teacher)]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            var model = _mapper.Map<IList<UserModel>>(users);
            return Ok(model);
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var currentUserId = int.Parse(User.Identity.Name);
            if (id != currentUserId && !User.IsInRole(ROLE.Admin) && !User.IsInRole(ROLE.Teacher))
                return Forbid();
            var user = _userService.GetById(id);

            if (user == null) return NotFound();
            var model = _mapper.Map<UserModel>(user);
            return Ok(model);
        }
        /// <summary>
        /// Update user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromForm] UserUpdateModel model)
        {
            var user = _mapper.Map<User>(model);
            user.Id = id;
            var currentUserId = int.Parse(User.Identity.Name);
            if (id != currentUserId && !User.IsInRole(ROLE.Admin))
                return Forbid();
            //update avatar if provied
            var image = model.Image;
            //saving image on server
            if (image != null)
            {
                if (image.Length > 0)
                {
                    string basePath = @"wwwroot/avatars";
                    var supportedTypes = new[] { ".jpg", ".png", ".bmp", ".gif", ".jpeg", "webp" };
                    var imageExt = Path.GetExtension(image.FileName).ToLower();
                    if (!supportedTypes.Contains(imageExt))
                        throw new AppException("File Extension Is InValid - Only Upload jpg/png/bmp/gif/jpeg/webp File");
                    Directory.CreateDirectory(basePath);
                    string fileName = $"{user.Id}_avatar{imageExt}";
                    string filePath = Path.Combine(basePath, fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        image.CopyTo(fileStream);
                    }
                    Image imageEntity = new Image();
                    imageEntity.Path = filePath;
                    var savedImage = _imageService.Create(imageEntity);

                    user.AvatarId = savedImage.Id;
                }
            }
            try
            {
                //update user 
                _userService.Update(user, model.Password);
                return Ok(new { message = "Updated" });
            }
            catch(AppException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        /// <summary>
        /// Delete user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = ROLE.Admin)]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _userService.Delete(id);
            return Ok(new { message = "Deleted" });
        }
        /// <summary>
        /// Toggle IsActive user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = ROLE.Admin)]
        [HttpPut("block/{id}")]
        public IActionResult Block(int id)
        {
            _userService.BlockUser(id);
            return Ok();
        }
    }
}
