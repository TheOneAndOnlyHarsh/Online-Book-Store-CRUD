using AutoMapper;
using BookStore.Model;
using BookStore.Model.DTO;

namespace BookStore.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Books, CreateBookDTO>().ReverseMap();
            CreateMap<Books, BooksDTO>().ReverseMap();
            CreateMap<Books, UpdateBookDTO>().ReverseMap();



            CreateMap<Author, AuthorDTO>().ReverseMap();
            CreateMap<Author, UpdateAuthorDTO>().ReverseMap();


            CreateMap<Publisher, PublisherDTO>().ReverseMap();
            CreateMap<Publisher, UpdatePublisherDTO>().ReverseMap();




        }
    }
}
