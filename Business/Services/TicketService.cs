using AutoMapper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Transactions;
using System.Linq;
using TicketMaster.Business.Exceptions;
using TicketMaster.Business.Interfaces;
using TicketMaster.Data.Interfaces;
using TicketMaster.Data.Model;


namespace TicketMaster.Business.Services
{
    public class TicketService : Service<Ticket>, ITicketService
    {
        private readonly IShowService _showRepository;
        public TicketService(IMyDbContextWrapper ctxWrapper, 
                             IShowService showRepository)
            : base(ctxWrapper)
        {
            _showRepository = showRepository;
        }


        public async Task<ICollection<Ticket>> ReserveTickets(long showId, int noOfTickets, decimal price)
        {
            List<Ticket> tickets = new List<Ticket>();
            using var trans = ContextWrapper.GetTransaction();
            try
            {
                Show e = await _showRepository.GetAsync(showId);
                
                if (e == null)
                {
                    throw new ShowNotFoundException(showId);
                }

                if (e.Tickets.Count >= e.Venue.Capacity)
                {
                    throw new SoldOutException(showId);
                }

                if (e.Tickets.Count + noOfTickets > e.Venue.Capacity)
                {
                    throw new InvalidTicketQtyException(noOfTickets);
                }

                long count = e.Tickets.Count;
                //Debug.WriteLine("############" + count);
                for (int i = 0; i < noOfTickets; i++)
                {
                    Ticket ticket = new Ticket()
                    {
                        Id = Convert.ToInt64(e.Id.ToString()+(++count).ToString()),
                        ShowId = e.Id,                      
                        Price = price
                    };
                    //ticket = Repository.Add(ticket);
                    e.Tickets.Add(ticket);
                    tickets.Add(ticket);                    
                }
                await ContextWrapper.Save();
                trans.Commit();
                return tickets;
            }            
            catch(Exception e)
            {                
                Debug.WriteLine("Generic Rolling");
                throw e;
            }
        }
        public async Task<ICollection<Ticket>> GetByShow(long showId)
        {
            Show show = await _showRepository.GetAsync(showId);

            if (show == null)
            {
                throw new ShowNotFoundException(showId);
            }
            return show.Tickets;
        }        
    }
}
