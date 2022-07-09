using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MusicStore.Dto.Response;
using MusicStore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.DataAccess.Repositories
{
 

    public class GenreRepository : IGenreRepository
    {
        private readonly MusicStoreDbContext _context;
        private readonly IMapper _mapper;

        public GenreRepository(MusicStoreDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

       
        public  async Task<ICollection<Genre>> ListAsync(string? filter)
        {
            var list = await _context.Set<Genre>()
             //.IgnoreQueryFilters()
             // .AsNoTracking()// indica a EF para que que haga un trackeao y no utilizar el cache
             .Where(p => p.Description.StartsWith(filter ?? String.Empty))
             .ToListAsync();

            return list;
        }

        public async Task<Genre?> GetByIdAsync(int id)
        {

            var genre = await _context.Set<Genre>()
               .FirstOrDefaultAsync(p => p.Id == id);

            return genre;
        }

        public async Task<int> CreateAsync(Genre entity)
        {
            await _context.Set<Genre>().AddAsync(entity);//agrega
            await _context.SaveChangesAsync(); // confirma la transacción a la base

            return entity.Id;
        }

        public async Task UpdateAsync(Genre entity)
        {
            var genre = await _context.Set<Genre>()
                   .AsTracking()
                   .FirstOrDefaultAsync(p => p.Id == entity.Id);

            if (genre != null)
            {

                genre.Description = entity.Description ?? String.Empty;
                await _context.SaveChangesAsync();

            }
        }

        public async Task DeleteAsync(int id)
        {
            var genre = await _context.Set<Genre>()
                   .AsTracking()
                   .FirstOrDefaultAsync(p => p.Id == id);

            if (genre == null) return;

            genre.Status = false;
            await _context.SaveChangesAsync();
        }
    }
}
