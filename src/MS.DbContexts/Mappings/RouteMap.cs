using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MS.Entities;
using MS.Entities.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace MS.DbContexts.Mappings
{
    public class RouteMap : IEntityTypeConfiguration<Route>
    {
        public void Configure(EntityTypeBuilder<Route> builder)
        {
            builder.ToTable("TblRoutes");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).ValueGeneratedNever();
            builder.Property(c => c.Name).IsRequired().HasMaxLength(255);
            builder.Property(c => c.StartAddress).IsRequired().HasMaxLength(255);
            builder.Property(c => c.StartTime).IsRequired().HasColumnType("datetime(0)").HasDefaultValue(DateTime.Now);
            builder.Property(c => c.TargetAddress).IsRequired().HasMaxLength(255);
            builder.Property(c => c.EndTime).HasColumnType("datetime(0)"); //.IsRequired(required: false)
            builder.Property(c => c.RunStatus).IsRequired().HasColumnType("tinyint(4)");
            builder.Property(c => c.TruckId).IsRequired();
            //builder.Property(c => c.DriverIds).IsRequired().HasMaxLength(255);
            builder.Property(c => c.IsRound).IsRequired().HasColumnType("bit(1)");
            builder.Property(c => c.Remark).HasMaxLength(500);
            builder.Property(c => c.Status).IsRequired().HasColumnType("tinyint(4)").HasDefaultValue(StatusCode.Enable);
            builder.Property(c => c.Creator).IsRequired();
            builder.Property(c => c.CreateTime).IsRequired().HasColumnType("datetime(0)").HasDefaultValue(DateTime.Now);
            builder.Property(c => c.Modifier);
            builder.Property(c => c.ModifyTime).HasColumnType("datetime(0)");

            builder.HasOne(c => c.Truck);
            builder.HasQueryFilter(b => b.Status != StatusCode.Deleted);//默认不查询软删除数据
        }
    }
}
