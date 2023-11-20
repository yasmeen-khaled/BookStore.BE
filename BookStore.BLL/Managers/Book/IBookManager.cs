using BookStore.BLL.DTOs;
using BookStore.BLL.DTOs.Book;
using BookStore.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.BLL
{
    public interface IBookManager
    {
        Task<BookReturnDTO?> GetBookByIdAsync(int id);
        IQueryable<BookReturnDTO> Search(BookFilteringDTO filterDto);
        Task<IEnumerable<BookReturnDTO>> GetAllBooksAsync();
        Task AddBookAsync(BookBodyDTO bookDTO);
        Task UpdateBookAsync(BookBodyDTO book);
        Task DeleteBookAsync(int id);
    }
}
