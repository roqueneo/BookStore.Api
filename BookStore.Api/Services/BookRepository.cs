using BookStore.Api.Contracts;
using BookStore.Api.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Api.Services
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public BookRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Create(Book entity)
        {
            await _dbContext.Books.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(Book entity)
        {
            _dbContext.Books.Remove(entity);
            return await Save();
        }

        public async Task<IList<Book>> FindAll()
            => await _dbContext.Books.ToListAsync();

        public async Task<Book> FindById(int id)
            => await _dbContext.Books.FindAsync(id);

        public async Task<bool> Save()
        {
            var changes = await _dbContext.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> Update(Book entity)
        {
            _dbContext.Books.Update(entity);
            return await Save();
        }
    }
}
