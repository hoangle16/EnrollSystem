using EnrollSystem.Data;
using EnrollSystem.Entities;
using EnrollSystem.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnrollSystem.Services
{
    public class StudentService : IStudentRepository
    {
        private EnrollContext _context;
        public StudentService(EnrollContext context)
        {
            _context = context;
        }
        public IEnumerable<Student> GetAll()
        {
            return _context.Students;
        }

        public Student GetById(int studentId)
        {
            return _context.Students.Find(studentId);
        }

        public IEnumerable<Section> GetMySections(int studentId)
        {
            return _context.Sections.Where(e => e.StudentSections.Any(e => e.StudentId == studentId));
        }
    }
}
