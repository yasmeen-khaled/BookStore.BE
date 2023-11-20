using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DAL
{
    public class BookRepository : IBookRepository
    {
        private readonly SystemContext _context;

        public BookRepository(SystemContext context)
        {
            _context = context;
        }

        public async Task DeleteAsync(int id)
        {
            Book? book = await _context.Books
                .Where(book => book.Id == id).FirstOrDefaultAsync();
            if (book != null)
                _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            return await _context.Set<Book>().ToListAsync();
        }

        public IQueryable<Book> Search(int? price, string? title)
        {
            var query = _context.Books.Where(b =>
                (price == null ? true : b.Price <= price)
                &&
                (title == null ? true : b.Title.ToLower().Contains(title.ToLower()))
                );
            return query;
        }

        public async Task<Book?> GetByIdAsync(int id)
        {
            return await _context.Books
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task InsertAsync(Book obj)
        {
            _context.Books.Add(obj);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Book obj)
        {
            var DB_Book = _context.Books
                .Where(b => b.Id == obj.Id)
                .FirstOrDefault();
            if (DB_Book != null)
            {
                DB_Book.Summary = obj.Summary;
                DB_Book.Price = obj.Price;
                DB_Book.Title = obj.Title;
                DB_Book.Cover = obj.Cover;
                await _context.SaveChangesAsync();
            }
        }
    }
}
