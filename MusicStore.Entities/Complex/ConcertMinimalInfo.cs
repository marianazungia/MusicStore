using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Entities.Complex
{
    public class ConcertMinimalInfo
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public ConcertMinimalInfo()
        {
            Title = string.Empty;
        }
    }
}
