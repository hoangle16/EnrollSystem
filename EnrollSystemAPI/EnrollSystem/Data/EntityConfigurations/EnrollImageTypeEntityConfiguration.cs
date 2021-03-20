using EnrollSystem.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnrollSystem.Data.EntityConfigurations
{
    public class EnrollImageTypeEntityConfiguration : IEntityTypeConfiguration<EnrollImage>
    {
        public void Configure(EntityTypeBuilder<EnrollImage> entity)
        {
            entity.HasKey(entity => entity.Id);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();
            //Create relationship
            entity.HasOne<Class>(e => e.Class)
                .WithMany(e => e.EnrollImages)
                .HasForeignKey(e => e.ClassId);
        }
    }
}
