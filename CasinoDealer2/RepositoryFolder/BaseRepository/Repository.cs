using CasinoDealer2.Data;
using Microsoft.EntityFrameworkCore;

namespace CasinoDealer2.RepositoryFolder.BaseRepository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        // ADD
        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        // UPDATE
        public async Task UpdateAsync(T entity)
        {
             _context.Set<T>().Update(entity);
        }

        // DELETE
        public async Task DeleteAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);
            if (entity is not null)
                _context.Set<T>().Remove(entity);
        }
       
       
    }
}
