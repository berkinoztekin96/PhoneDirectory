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

    
        public async Task CreateAsync(TEntity entity)
        {
            try
            {
                await dbContext.Set<TEntity>().AddAsync(entity);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public IQueryable<TEntity> GetAllAsync()
        {
            try
            {
                return  dbContext.Set<TEntity>().AsNoTracking();
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

        public async void UpdateAsync(TEntity entity)
        {
            try
            {
                dbContext.Set<TEntity>().Update(entity);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }

}