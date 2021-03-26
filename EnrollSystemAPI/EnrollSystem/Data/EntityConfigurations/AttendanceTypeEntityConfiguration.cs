using EnrollSystem.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnrollSystem.Data.EntityConfigurations
{
    public class AttendanceTypeEntityConfiguration : IEntityTypeConfiguration<Attendance>
    {
        public void Configure(EntityTypeBuilder<Attendance> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();
            builder.Property(e => e.HasAttendance)
                .HasDefaultValue(false);
            //create relationship
            builder.HasOne<StudentCourse>(e => e.StudentCourse)
                .WithMany(e => e.Attendances)
                .HasForeignKey(e => e.StudentCourseId);
        }
    }
}
