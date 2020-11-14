using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Api.Data
{
    [Table("Authors")]
    public partial class Author
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Bio { get; set; }
        public IList<Book> Books { get; set; }
    }
}