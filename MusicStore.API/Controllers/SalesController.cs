﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicStore.Dto.Request;
using MusicStore.Dto.Response;
using MusicStore.Entities;
using MusicStore.Entities.Complex;
using MusicStore.Services.Interfaces;
using System.Security.Claims;

namespace MusicStore.API.Controllers
{
    [ApiController]
    [Route($"{Constants.DefaultRoute}/[action]")]
    [Authorize] // para que en todos los metodos nos obligue a estar autenticado
    public class SalesController : ControllerBase
    {
        private readonly ISaleService _service;

        public SalesController(ISaleService service)
        {
            _service = service;
        }


        [HttpGet]
        [Route("{id:int}")]
        //  [AllowAnonymous] para que invalide la configuracion de arriba  [Authorize] y deje obtener datos sin estar autenticado
        public async Task<ActionResult<BaseResponseGeneric<ICollection<SaleInfo>>>> GetById(int id)
        {
            return Ok(await _service.GetSaleById(id));
        }

        [HttpGet]
        public async Task<ActionResult<BaseResponseGeneric<ICollection<SaleInfo>>>> GetByUser()
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid);

            if (userId == null) return Unauthorized();

            return Ok(await _service.GetSaleByUserId(userId.Value));
        }

        [HttpGet]
        public async Task<IActionResult> GetCollection(int genreId, string? dateInit = null, string? dateEnd = null)
        {
            return Ok(await _service.GetSaleCollection(genreId, dateInit, dateEnd));
        }

        [HttpPost]
        public async Task<IActionResult> Post(DtoSale request)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid);

            if (userId == null) return Unauthorized();

            return Ok(await _service.CreateAsync(request, userId.Value));
            
        }

    }
}
