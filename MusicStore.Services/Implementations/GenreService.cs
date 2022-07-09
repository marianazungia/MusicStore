﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using MusicStore.DataAccess.Repositories;
using MusicStore.Dto.Request;
using MusicStore.Dto.Response;
using MusicStore.Entities;
using MusicStore.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Services.Implementations
{
    public class GenreService : IGenreService
    {
        private readonly IGenreRepository _repository;
        private readonly IMapper _mapper;
        public GenreService(IGenreRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        
        public async Task<BaseResponseGeneric<ICollection<DtoResponseGenre>>> ListAsync(string? filter)
        {
            var response = new BaseResponseGeneric<ICollection<DtoResponseGenre>>();
            try
            {
                var collection = await _repository.ListAsync(filter);

                response.Result = _mapper.Map<ICollection<DtoResponseGenre>>(collection);
               
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ListErrors.Add(ex.Message);
            }
            
            return response;
        }

        public async Task<BaseResponseGeneric<DtoResponseGenre?>> GetByIdAsync(int id)
        {
            var response = new BaseResponseGeneric<DtoResponseGenre>();
            var entity = await _repository.GetByIdAsync(id);

            response.Result = _mapper.Map<DtoResponseGenre>(entity);
            response.Success = true;

            return response;
        }

        public async Task<BaseResponseGeneric<int>> CreateAsync(DtoGenre request)
        {
            var response = new BaseResponseGeneric<int>();
            try
            {
                response.Result = await _repository.CreateAsync(_mapper.Map<Genre>(request));
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ListErrors.Add(ex.Message);
            }
            
            return response;
        }

        public async Task<BaseResponse> UpdateAsync(int id, DtoGenre request)
        {
            var response = new BaseResponse();
            try
            {
                var entity = _mapper.Map<Genre>(request);
                entity.Id = id;
                await _repository.UpdateAsync(entity);
                response.Success = true;

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ListErrors.Add(ex.Message);
            }

            return response;
        }

        public async Task<BaseResponse> DeleteAsync(int id)
        {
            var response = new BaseResponse();
            try
            {
                await _repository.DeleteAsync(id);
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ListErrors.Add(ex.Message);
            }

            return response;
            
        }
    }
}
