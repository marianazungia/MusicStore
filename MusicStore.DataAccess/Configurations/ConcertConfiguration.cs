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
    public class ConcertConfiguration : IEntityTypeConfiguration<Concert>
    {
        public void Configure(EntityTypeBuilder<Concert> builder)
        {
            builder
               .Property(e => e.UnitPrice)
               .HasPrecision(10, 2);

            builder
               .Property(e => e.ImageUrl)
               .HasMaxLength(500)
               .IsUnicode(false);

            builder.Property(p => p.Status)
                .HasDefaultValue(true);

            builder.HasQueryFilter(p => p.Status);
        }
    }
}
