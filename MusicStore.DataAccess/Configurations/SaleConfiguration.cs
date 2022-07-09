using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicStore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.DataAccess.Configurations
{
    public class SaleConfiguration : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> builder)
        {
            // CON HASHCOLUMN TYPE SE PUEDE PERSONALIZAR EL TIPO DE DATOS EN LA BD
            // builder.Property(p => p.SaleDate)
            //    .HasColumnType("DATE");

            builder.Property(p => p.SaleDate)
                .HasDefaultValueSql("GETDATE()");

            builder.Property(p => p.UnitPrice)
                 .HasPrecision(10,2);

            builder.Property(p => p.TotalSale)
                .HasPrecision(10, 2);

            builder.Property(p => p.UserId)
                .HasMaxLength(36)
                .IsUnicode(false);

            builder.Property(p => p.OperationNumber)
                .HasMaxLength(8);

            builder.HasIndex(p => p.UserId);

            builder.Property(p => p.Status)
               .HasDefaultValue(true);




        }
    }
}
