using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Api.DTOs
{
    public class AuthorDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Bio { get; set; }
        public IList<BookDto> Books { get; set; }
    }

    public class AuthorCreateDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Bio { get; set; }
    }
}
