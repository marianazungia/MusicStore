using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Entities.Complex
{
    public class SaleInfo
    {
        public int Id { get; set; }
        public DateTime DateEvent { get; set; }
        public string Genre { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public string Title { get; set; } = null!;
        public string OperationNumber { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public int Quantity { get; set; }
        public DateTime SaleDate { get; set; }
        public decimal TotalSale { get; set; }
       
    }
}
