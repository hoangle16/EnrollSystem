using EnrollSystem.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnrollSystem.Data.EntityConfigurations
{
    public class RegistrationTimeTypeEntityConfiguration : IEntityTypeConfiguration<RegistrationTime>
    {
        public void Configure(EntityTypeBuilder<RegistrationTime> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();
        }
    }
}
