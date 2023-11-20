using BookStore.BLL.DTOs;
using BookStore.BLL.DTOs.Book;
using BookStore.BLL.Helpers;
using BookStore.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.BLL
{
    public class BookManager : IBookManager
    {
        private readonly IBookRepository _bookRepository;

        public BookManager(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));
        }

        public async Task<BookReturnDTO?> GetBookByIdAsync(int id)
        {
            Book book = await _bookRepository.GetByIdAsync(id);
            if (book == null) return null;
            //IFormFile imageFile = ImageHelpers.ByteArrayToImage(book.Cover);
            return new BookReturnDTO()
            {
                Id = book.Id,
                Title = book.Title,
                Summary = book.Summary,
                Price = book.Price,
                Cover = ImageHelpers.ByteArrayToString(book.Cover)
            };
        }
        public IQueryable<BookReturnDTO> Search(BookFilteringDTO filterDto)
        {
            return _bookRepository.Search(filterDto.Price, filterDto.Title)
                .Select(b => new BookReturnDTO
                {
                    Id = b.Id,
                    Title = b.Title,
                    Price = b.Price,
                    Summary = b.Summary,
                    Cover = ImageHelpers.ByteArrayToString(b.Cover)
                });
        }
        public async Task<IEnumerable<BookReturnDTO>> GetAllBooksAsync()
        {
            var books = await _bookRepository.GetAllAsync();

            return books.Select(book => new BookReturnDTO
            {
                Id = book.Id,
                Title = book.Title,
                Price = book.Price,
                Summary = book.Summary,
                Cover = ImageHelpers.ByteArrayToString(book.Cover)
            });
        }

        public async Task AddBookAsync(BookBodyDTO bookDTO)
        {
            Book book = new()
            {
                Title = bookDTO.Title,
                Price = bookDTO.Price,
                Summary = bookDTO.Summary,
                Cover = ImageHelpers.ImageToByteArray(image: bookDTO.Cover)
            };
            await _bookRepository.InsertAsync(book);
        }

        public async Task UpdateBookAsync(BookBodyDTO bookDTO)
        {
            await _bookRepository.UpdateAsync(new Book()
            {
                Id= bookDTO.Id, 
                Title= bookDTO.Title,
                Price= bookDTO.Price,
                Summary = bookDTO.Summary,
                Cover = ImageHelpers.ImageToByteArray(bookDTO.Cover)
            });
        }

        public async Task DeleteBookAsync(int id)
        {
            await _bookRepository.DeleteAsync(id);
        }
    }
}
