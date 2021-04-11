using AutoMapper;
using Microsoft.EntityFrameworkCore.Storage;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading.Tasks;
using TicketMaster.Business.Exceptions;
using TicketMaster.Business.Interfaces;
using TicketMaster.Business.Services;
using TicketMaster.Data.Interfaces;
using TicketMaster.Data.Model;

namespace TicketMaster.UnitTests.Business
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    class TicketServiceTests
    {        
        private IMyDbContextWrapper _ctxWrapper;       
        private IShowService _showRepository;
        private TicketService _service;
        List<Ticket> _tickets;


        [SetUp]
        protected void Setup()
        {
            _ctxWrapper = Substitute.For<IMyDbContextWrapper>();            
            _showRepository = Substitute.For<IShowService>();          
            _service = new TicketService(_ctxWrapper, _showRepository);
                                
        }

        [OneTimeSetUp]
        protected void OneTimeSetup()
        {
            _tickets = new List<Ticket>
            {
                new Ticket() { Id = 1, ShowId = 1, Price = 50 },
                new Ticket() { Id = 2, ShowId = 1, Price = 50 },
                new Ticket() { Id = 3, ShowId = 1, Price = 50 }
            };
        }

        [Test]
        public async Task GetByShowExistsTest()
        {                               
            _showRepository.GetAsync(1).Returns<Show>(new Show() { Id = 1 });
            await _service.GetByShow(1);
            await _showRepository.Received(1).GetAsync(1);
        }

        [Test]
        public void ShowDoesNotExistTest()
        {
            Assert.ThrowsAsync<ShowNotFoundException>(async () => await _service.GetByShow(1));                                    
        }

        [Test]
        public async Task ReserveTicketsOkTest()
        {
            _showRepository.GetAsync(1).Returns<Show>(new Show() 
                { 
                    Id = 1, 
                    Tickets = _tickets, 
                    Venue =  new Venue() { Capacity = 300}
                });

            await _service.ReserveTickets(1, 3, 50);
            await _showRepository.Received(1).GetAsync(1);
        }

        [Test]
        public void ReserveTicketsCapacityExceededTest()
        {
            _showRepository.GetAsync(1).Returns<Show>(new Show()
            {
                Id = 1,
                Tickets = _tickets,
                Venue = new Venue() { Capacity = 3 }
            });

            Assert.ThrowsAsync<SoldOutException>(async () =>  await _service.ReserveTickets(1, 4, 50));
            _showRepository.Received(1).GetAsync(1);            
        }

    }
}
