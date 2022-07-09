﻿using MusicStore.Dto.Response;
using MusicStore.Entities;

namespace MusicStore.DataAccess.Repositories
{
    public interface IGenreRepository
    {
     
        Task<ICollection<Genre>> ListAsync(string? filter);
        Task<int> CreateAsync(Genre entity);
        Task<Genre?> GetByIdAsync(int id);
        Task UpdateAsync(Genre entity);
        Task DeleteAsync(int id);
    }
}