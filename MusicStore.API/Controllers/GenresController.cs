using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicStore.DataAccess;
using MusicStore.Dto;
using MusicStore.Dto.Request;
using MusicStore.Dto.Response;
using MusicStore.Entities;
using MusicStore.Services.Implementations;
using MusicStore.Services.Interfaces;

namespace MusicStore.API.Controllers
{
    [ApiController]
    [Route(Constants.DefaultRoute)]
    public class GenresController : ControllerBase
    {
        //Creamos esto porque tenemos que recuperar datos de la base de datos.
        //private readonly MusicStoreDbContext _context;
        private readonly IGenreService _service;

        public GenresController(IGenreService service)
        {
           // _context = context;
           _service = service;
        }

       

        [HttpGet]
        // el async Task es para indicar que va ser una tarea asincrona
        [ProducesResponseType(typeof(ICollection<Genre>), 200)]//Para indicar al swager que es lo que va devolver
        public async Task<IActionResult> Get(string? filter)
        {

            var response = await _service.ListAsync(filter);
            //_logger.LogInformation("Habian {Count} registros en la coleccion", response.Result.Count);
            //_logger.LogCritical("Objeto Response {@response}", response);
            return Ok(response);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(BaseResponseGeneric<Genre>), 200)]
        [ProducesResponseType(typeof(BaseResponseGeneric<Genre>), 404)]
        public async Task<IActionResult> Get(int id)
        {
            //var response = new BaseResponseGeneric<Genre>();

            //var genre = await _context.Set<Genre>()
            //    .FirstOrDefaultAsync(p => p.Id == id);
            //if (genre == null)
            //    return NotFound(response);

            //response.ResponseResult = genre;
            //response.Success = true;

            var response = await _service.GetByIdAsync(id);
            if (!response.Success)
                return NotFound(response);

            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(BaseResponseGeneric<int>), 201)]
        [ProducesResponseType(typeof(BaseResponseGeneric<int>), 400)]
        public async Task<IActionResult> Post(DtoGenre request) {

            //var response = new BaseResponseGeneric<int>();

            //try
            //{
            //    //Esto es para que en el json solo pida completar el campo Description
            //    var genre = new Genre
            //    {
            //        Description = request.Description ?? string.Empty,
            //    };
            //    ///

            //    await _context.Set<Genre>().AddAsync(genre);//agrega
            //    await _context.SaveChangesAsync(); // confirma la transacción a la base

            //    response.Success = true;
            //    response.ResponseResult = genre.Id;
            //}
            //catch (Exception ex) {
            //    response.Success = false;
            //    response.ListErrors.Add(ex.Message);
            //}
            var response = await _service.CreateAsync(request);
            
            return Created($"api/Genres/{response.Result}",response);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(BaseResponse), 200)]
        [ProducesResponseType(typeof(BaseResponse), 400)]
        public async Task<IActionResult> Put(int id, DtoGenre request)
        {
            //var response = new BaseResponse();

            //try
            //{
            //    var entity = await _context.Set<Genre>()
            //        .AsTracking()
            //        .FirstOrDefaultAsync(p => p.Id ==id);

            //    if (entity != null) {
            //        entity.Description = request.Description ?? String.Empty;
            //        await _context.SaveChangesAsync();
            //        response.Success = true;
            //    }


            //}
            //catch (Exception ex)
            //{
            //    response.Success = false;
            //    response.ListErrors.Add(ex.Message);

            //}
            var response = await _service.UpdateAsync(id, request);
            return Ok(response);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(BaseResponse), 200)]
        [ProducesResponseType(typeof(BaseResponse), 400)]
        public async Task<IActionResult> Delete(int id)
        {
            //var response = new BaseResponse();

            //try
            //{
            //    var entity = await _context.Set<Genre>()
            //        .AsTracking()
            //        .FirstOrDefaultAsync(p => p.Id == id);

            //    if (entity != null)
            //    {
            //        entity.Status = false;
            //        await _context.SaveChangesAsync();
            //        response.Success = true;
            //    }


            //}
            //catch (Exception ex)
            //{
            //    response.Success = false;
            //    response.ListErrors.Add(ex.Message);

          //  }
            var response = await _service.DeleteAsync(id);
            return Ok(response);
        }
    }
}
