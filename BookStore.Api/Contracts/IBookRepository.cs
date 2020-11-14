using BookStore.Api.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Api.Contracts
{
    public interface IBookRepository : IRepositoryBase<Book>
    {
    }
}
