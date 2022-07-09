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
    public class GenreConfiguration : IEntityTypeConfiguration<Genre>
    {
        public void Configure(EntityTypeBuilder<Genre> builder)
        {
            builder.Property(p => p.Description)
                .HasMaxLength(100);

            builder.Property(p => p.Status)
               .HasDefaultValue(true);

            //Esto permite que cuando hagamos un select a la tabla siempre cumpla
            //esta condición, de buscar la columna status solo en true
            builder.HasQueryFilter(p => p.Status);
        }
    }
}
