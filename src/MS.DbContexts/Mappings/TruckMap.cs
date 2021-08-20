using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MS.Entities;
using MS.Entities.Core;
using System;
using System.Collections.Generic;
using System.Text;
using static MS.Entities.Core.TruckEnums;

namespace MS.DbContexts.Mappings
{
    public class TruckMap : IEntityTypeConfiguration<Truck>
    {
        public void Configure(EntityTypeBuilder<Truck> builder)
        {
            builder.ToTable("TblTrucks");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).ValueGeneratedNever();
            builder.HasIndex(c => c.PlateNumber).IsUnique();
            builder.Property(c => c.PlateNumber).IsRequired().HasMaxLength(10);//.HasColumnName("车牌号");
            builder.Property(c => c.ModelNumber).IsRequired().HasMaxLength(50);//.HasColumnName("车型");
            builder.Property(c => c.BearWeight).IsRequired().HasColumnType("double(10,2)");//.HasColumnName("载重");
            builder.Property(c => c.UseRegion).IsRequired().HasMaxLength(100).HasDefaultValue("");//.HasColumnName("用车范围");
            builder.Property(c => c.Origin).IsRequired().HasColumnType("tinyint(4)").HasDefaultValue(OriginEnum.Buy);//.HasColumnName("性质");
            builder.Property(c => c.Alias).HasMaxLength(50);//.HasColumnName("别名");
            builder.Property(c => c.IsUsed).IsRequired().HasColumnType("bit(1)").HasDefaultValue(0);//.HasColumnName("当前使用状态");
            builder.Property(c => c.GpsCode).IsRequired().HasMaxLength(50).HasDefaultValue("000-000-001");//.HasColumnName("GPS编码");
            builder.Property(c => c.Remark).HasMaxLength(200);//.HasColumnName("备注");
            builder.Property(c => c.Status).IsRequired().HasColumnType("tinyint(4)").HasDefaultValue(StatusCode.Enable);//.HasColumnName("状态");
            builder.Property(c => c.Creator).IsRequired();//.HasColumnName("创建人");
            builder.Property(c => c.CreateTime).IsRequired().HasColumnType("datetime(0)").HasDefaultValue(DateTime.Now);//.HasColumnName("创建时间");
            builder.Property(c => c.Modifier);//.HasColumnName("修改人");
            builder.Property(c => c.ModifyTime).HasColumnType("datetime(0)");//.HasColumnName("修改时间");
        }
    }
}
