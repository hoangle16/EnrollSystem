using EnrollSystem.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnrollSystem.Data.EntityConfigurations
{
    public class StudentSectionTypeEntityConfiguration : IEntityTypeConfiguration<StudentSection>
    {
        public void Configure(EntityTypeBuilder<StudentSection> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();
            //create relationship
            builder.HasOne<Student>(e => e.Student)
                .WithMany(e => e.StudentSection)
                .HasForeignKey(e => e.StudentId);
            builder.HasOne<Section>(e => e.Section)
                .WithMany(e => e.StudentSections)
                .HasForeignKey(e => e.SectionId);
        }
    }
}
