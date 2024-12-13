﻿using Microsoft.EntityFrameworkCore;
using VoteHub.Persistance.Repositories.Interfaces;
using VotingAppApi.Data;

namespace VoteHub.Persistance.Repositories.Implementation
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly VotingAppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(VotingAppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}