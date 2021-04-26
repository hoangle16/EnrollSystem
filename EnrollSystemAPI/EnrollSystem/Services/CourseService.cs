using EnrollSystem.Data;
using EnrollSystem.Entities;
using EnrollSystem.Helpers;
using EnrollSystem.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnrollSystem.Services
{
    public class CourseService : ICourseRepository
    {
        private EnrollContext _context;
        public CourseService(EnrollContext context)
        {
            _context = context;
        }
        public Course Create(Course course)
        {
            if (_context.Courses.Any(x => x.Name.ToLower() == course.Name.ToLower()))
                throw new AppException("Course name \"" + course.Name + "\" is already exist");

            _context.Courses.Add(course);
            _context.SaveChanges();
            return course;
        }

        public void Delete(int courseId)
        {
            var course = _context.Courses.Find(courseId);
            if (course != null)
            {
                _context.Courses.Remove(course);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Course> GetAll()
        {
            return _context.Courses;
        }

        public Course GetById(int courseId)
        {
            return _context.Courses.Find(courseId);
        }

        public void Update(int courseId, Course courseParams)
        {
            if (_context.Courses.Any(x => x.Name == courseParams.Name))
                throw new AppException("Course name \"" + courseParams.Name + "\" is already exist");
            var course = _context.Courses.Find(courseId);
            if (course == null)
            {
                throw new AppException("Not Found");
            }
            if (!string.IsNullOrWhiteSpace(courseParams.Name))
                course.Name = courseParams.Name;
            _context.Courses.Update(course);
            _context.SaveChanges();
        }
    }
}
