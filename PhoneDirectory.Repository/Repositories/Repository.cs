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
        public IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
        {
            var query =  GetAllAsync().Where(predicate);
            return  includes.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }

        public IQueryable<TEntity> GetAllAsync()
        {
            try
            {
                return dbContext.Set<TEntity>();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }

}