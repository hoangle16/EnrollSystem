using EnrollSystem.Data;
using EnrollSystem.Entities;
using EnrollSystem.Interfaces;
using EnrollSystem.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EnrollSystem.Constants;

namespace EnrollSystem.Services
{
    public class UserService : IUserRepository
    {
        private EnrollContext _context;
        public UserService(EnrollContext context)
        {
            _context = context;
        }
        public void BlockUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
                throw new AppException("User not found!");
            user.IsActive = !user.IsActive;

            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public User Create(User user, string password)
        {
            //validation
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Password is required");
            if (_context.Users.Any(x => x.UserName == user.UserName))
                throw new AppException("Username \"" + user.UserName + "\" is already taken");

            //
            byte[] passwordHash, passwordSalt;
            PasswordHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            if (string.IsNullOrWhiteSpace(user.Role) || (user.Role != ROLE.Admin && user.Role != ROLE.Teacher) )
                user.Role = ROLE.Student;
            //add user
            _context.Users.Add(user);
            _context.SaveChanges();
            switch (user.Role)
            {
                case ROLE.Admin:
                    Admin admin = new Admin();
                    admin.UserId = user.Id;
                    _context.Admins.Add(admin);
                    break;
                case ROLE.Teacher:
                    Teacher teacher = new Teacher();
                    teacher.UserId = user.Id;
                    _context.Teachers.Add(teacher);
                    break;
                case ROLE.Student:
                    Student student = new Student();
                    student.UserId = user.Id;
                    _context.Students.Add(student);
                    break;
            }
            //Save
            _context.SaveChanges();

            return user;
        }

        public void Delete(int id)
        {
            var user = _context.Users.Find(id);
            
            if (user != null)
            {
                if (user.Role == ROLE.Admin)
                {
                    var admin = _context.Admins.SingleOrDefault(x => x.UserId == id);
                    if (admin != null)
                        _context.Admins.Remove(admin);
                }
                if (user.Role == ROLE.Teacher)
                {
                    var teacher = _context.Teachers.SingleOrDefault(x => x.UserId == id);
                    if (teacher != null)
                        _context.Teachers.Remove(teacher);
                }
                if (user.Role == ROLE.Student)
                {
                    var student = _context.Students.SingleOrDefault(x => x.UserId == id);
                    if (student != null)
                        _context.Students.Remove(student);
                }

                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users;
        }

        public User GetById(int id)
        {
            return _context.Users.Find(id);
        }

        public User Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;
            var user = _context.Users.SingleOrDefault(x => x.UserName == username);
            //check if username exists
            if (user == null)
                return null;
            //check if password is correct
            if (!PasswordHelper.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;
            return user;
        }

        public void Update(User userParams, string password = null)
        {
            var user = _context.Users.Find(userParams.Id);

            if (user == null)
                throw new AppException("User not found");
            //update password if provied
            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                PasswordHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }
            //update avatar
            if (userParams.AvatarId != 0)
            {
                user.AvatarId = userParams.AvatarId;
            }
            //update user info
            if (!string.IsNullOrWhiteSpace(userParams.Name))
                user.Name = userParams.Name;
            user.Gender = userParams.Gender;
            if (!string.IsNullOrWhiteSpace(userParams.IdNumber))
                user.IdNumber = userParams.IdNumber;
            if (!string.IsNullOrWhiteSpace(userParams.PhoneNumber))
                user.PhoneNumber = userParams.PhoneNumber;
            if (!string.IsNullOrWhiteSpace(userParams.Address))
                user.Address = userParams.Address;
            //update role
            if (userParams.Role != null && user.Role != userParams.Role)
            {
                //find current role talbe
                if (user.Role == ROLE.Admin)
                {
                    var _role = _context.Admins.SingleOrDefault(x => x.UserId == user.Id);
                    _context.Admins.Remove(_role);
                }
                else if (user.Role == ROLE.Teacher)
                {
                    var _role = _context.Teachers.SingleOrDefault(x => x.UserId == user.Id);
                    _context.Teachers.Remove(_role);
                }
                else
                {
                    var _role = _context.Students.SingleOrDefault(x => x.UserId == user.Id);
                    _context.Students.Remove(_role);
                }
                if (userParams.Role != ROLE.Admin && userParams.Role != ROLE.Teacher)
                {
                    user.Role = ROLE.Student;
                }else
                {
                    user.Role = userParams.Role;
                }
                //add role table
                switch (user.Role)
                {
                    case ROLE.Admin:
                        Admin admin = new Admin();
                        admin.UserId = user.Id;
                        _context.Admins.Add(admin);
                        break;
                    case ROLE.Teacher:
                        Teacher teacher = new Teacher();
                        teacher.UserId = user.Id;
                        _context.Teachers.Add(teacher);
                        break;
                    case ROLE.Student:
                        Student student = new Student();
                        student.UserId = user.Id;
                        _context.Students.Add(student);
                        break;
                }
            }
            
            _context.Users.Update(user);
            _context.SaveChanges();
        }
    }
}
