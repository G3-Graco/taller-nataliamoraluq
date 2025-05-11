using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        //requiere migration, cierto?:> sipi pero cm osmar ya lo hizo, ESTA VEZ no haec falta
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.UserName).IsRequired().HasMaxLength(255); 
            builder.Property(x => x.Password).IsRequired().HasMaxLength(255); 
            builder.ToTable("User");
        }
    }
}