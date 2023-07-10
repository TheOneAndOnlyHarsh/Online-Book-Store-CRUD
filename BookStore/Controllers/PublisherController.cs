using BookStore.Data;
using BookStore.Model.DTO;
using BookStore.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace BookStore.Controllers
{
    [Route("api/Publisher")]
    [ApiController]
    public class PublisherController : ControllerBase
    {
        private readonly ApplicationDbContext _dBContext;

        private readonly IMapper _mapper;
        public PublisherController(ApplicationDbContext dBContext , IMapper mapper )
        {
            _dBContext = dBContext;
            _mapper = mapper;
        }

        // get all
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var author = await _dBContext.Publisher
            .Include(_ => _.Books).ToListAsync();
            return Ok(author);
        }

        //find by Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPublisherById(int id)
        {
            var publisher = await _dBContext.Publisher.Include(b => b.Books).FirstOrDefaultAsync(b=>b.Id==id);

            if (publisher == null)
            {
                return NotFound();
            }

            return Ok(publisher);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAuthor([FromBody] PublisherDTO createPublisher)
        {
            var publisher = _mapper.Map<Publisher>(createPublisher);

            _dBContext.Publisher.Add(publisher);
            await _dBContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPublisherById), new { id = publisher.Id }, publisher);
        }

        //delete Publisher
        [HttpDelete]

        public async Task<IActionResult> DeletePublisher (int id)

        {
            {
                var publisher = await _dBContext.Publisher.FindAsync(id);

                if (publisher == null)
                {
                    return NotFound();
                }

                _dBContext.Publisher.Remove(publisher);
                await _dBContext.SaveChangesAsync();

                return NoContent();
            }


        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePublisher([FromBody] UpdatePublisherDTO updatePublisherDto)
        {
            var publisher = await _dBContext.Publisher.FindAsync(updatePublisherDto.Id);

            if (publisher == null)
            {
                return NotFound();
            }

            _mapper.Map(updatePublisherDto, publisher);

            await _dBContext.SaveChangesAsync();

            return NoContent();
        }


    }
}
