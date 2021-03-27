using EnrollSystem.Data.EntityConfigurations;
using EnrollSystem.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnrollSystem.Data
{
    public class EnrollContext : DbContext
    {
        public EnrollContext(DbContextOptions<EnrollContext> options) : base(options)
        {

        }
        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<Section> Sections { get; set; }
        public virtual DbSet<AttendanceImage> AttendanceImages { get; set; }
        public virtual DbSet<Attendance> Attendances { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<StudentSection> StudentSections { get; set; }
        public virtual DbSet<Teacher> Teachers { get; set; }
        public virtual DbSet<TrainingImage> TrainingImages { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<StudentSectionRegistration> StudentSectionRegistrations { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserTypeEntityConfiguration());
            modelBuilder.ApplyConfiguration(new AdminTypeEntityConfiguration());
            modelBuilder.ApplyConfiguration(new TeacherTypeEntityConfiguration());
            modelBuilder.ApplyConfiguration(new StudentTypeEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ImageTypeEntityConfiguration());
            modelBuilder.ApplyConfiguration(new TrainingImageTypeEntityConfiguration());
            modelBuilder.ApplyConfiguration(new AttendanceImageTypeEntityConfiguration());
            modelBuilder.ApplyConfiguration(new StudentSectionTypeEntityConfiguration());
            modelBuilder.ApplyConfiguration(new AttendanceTypeEntityConfiguration());
            modelBuilder.ApplyConfiguration(new RoomTypeEntityConfiguration());
            modelBuilder.ApplyConfiguration(new SectionTypeEntityConfiguration());
            modelBuilder.ApplyConfiguration(new CourseTypeEntityConfiguration());
            modelBuilder.ApplyConfiguration(new StudentSectionRegistrationTypeEntityConfiguration());
        }
    }
}
