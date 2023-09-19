using System.Linq.Expressions;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories;

public class GenericRepositories<T> : IGenericDAL<T> where T : class
{
    private readonly DbSet<T> _object;
    private readonly Context context = new();

    public GenericRepositories()
    {
        _object = context.Set<T>();
    }

    public void AddRangeDAL(List<T> p)
    {
        var context = new Context();
        context.AddRange(p);
        context.SaveChanges();
    }

    public void DeleteDAL(T p)
    {
        var deletedEntity = context.Entry(p);
        deletedEntity.State = EntityState.Deleted;
        context.SaveChanges();
    }

    public T GetByIdDAL(int id)
    {
        using var context = new Context();
        return context.Set<T>().Find(id);
    }

    public T GetDAL(Expression<Func<T, bool>> filter)
    {
        return _object.SingleOrDefault(filter);
    }

    public void InsertDAL(T p)
    {
        var addedEntity = context.Entry(p);
        addedEntity.State = EntityState.Added;
        context.SaveChanges();
    }

    public List<T> ListDAL()
    {
        return _object.ToList();
    }

    public List<T> ListDAL(Expression<Func<T, bool>> filter)
    {
        using var c = new Context();
        return c.Set<T>().Where(filter).ToList();
    }

    public void UpdateDAL(T p)
    {
        var updatedEntity = context.Entry(p);
        updatedEntity.State = EntityState.Modified;
        context.SaveChanges();
    }
}