using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.BLL.DTOs.Book
{
    public record BookFilteringDTO
    {
        public int? Price { get; set; }
        public string? Title { get; set; }
    }
}
