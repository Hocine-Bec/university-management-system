using Applications.Interfaces.Base;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Base;

public abstract class GenericRepository<TEntity>: IGenericRepository<TEntity>
    where TEntity : class, IEntity
{
    protected readonly AppDbContext _context;

    protected GenericRepository(AppDbContext context)
    {
        _context = context;
    }

    public virtual async Task<int> AddAsync(TEntity entity)
    {
        await _context.Set<TEntity>().AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity.Id;
    }

    public virtual async Task<TEntity?> GetByIdAsync(int id)
    {
        return await _context.Set<TEntity>()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id.Equals(id));
    }

    public virtual async Task<IReadOnlyCollection<TEntity>> GetListAsync()
    {
        return await _context.Set<TEntity>()
            .AsNoTracking()
            .ToListAsync();
    }

    public virtual async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.Set<TEntity>().FindAsync(id);
        if (entity == null)
            return false;
            
        _context.Set<TEntity>().Remove(entity);
        return await _context.SaveChangesAsync() > 0;
    }

    public virtual async Task<bool> UpdateAsync(TEntity entity)
    {
        // Attach the entity without marking the entire graph as modified
        _context.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;

        // Prevent updates to navigation properties
        foreach (var navigation in _context.Entry(entity).Navigations)
        {
            if (navigation is { IsLoaded: true, CurrentValue: not null })
                _context.Entry(navigation.CurrentValue).State = EntityState.Unchanged;
        }

        return await _context.SaveChangesAsync() > 0;
    }
}