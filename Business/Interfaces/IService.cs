using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicketMaster.Data.Interfaces;
using TicketMaster.Data.Model;


namespace TicketMaster.Business.Interfaces
{
    public interface IService<T> where T : Item, new()
    {
        public IMyDbContextWrapper ContextWrapper { get; }       
        Task<T> Add(T entity);
        Task Remove(long id);
        Task RemoveRange(ICollection<long> ids);
        Task<T> GetAsync(long id);
        Task<ICollection<T>> GetAllAsync();
    }
}

