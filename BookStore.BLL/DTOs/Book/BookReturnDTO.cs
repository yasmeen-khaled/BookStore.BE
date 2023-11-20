using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.BLL.DTOs.Book
{
    public record BookReturnDTO
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Summary { get; set; } = string.Empty;
        [Required]
        public int Price { get; set; }

        [Required]
        public string Cover { get; set; } = string.Empty;
    }
}
