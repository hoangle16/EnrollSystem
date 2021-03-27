using EnrollSystem.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnrollSystem.Data.EntityConfigurations
{
    public class SectionTypeEntityConfiguration : IEntityTypeConfiguration<Section>
    {
        public void Configure(EntityTypeBuilder<Section> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();
            builder.Property(e => e.StartDay)
                .IsRequired();
            builder.Property(e => e.EndDay)
                .IsRequired();
            builder.Property(e => e.StartTime)
                .IsRequired();
            builder.Property(e => e.EndTime)
                .IsRequired();
            builder.Property(e => e.Schedule)
                .IsRequired();
            builder.Property(e => e.MaxSlot)
                .HasDefaultValue(30);
            //create relationship
            builder.HasOne<Room>(e => e.Room)
                .WithMany(e => e.Sections)
                .HasForeignKey(e => e.RoomId);
            builder.HasOne<Teacher>(e => e.Teacher)
                .WithMany(e => e.Sections)
                .HasForeignKey(e => e.TeacherId);
            builder.HasOne<Course>(e => e.Course)
                .WithMany(e => e.Sections)
                .HasForeignKey(e => e.CourseId);
        }
    }
}
