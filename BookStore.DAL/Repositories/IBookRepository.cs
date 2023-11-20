using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DAL
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllAsync();
        Task<Book?> GetByIdAsync(int id);
        Task InsertAsync(Book obj);
        Task UpdateAsync(Book obj);
        Task DeleteAsync(int id);
        IQueryable<Book> Search(int? price, string? title);
    }
}
