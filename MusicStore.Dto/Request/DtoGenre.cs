using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Dto.Request
{
    //public record  DtoGenreRequest
    //{
    //    public string? Description { get; init; }
    //}

    // el record se utiliza cuando queremos que una variable no pueda cambiar de valor
    public record DtoGenre(string? Description);
    
}
