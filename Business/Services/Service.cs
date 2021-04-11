using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicketMaster.Business.Exceptions;
using TicketMaster.Business.Interfaces;
using TicketMaster.Data.Interfaces;
using TicketMaster.Data.Model;

namespace TicketMaster.Business.Services
{
    public class Service<T> : IService<T> where T : Item, new()
    {
        public IMyDbContextWrapper ContextWrapper { get; }
     

        public Service(IMyDbContextWrapper ctxWrapper)
        {
            ContextWrapper = ctxWrapper;
        
        }

        public async Task<T> Add(T entity)
        {
            T result = (await this.ContextWrapper.Context.AddAsync(entity)).Entity;
            await ContextWrapper.Save();
            return result;
        }


        public async Task<T> GetAsync(long id)
        {
            T result = await this.ContextWrapper.Context.FindAsync<T>(id);
            if (result != null)
            {
                return result;
            }
            throw new ItemNotFoundException(id);
        }

        public async Task<ICollection<T>> GetAllAsync()
        {
            return await this.ContextWrapper.Context.Set<T>().ToListAsync();
        }

        public async Task Remove(long id)
        {
            try
            {
                var foo = new T() { Id = id };
                this.ContextWrapper.Context.Remove(foo);
                await ContextWrapper.Save();
            }
            catch(DbUpdateConcurrencyException)
            {
                throw new ItemNotFoundException(id);
            }
            catch (InvalidOperationException)
            {
                throw new ItemNotFoundException(id);
            }
        }

        public async Task RemoveRange(ICollection<long> ids)
        {
            List<T> list = new List<T>();
            foreach (long id in ids)
            {
                list.Add(new T() { Id = id });
            }

            try
            {
                this.ContextWrapper.Context.RemoveRange(list);
                await ContextWrapper.Save();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
