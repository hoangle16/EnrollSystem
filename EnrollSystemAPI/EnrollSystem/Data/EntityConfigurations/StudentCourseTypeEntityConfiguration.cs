using EnrollSystem.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnrollSystem.Data.EntityConfigurations
{
    public class StudentCourseTypeEntityConfiguration : IEntityTypeConfiguration<StudentCourse>
    {
        public void Configure(EntityTypeBuilder<StudentCourse> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();
            //create relationship
            builder.HasOne<Student>(e => e.Student)
                .WithMany(e => e.StudentCourses)
                .HasForeignKey(e => e.StudentId);
            builder.HasOne<Course>(e => e.Course)
                .WithMany(e => e.StudentCourses)
                .HasForeignKey(e => e.CourseId);
        }
    }
}
