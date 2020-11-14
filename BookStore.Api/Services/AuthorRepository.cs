﻿using BookStore.Api.Contracts;
using BookStore.Api.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStore.Api.Services
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public AuthorRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Create(Author entity)
        {
            await _dbContext.Authors.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(Author entity)
        {
            _dbContext.Authors.Remove(entity);
            return await Save();
        }

        public async Task<IList<Author>> FindAll()
            => await _dbContext.Authors.ToListAsync();

        public async Task<Author> FindById(int id)
            => await _dbContext.Authors.FindAsync(id);

        public async Task<bool> Save()
        {
            var changes = await _dbContext.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> Update(Author entity)
        {
            _dbContext.Authors.Update(entity);
            return await Save();
        }
    }
}
