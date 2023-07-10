namespace BookStore.Model.DTO
{
    public class BooksDTO
    {
        public string Id { get ; set; }     
        public string Title { get; set; }

        public double ISBN { get; set; }

        public DateTime PublicationYear { get; set; }

        public int AuthorId { get; set; }
        public int PublisherId { get; set; }
    }
}
