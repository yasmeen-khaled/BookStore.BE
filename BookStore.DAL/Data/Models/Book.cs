using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DAL
{
    public class Book
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Summary { get; set; } = string.Empty;
        public int Price { get; set; }

        [Column(TypeName = "varbinary(max)"),Required] 
        public byte[]? Cover { get; set; }
    }
}
