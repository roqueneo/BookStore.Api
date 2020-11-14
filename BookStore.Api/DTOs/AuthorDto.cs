using System.Collections.Generic;

namespace BookStore.Api.DTOs
{
    public class AuthorDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Bio { get; set; }
        public IList<BookDto> Books { get; set; }
    }
}
