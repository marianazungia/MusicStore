﻿using AutoMapper;
using Microsoft.Extensions.Logging;
using MusicStore.DataAccess.Repositories;
using MusicStore.Dto.Request;
using MusicStore.Dto.Response;
using MusicStore.Entities;
using MusicStore.Entities.Complex;
using MusicStore.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Services.Implementations
{
    public class SaleService : ISaleService
    {
        private readonly ISaleRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<SaleService> _logger;

        public SaleService(ISaleRepository repository, IMapper mapper, ILogger<SaleService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponseGeneric<int>> CreateAsync(DtoSale request, string userId)
        {
            var response = new BaseResponseGeneric<int>();

            try
            {
                //    // var concert = await _concertRepository.GetByIdAsync(request.EventId);
                //    //
                //    // if (concert == null)
                //    // {
                //    //     response.Errors.Add($"El Id {request.ConcertId} del Concierto no existe");
                //    //     response.Success = false;
                //    //     return response;
                //    // }

                //    //if (concert.Finalized)
                //    //{
                //    //    response.Errors.Add($"El Concierto {concert.Title} ya finalizó");
                //    //    response.Success = false;
                //    //    return response;
                //    //}

                var sale = _mapper.Map<Sale>(request);
                sale.SaleDate = DateTime.Now;
                sale.TotalSale = request.Quantity * request.UnitPrice;
                sale.UserId = userId;

                response.Result = await _repository.CreateAsync(sale);
                response.Success = true;

            }
            catch (Exception ex)
            {
                response.ListErrors.Add(ex.Message);
                response.Success = false;
                _logger.LogCritical(ex.Message);
            }

            return response;
            
        }

        public async Task<BaseResponseGeneric<DtoSaleInfo>> GetSaleById(int id)
        {
            var response = new BaseResponseGeneric<DtoSaleInfo>();

            try
            {
                response.Result = _mapper.Map<DtoSaleInfo>(await _repository.GetSaleById(id));

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ListErrors.Add(ex.Message);
                response.Success = false;
                _logger.LogCritical(ex.Message);
            }

            return response;

        }

        public async Task<BaseResponseGeneric<ICollection<DtoSaleInfo>>> GetSaleCollection(int genreId, string? dateInit, string? dateEnd)
        {
            var response = new BaseResponseGeneric<ICollection<DtoSaleInfo>>();

            try
            {
                var englishFormat = new CultureInfo("en-US");

                var collection = await _repository.GetSaleCollection(genreId,
                    string.IsNullOrEmpty(dateInit) ? null : Convert.ToDateTime(dateInit, englishFormat),
                    string.IsNullOrEmpty(dateEnd) ? null : Convert.ToDateTime(dateEnd, englishFormat));

                response.Result = collection
                    .Select(p => _mapper.Map<DtoSaleInfo>(p))
                    .ToList();

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ListErrors.Add(ex.Message);
                response.Success = false;
                _logger.LogCritical(ex.Message);
            }

            return response;

        }

        public async Task<BaseResponseGeneric<ICollection<DtoSaleInfo>>> GetSaleByUserId(string userId)
        {
            var response = new BaseResponseGeneric<ICollection<DtoSaleInfo>>();

            try
            {
                var collection = await _repository.GetSaleByUserId(userId);

                response.Result = collection
                    .Select(p => _mapper.Map<DtoSaleInfo>(p))
                    .ToList();

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ListErrors.Add(ex.Message);
                response.Success = false;
                _logger.LogCritical(ex.Message);
            }

            return response;
        }

        public async Task<BaseResponseGeneric<ICollection<ReportSaleInfo>>> GetReportSale(int genreId, string dateInit, string dateEnd)
        {
            var response = new BaseResponseGeneric<ICollection<ReportSaleInfo>>();

            try
            {
                response.Result = await _repository.GetReportSale(genreId, Convert.ToDateTime(dateInit), Convert.ToDateTime(dateEnd));
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ListErrors.Add(ex.Message);
                response.Success = false;
                _logger.LogCritical(ex.Message);
            }

            return response;
        }

       
    }
}
