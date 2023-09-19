using System.Linq.Expressions;

namespace DataAccessLayer.Abstract;

public interface IGenericDAL<T>
{
    void AddRangeDAL(List<T> p);

    void DeleteDAL(T p);

    T GetByIdDAL(int id);

    T GetDAL(Expression<Func<T, bool>> filter);

    void InsertDAL(T p);

    List<T> ListDAL();

    List<T> ListDAL(Expression<Func<T, bool>> filter);

    void UpdateDAL(T p);
}