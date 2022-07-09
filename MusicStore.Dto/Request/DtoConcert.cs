using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Dto.Request
{
    public class DtoConcert
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }

        [Range(1,999,ErrorMessage = "El valor debe de estar entre 1 y 999")]

        public int TicketsQuantity { get; set; }
        public string DateEvent { get; set; }
        public string TimeEvent { get; set; }
        public decimal UnitPrice { get; set; }

        
        public string ImageBase64 { get; set; }
        public string? FileName { get; set; }
        public string Place { get; set; }
        public int GenreId { get; set; }
    }
  
}
