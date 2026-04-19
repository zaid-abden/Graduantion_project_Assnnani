using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;


namespace GraduationProject.Infrastructure.Repositories
{
	public class GenericRepository<T> : IGenericRepository<T> where T : class
	{
		private readonly ApplicationDbContext _dbContext;
		private readonly DbSet<T> _dbSet;

		public GenericRepository(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
			_dbSet = _dbContext.Set<T>();
		}

		public async Task AddAsync(T entity)
		{
			await _dbSet.AddAsync(entity);
		}

		public void Delete(T entity)
		{
			_dbSet.Remove(entity);
		}

		public async Task<IEnumerable<T>> GetAllAsync()
		{
			return await _dbSet.ToListAsync();
		}

		public async Task<T> GetByIdAsync(int id)
		{
			return await _dbSet.FindAsync(id);
		}

		public IQueryable<T> Query()
		{
			return _dbSet.AsQueryable();
		}

		public void Update(T entity)
		{
			_dbSet.Update(entity);
		}
	}
}
