using Microsoft.EntityFrameworkCore;
using RepoPattern.Data;

namespace RepoPattern.Models
{
    public class Repository<T> //: IRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public T GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
            //return _context.SaveChanges();
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
            //return _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = _dbSet.Find(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                //return _context.SaveChanges();
            }
        }
    }

}
