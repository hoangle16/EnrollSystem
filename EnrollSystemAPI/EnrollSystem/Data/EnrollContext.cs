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
        public virtual DbSet<Class> Classes { get; set; }
        public virtual DbSet<EnrollImage> EnrollImages { get; set; }
        public virtual DbSet<Enrollment> Enrollments { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<StudentClass> StudentClasses { get; set; }
        public virtual DbSet<Teacher> Teachers { get; set; }
        public virtual DbSet<TrainingImage> TrainingImages { get; set; }
        public virtual DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserTypeEntityConfiguration());
            modelBuilder.ApplyConfiguration(new AdminTypeEntityConfiguration());
            modelBuilder.ApplyConfiguration(new TeacherTypeEntityConfiguration());
            modelBuilder.ApplyConfiguration(new StudentTypeEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ImageTypeEntityConfiguration());
            modelBuilder.ApplyConfiguration(new TrainingImageTypeEntityConfiguration());
            modelBuilder.ApplyConfiguration(new EnrollImageTypeEntityConfiguration());
            modelBuilder.ApplyConfiguration(new StudentClassTypeEntityConfiguration());
            modelBuilder.ApplyConfiguration(new EnrollmentTypeEntityConfiguration());
            modelBuilder.ApplyConfiguration(new RoomTypeEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ClassTypeEntityConfiguration());
        }
    }
}
