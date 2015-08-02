using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRealty.DataLayer
{
    public interface IRoutines<T>
    {
        T GetById(int id);
        T GetAnyById(int id);
        void Create(T obj);
        void Update(T obj);
        void Delete(T obj);
        void Delete(int id);
        IQueryable<T> GetSet();
        IQueryable<T> GetAllSet();
    }
}