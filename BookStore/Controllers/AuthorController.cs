using AutoMapper;
using BookStore.Data;
using BookStore.Model;
using BookStore.Model.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Controllers
{
    [Route("api/Author")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly ApplicationDbContext _dBContext;

        private readonly IMapper _mapper;
        public AuthorController(ApplicationDbContext dBContext , IMapper mapper)    

        {
            _dBContext = dBContext;
            _mapper= mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var author = await _dBContext.Authors
            .Include(_ => _.Books).ToListAsync();
            return Ok(author);
        }


        // Create A new Author.
        [HttpPost]
        public async Task<IActionResult> CreateAuthor([FromBody] AuthorDTO createAuthor)
        {
            var author = _mapper.Map<Author>(createAuthor);

            _dBContext.Authors.Add(author);
            await _dBContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAuthorById), new { id = author.Id }, author);
        }

        //Get Author by Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuthorById(int id)
        {
            var author = await _dBContext.Authors.Include(_ => _.Books).FirstOrDefaultAsync(b=>b.Id==id);

            if (author == null)
            {
                return NotFound();
            }

            return Ok(author);
        }

        // delete by id

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await _dBContext.Authors.FindAsync(id);

            if (author == null)
            {
                return NotFound();
            }

            _dBContext.Authors.Remove(author);
            await _dBContext.SaveChangesAsync();

            return NoContent();
        }

        // update by id
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthor( [FromBody] UpdateAuthorDTO updateAuthorDto)
        {
            var author = await _dBContext.Authors.FindAsync(updateAuthorDto.Id);

            if (author == null)
            {
                return NotFound();
            }

            _mapper.Map(updateAuthorDto, author);

            await _dBContext.SaveChangesAsync();

            return NoContent();
        }







    }
}
