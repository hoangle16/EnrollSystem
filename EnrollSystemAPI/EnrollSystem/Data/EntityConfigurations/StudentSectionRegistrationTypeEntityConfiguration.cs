using EnrollSystem.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnrollSystem.Data.EntityConfigurations
{
    public class StudentSectionRegistrationTypeEntityConfiguration : IEntityTypeConfiguration<StudentSectionRegistration>
    {
        public void Configure(EntityTypeBuilder<StudentSectionRegistration> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();
            //relationship config
            builder.HasOne<Student>(e => e.Student)
                .WithMany(e => e.StudentSectionRegistrations)
                .HasForeignKey(e => e.StudentId);
            builder.HasOne<Section>(e => e.Section)
                .WithMany(e => e.StudentSectionRegistrations)
                .HasForeignKey(e => e.SectionId);
        }
    }
}
