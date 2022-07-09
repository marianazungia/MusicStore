using Microsoft.Extensions.DependencyInjection;
using MusicStore.DataAccess.Repositories;
using MusicStore.Services.Implementations;
using MusicStore.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Services
{
    public static class DependencyInjection
    {
        //Se agrega las dependencias de nuestras clases
        public static IServiceCollection AddDependencies(this IServiceCollection services) {
        //    services.AddTransient<IGenreRepository, GenreRepository>()
        //        .AddTransient<IGenreService, GenreService>()
        //        .AddTransient<IConcertRepository, ConcertRepository>()
        //        .AddTransient<IConcertService, ConcertService>();

       //services.AddTransient<IFileUploader, FileUploader>();
       
        services.AddTransient<IGenreRepository, GenreRepository>()
            .AddTransient<IGenreService, GenreService>();

        services.AddTransient<IConcertRepository, ConcertRepository>()
            .AddTransient<IConcertService, ConcertService>();

        services.AddTransient<ISaleRepository, SaleRepository>()
            .AddTransient<ISaleService, SaleService>();

         services.AddTransient<IUserService, UserService>();
            return services;
        }
    }
}
