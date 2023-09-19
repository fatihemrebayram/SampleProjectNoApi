using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IGenericService<T>
    {
        void AddBL(T t);

        void AddRangeBL(List<T> p);

        List<T> GetListBL();

        List<T> GetListFilteredBL(string filter);

        void RemoveBL(T t);

        T TGetByID(int id);

        void UpdateBL(T t);
    }
}