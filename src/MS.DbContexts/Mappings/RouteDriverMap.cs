using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MS.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MS.DbContexts.Mappings
{
    public class RouteDriverMap : IEntityTypeConfiguration<RouteDriver>
    {
        public void Configure(EntityTypeBuilder<RouteDriver> builder)
        {
            builder.ToTable("TblRouteDriver")
                .HasKey(t => new { t.RouteId, t.DriverId });

            builder.HasOne(t => t.Route)
                .WithMany(p => p.RouteDrivers)
                .HasForeignKey(t => t.RouteId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(t => t.Driver)
                .WithMany(p => p.RouteDrivers)
                .HasForeignKey(t => t.DriverId)
                .OnDelete(DeleteBehavior.ClientSetNull);


        }
    }
}
