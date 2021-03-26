using EnrollSystem.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnrollSystem.Data.EntityConfigurations
{
    public class ImageTypeEntityConfiguration : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> entity)
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();
            entity.Property(e => e.Path)
                .IsRequired();
            //create relationship
            entity.HasOne<TrainingImage>(e => e.TrainingImage)
                .WithOne(e => e.Image)
                .HasForeignKey<TrainingImage>(e => e.ImageId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne<AttendanceImage>(e => e.AttendanceImage)
                .WithOne(e => e.Image)
                .HasForeignKey<AttendanceImage>(e => e.ImageId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne<User>(e => e.User)
                .WithOne(e => e.Avatar)
                .HasForeignKey<User>(e => e.AvatarId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
