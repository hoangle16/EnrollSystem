using EnrollSystem.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnrollSystem.Data.EntityConfigurations
{
    public class StudentClassTypeEntityConfiguration : IEntityTypeConfiguration<StudentClass>
    {
        public void Configure(EntityTypeBuilder<StudentClass> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();
            //create relationship
            builder.HasOne<Student>(e => e.Student)
                .WithMany(e => e.StudentClasses)
                .HasForeignKey(e => e.StudentId);
            builder.HasOne<Class>(e => e.Class)
                .WithMany(e => e.StudentClasses)
                .HasForeignKey(e => e.ClassId);
        }
    }
}
