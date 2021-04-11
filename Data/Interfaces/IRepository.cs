using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using TicketMaster.Data.Model;

namespace TicketMaster.Data.Interfaces
{
    public interface IRepository<T> where T : Item
    {
        T GetById(long id);
        IEnumerable<T> GetAll();        
        T Add(T entity);       
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}
