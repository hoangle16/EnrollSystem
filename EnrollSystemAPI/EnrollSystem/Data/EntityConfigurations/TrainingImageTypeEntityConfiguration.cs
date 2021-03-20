using EnrollSystem.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnrollSystem.Data.EntityConfigurations
{
    public class TrainingImageTypeEntityConfiguration : IEntityTypeConfiguration<TrainingImage>
    {
        public void Configure(EntityTypeBuilder<TrainingImage> entity)
        {
            entity.HasKey(entity => entity.Id);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();
            //create relationship
            entity.HasOne<Student>(e => e.Student)
                .WithMany(e => e.TrainingImages)
                .HasForeignKey(e => e.StudentId);
        }
    }
}
