using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MS.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MS.DbContexts.Mappings
{
    public class TravelDriverMap : IEntityTypeConfiguration<TravelDriver>
    {
        public void Configure(EntityTypeBuilder<TravelDriver> builder)
        {
            builder.ToTable("TblTravelDriver")
                .HasKey(t => new { t.TravelId, t.DriverId });

            builder.HasOne(t => t.Travel)
                .WithMany(p => p.TravelDrivers)
                .HasForeignKey(t => t.TravelId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(t => t.Driver)
                .WithMany(p => p.TravelDrivers)
                .HasForeignKey(t => t.DriverId)
                .OnDelete(DeleteBehavior.ClientSetNull);


        }
    }
}
