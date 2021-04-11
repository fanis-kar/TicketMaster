using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TicketMaster.Data.Interfaces;
using TicketMaster.Data.Model;

namespace TicketMaster.Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : Item
    {
        protected readonly DbSet<T> _entities;

        public Repository(IMyDbContextWrapper ctxWrapper)
        {
            _entities = ctxWrapper.Context.Set<T>();
        }

        public virtual T GetById(long id)
        {
            return _entities.Find(id);
        }

        public IEnumerable<T> GetAll()
        {
            return _entities.ToList();
        }
        
        public T Add(T entity)
        {
            return _entities.Add(entity).Entity;
        }        

        public void Remove(T entity)
        {
            _entities.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _entities.RemoveRange(entities);
        }
    }
}
