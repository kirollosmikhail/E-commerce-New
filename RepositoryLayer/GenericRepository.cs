using CoreLayer.Entites;
using CoreLayer.Repositories;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _dbContext;

        public GenericRepository(StoreContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            if (typeof(T) == typeof(Product))
            {
                return (IEnumerable<T>)await _dbContext.Products
                     .Include(t => t.ProductType)
                     .Include(b => b.ProductBrand).ToListAsync();
            }
            else
                return await _dbContext.Set<T>().ToListAsync();
        }
        public async Task<T> GetByIdAsync(int id)
        {

            if (typeof(T) == typeof(Product))
            {
                return (T)(object)await _dbContext.Products
                          .Include(t => t.ProductType)
                          .Include(b => b.ProductBrand)
                          .FirstOrDefaultAsync(p => p.Id == id);
            }
            else
            {
                return await _dbContext.Set<T>().FindAsync(id);
            }

        }

    }
}
