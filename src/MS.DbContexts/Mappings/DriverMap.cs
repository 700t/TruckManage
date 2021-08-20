using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MS.Entities;
using MS.Entities.Core;
using System;
using System.Collections.Generic;
using System.Text;
using static MS.Entities.Core.DriverEnums;

namespace MS.DbContexts.Mappings
{
    public class DriverMap : IEntityTypeConfiguration<Driver>
    {
        public void Configure(EntityTypeBuilder<Driver> builder)
        {
            builder.ToTable("TblDrivers");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).ValueGeneratedNever();
            builder.Property(c => c.Name).IsRequired().HasMaxLength(20);//.HasColumnName("姓名");
            builder.Property(c => c.Gender).IsRequired().HasColumnType("tinyint(4)");//.HasDefaultValue(GenderCode.None).HasColumnName("性别");
            builder.Property(c => c.Phone).IsRequired().HasMaxLength(20);//.HasColumnName("联系电话");
            builder.Property(c => c.Photo).HasMaxLength(300);//.HasColumnName("照片");
            builder.Property(c => c.IdNumber).IsRequired().HasMaxLength(20);//.HasColumnName("身份证号");
            builder.Property(c => c.License).IsRequired().HasColumnType("tinyint(4)").HasDefaultValue(DrivingLicenseEnum.None);//.HasColumnName("驾照类型");
            builder.Property(c => c.IssueDate).IsRequired().HasColumnType("date").HasDefaultValue("2001-01-01");//.HasColumnName("驾照颁发日期");
            builder.Property(c => c.LicensePhoto).HasMaxLength(300);//.HasColumnName("驾照图片");
            builder.Property(c => c.Remark).HasMaxLength(200);//.HasColumnName("备注");
            builder.Property(c => c.Status).IsRequired().HasColumnType("tinyint(4)").HasDefaultValue(StatusCode.Enable);//.HasColumnName("状态");
            builder.Property(c => c.Creator).IsRequired();//.HasColumnName("创建人");
            builder.Property(c => c.CreateTime).IsRequired().HasColumnType("datetime(0)").HasDefaultValue(DateTime.Now);//.HasColumnName("创建时间");
            builder.Property(c => c.Modifier);//.HasColumnName("修改人");
            builder.Property(c => c.ModifyTime).HasColumnType("datetime(0)");//.HasColumnName("修改时间");
        }
    }
}
