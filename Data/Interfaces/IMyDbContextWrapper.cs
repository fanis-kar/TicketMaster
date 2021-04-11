using TicketMaster.Data.Context;
using Microsoft.EntityFrameworkCore.Storage;
using System.Transactions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TicketMaster.Data.Model;

namespace TicketMaster.Data.Interfaces
{
    public interface IMyDbContextWrapper
    {       
        MyDbContext Context { get; }
        IDbContextTransaction GetTransaction();
        Task Save();                
    }
}
