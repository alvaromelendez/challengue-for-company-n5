using Microsoft.EntityFrameworkCore;
using Permissions.Domain.AggregatesModel.PermissionAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permissions.Infrastructure.EntityConfigurations
{
    public class PermissionEntityTypeConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Permission> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.Id)
                .UseHiLo("permissionseq");

            builder.Property(cr => cr.DatePermission).IsRequired(false);
            builder.Property(cr => cr.Comment).IsRequired(false);

            builder
                .Property<int>("_employeeId")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("EmployeeId")
                .IsRequired();

            builder.HasOne(o => o.Employee)
                .WithMany()
                .HasForeignKey("_employeeId");


            builder
                .Property<int>("_permissionTypeId")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("PermissionTypeId")
                .IsRequired();

            builder.HasOne(o => o.PermissionType)
                .WithMany()
                .HasForeignKey("_permissionTypeId");

        }
    }
}
