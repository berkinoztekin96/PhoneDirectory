using Microsoft.EntityFrameworkCore;
using PhoneDirectory.Repository.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PhoneDirectory.Repository.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly PhoneDirectoryDbContext dbContext;

        public Repository(PhoneDirectoryDbContext phoneDirectoryDbContext)
        {
            this.dbContext = phoneDirectoryDbContext;
        }

    
        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            try
            {
                await dbContext.Set<TEntity>().AddAsync(entity);
                await dbContext.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public Task<IQueryable<TEntity>> GetAllAsync()
        {
            try
            {
                return  Task.FromResult(dbContext.Set<TEntity>().AsNoTracking());
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            try
            {
                return await dbContext.Set<TEntity>().FindAsync(id);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            try
            {
                dbContext.Set<TEntity>().Update(entity);
                await dbContext.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }

}