using System.ComponentModel.DataAnnotations;

namespace BookStore.Model.DTO
{
    public class UpdateBookDTO
    {
        [Required]
        public string Id { get; set; }
        public string Title { get; set; }

        public double ISBN { get; set; }

        public DateTime PublicationYear { get; set; }

        public int AuthorId { get; set; }
        public int PublisherId { get; set; }

        //
        public AuthorDTO Author { get; set; }

        public PublisherDTO Publisher { get; set; }    


    }
}
