using BookStore.BLL;
using BookStore.BLL.DTOs;
using BookStore.BLL.DTOs.Book;
using BookStore.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookStore.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookManager _bookManager;
        public BooksController(IBookManager bookManager)
        {
            _bookManager = bookManager ?? throw new ArgumentNullException(nameof(bookManager));
        }

        // GET: api/<BooksController>
        [HttpGet]
        //[Authorize]
        public async Task<ActionResult<IEnumerable<BookReturnDTO>>> Get()
        {
            var books = await _bookManager.GetAllBooksAsync();
            return Ok(books);
        }
        
        [HttpPost]
        [Route("search")]
        //[Authorize(Policy = "allowAdmins")]
        public ActionResult<IEnumerable<BookReturnDTO>> Search(BookFilteringDTO filter)
        {
            var books = _bookManager.Search(filter);
            return Ok(books);
        }

        // GET api/<BooksController>/5
        [HttpGet("{id}")]
        //[Authorize(Policy = "allowAdmins")]
        public async Task<ActionResult<BookReturnDTO>> Get(int id)
        {
            var book = await _bookManager.GetBookByIdAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        // POST api/<BooksController>
        [HttpPost]
        //[Authorize]
        [Authorize(Policy = "allowAdmins")]
        public async Task<ActionResult<Book>> Post([FromForm]BookBodyDTO bookDTO)
        {
            if (bookDTO == null)
            {
                return BadRequest("Book data is invalid");
            }

            await _bookManager.AddBookAsync(bookDTO);

            //return CreatedAtAction(nameof(Get), new { id = book.Id }, book);
            return Ok();
        }

        // PUT api/<BooksController>/5
        [HttpPut("{id}")]
        [Authorize(Policy = "allowAdmins")]
        public async Task<IActionResult> Put([FromForm]BookBodyDTO book)
        {
            try
            {
                if (book == null)
                {
                    return BadRequest("Invalid book data or ID mismatch");
                }

                var existingBook = await _bookManager.GetBookByIdAsync(book.Id);

                if (existingBook == null)
                {
                    return NotFound();
                }

                await _bookManager.UpdateBookAsync(book);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        // DELETE api/<BooksController>/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "allowAdmins")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _bookManager.GetBookByIdAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            await _bookManager.DeleteBookAsync(id);

            return NoContent();
        }
    }
}
