using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Transactions;
using TicketMaster.Data.Interfaces;
using TicketMaster.Data.Model;

namespace TicketMaster.Data.Context
{
    public class MyDbContextWrapper : IMyDbContextWrapper
    {
        public MyDbContext Context { get; }            

        public MyDbContextWrapper(MyDbContext dbContext)
        {
            this.Context = dbContext;
        }
        
        public async Task Save()
        {
            await this.Context.SaveChangesAsync();
        }

        public IDbContextTransaction GetTransaction()
        {
            return this.Context.Database.BeginTransaction();
        }
    }
}
