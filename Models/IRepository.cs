using System;
using System.Collections.Generic;
using System.Linq.Expressions;


namespace net_assignment.Models
{
    public interface IRepository<T, ID>
    {
        ID Create(T entity);
        T Read(ID id);
        int Update(T Entity);
        int Delete(ID id);
        IEnumerable<T> FindAll();
    }
}