﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MusicStore.Entities;
using MusicStore.Entities.Complex;

namespace MusicStore.DataAccess.Repositories
{
    public class ConcertRepository : IConcertRepository
    {
        private readonly MusicStoreDbContext _context;
        private readonly IMapper _mapper;

        public ConcertRepository(MusicStoreDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<(ICollection<ConcertInfo> Collection, int Total)> GetCollectionAsync(string? filter, int page, int rows, bool home = true)
        {
            //almacenamos consulta dentro de una variable expression
            Expression<Func<Concert, bool>> expression = p => p.Title.Contains(filter ?? string.Empty);
            
            //lista todos los registros de concert pero paginado
            var query = _context.Set<Concert>()
               .Include(p => p.Genre)
               .Where(expression)
               .Skip((page - 1) * rows)//esto es para la paginación
               .Take(rows);//esto también es para la paginación

            query = home ? query.OrderBy(p => p.DateEvent) : query.OrderByDescending(p => p.Id);// cuando se utiliza el skip y take si o si se debe ordenar por una columna
           
            var totalCount = await _context.Set<Concert>()
            .Where(expression)
            .CountAsync();

            var collection = await query
                .Select(x => _mapper.Map<ConcertInfo>(x))
                .ToListAsync();

            return (collection, totalCount);
        }

        public async Task<ICollection<ConcertInfo>> GetCollectionByGenre(int id)
        {
            return await _context.Set<Concert>()
                .Where(p => p.GenreId == id)
                .OrderBy(p => p.DateEvent)
                .Select(x => _mapper.Map<ConcertInfo>(x))
                .ToListAsync();
        }

        public async Task<ICollection<ConcertMinimalInfo>> GetMinimalCollectionByGenre(int id)
        {
            return await _context.Set<Concert>()
                .Where(p => p.GenreId == id)
                .OrderBy(p => p.Title)
                .Select(x => _mapper.Map<ConcertMinimalInfo>(x))
                .ToListAsync();
        }

        public async Task<Concert?> GetByIdAsync(int id)
        {
            return await _context.Set<Concert>()
                .AsTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<int> CreateAsync(Concert concert)
        {
            await _context.Set<Concert>().AddAsync(concert);
            await _context.SaveChangesAsync();

            return concert.Id;
        }

        public async Task UpdateAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task Finalize(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
                entity.Finalized = true;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
                entity.Status = false;

            await _context.SaveChangesAsync();
        }
    }
}
