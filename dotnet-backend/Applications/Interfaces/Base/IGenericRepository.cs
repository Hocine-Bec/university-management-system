using Domain.Interfaces;

namespace Applications.Interfaces.Base
{
    public interface IGenericRepository<TEntity> where TEntity : class, IEntity
    {
        public Task<IReadOnlyCollection<TEntity>> GetListAsync();
        public Task<TEntity?> GetByIdAsync(int id);
        public Task<int> AddAsync(TEntity entity);
        public Task<bool> UpdateAsync(TEntity entity);
        public Task<bool> DeleteAsync(int id);
    }
}
