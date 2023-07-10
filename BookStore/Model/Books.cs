using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Model
{
    public class Books
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; }

        public double ISBN { get; set; }

        public DateTime PublicationYear { get; set; }

        [ForeignKey(nameof(AuthorId))]

        public int AuthorId { get; set; }

        [ForeignKey(nameof(PublisherId))]

        public int PublisherId { get; set; }

        public Author Author { get; set; }  

        public Publisher Publisher { get; set; }

      

      

    }
}
