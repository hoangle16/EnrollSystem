using EnrollSystem.Constants;
using EnrollSystem.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnrollSystem.Data.EntityConfigurations
{
    public class UserTypeEntityConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> entity)
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();
            entity.Property(e => e.UserName)
                .HasMaxLength(20)
                .IsRequired();
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsRequired();
            entity.Property(e => e.IdNumber)
                .HasMaxLength(16)
                .IsRequired();
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(16);
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true);
            entity.Property(e => e.Role)
                .HasDefaultValue(Role.Student);
            //create 1 - 1 relationship
            entity.HasOne(a => a.Admin)
                .WithOne(u => u.User)
                .HasForeignKey<Admin>(ud => ud.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(t => t.Teacher)
                .WithOne(u => u.User)
                .HasForeignKey<Teacher>(ud => ud.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(s => s.Student)
                .WithOne(u => u.User)
                .HasForeignKey<Student>(ud => ud.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
