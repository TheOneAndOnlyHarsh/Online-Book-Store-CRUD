using AutoMapper;
using Azure;
using BookStore.Data;
using BookStore.Model;
using BookStore.Model.DTO;
using Microsoft.AspNetCore.Authorization;
//using BookStore.Repository.IReposiotory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BookStore.Controllers
{
    [Route("api/Books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly ApplicationDbContext _dBContext;

        private readonly IMapper _mapper;
        public BooksController(ApplicationDbContext dBContext,IMapper mapper)
        {
            _dBContext = dBContext;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize] // any authrize user will be able to access that:
        public async Task<IActionResult> Get()
        {
            var books = await _dBContext.Books
            .Include(b => b.Author)
            .Include(b => b.Publisher)
            .Select(b => new

            {
                 Id = b.Id,
                 Title = b.Title,
                 ISBN = b.ISBN,
                 PublicationYear = b.PublicationYear,
                 AuthorName = b.Author != null ? b.Author.AuthorName : null,
                 PublisherName = b.Publisher != null ? b.Publisher.PublisherName : null

            }).ToListAsync();


            return Ok(books);


        }

        //Creating a post request:

        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] CreateBookDTO createBook)
        {
            // Verify if the author ID exists
            var author = await _dBContext.Authors.FindAsync(createBook.AuthorId);
            if (author == null)
            {
                return BadRequest("Invalid author ID. Author not found.");
            }
            // Verify if the publisher ID exists
            var publisher = await _dBContext.Publisher.FindAsync(createBook.PublisherId);
            if (publisher == null)
            {
                return BadRequest("Invalid publisher ID. Publisher not found.");
            }
            var book = _mapper.Map<Books>(createBook);
            _dBContext.Books.Add(book);
            await _dBContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBookById), new { id = book.Id } , book);

        }

        //update by book details by Id:
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] UpdateBookDTO updateBookDto)
        {
            var book = await _dBContext.Books
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            // Verify if the author ID exists
            var author = await _dBContext.Authors.FindAsync(updateBookDto.AuthorId);
            if (author == null)
            {
                return BadRequest("Invalid author ID. Author not found.");
            }

            // Verify if the publisher ID exists
            var publisher = await _dBContext.Publisher.FindAsync(updateBookDto.PublisherId);
            if (publisher == null)
            {
                return BadRequest("Invalid publisher ID. Publisher not found.");
            }

            // Update the book properties
            _mapper.Map(updateBookDto, book);

            // Update additional properties for author and publisher
            book.Author.AuthorName = updateBookDto.Author.AuthorName;
            book.Author.Email = updateBookDto.Author.Email;
            book.Publisher.PublisherName = updateBookDto.Publisher.PublisherName;
            book.Publisher.Email = updateBookDto.Publisher.Email;

            await _dBContext.SaveChangesAsync();

            return NoContent();
        }




        //get book by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(int id)
        {
            var book = await _dBContext.Books
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }


        // deleting a book :
        [HttpDelete("{id}")]
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _dBContext.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            _dBContext.Books.Remove(book);
            await _dBContext.SaveChangesAsync();

            return NoContent();
        }


    }
}
