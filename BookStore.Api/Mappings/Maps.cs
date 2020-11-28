using AutoMapper;
using BookStore.Api.Data;
using BookStore.Api.DTOs;

namespace BookStore.Api.Mappings
{
    public class Maps : Profile
    {
        public Maps()
        {
            CreateMap<Author, AuthorDto>().ReverseMap();
            CreateMap<Author, AuthorCreateDto>().ReverseMap();
            CreateMap<Book, BookDto>().ReverseMap();
        }
    }
}
