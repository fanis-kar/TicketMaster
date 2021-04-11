using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TicketMaster.Api.Model;
using TicketMaster.IntegrationTests;
using System.Diagnostics;

namespace DbLockTest
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    class TicketControllerTest
    {
        private string _venuesRoute, _actsRoute, _showsRoute, _ticketsRoute;
        private VenueJSON _venue;
        private long _newVenueId;
        private long _newActId;
        private long _newShow1Id;
        private long _newShow2Id;
        private ICollection<TicketJSONReply> _ticketsLot1;

        [OneTimeSetUp]
        protected async Task OneTimeSetup()
        {
            _venuesRoute = SetUpClass.hostname + "api/venues/";
            _actsRoute = SetUpClass.hostname + "api/acts/";
            _showsRoute = SetUpClass.hostname + "api/shows/";
            _ticketsRoute = SetUpClass.hostname + "api/tickets/";

            _venue = new VenueJSON()
            {
                Name = "Test Venue 1",
                Capacity = 8
            };

            ActJSON _act = new ActJSON()
            {
                Name = "Test Act 1"
            };

            using var client = new HttpClient();

            //Create Venue            
            _newVenueId = await TestUtils.CreateVenue(client, _venuesRoute, _venue).ConfigureAwait(false);

            //Create Act
            _newActId = await TestUtils.CreateAct(client, _actsRoute, _act).ConfigureAwait(false);

            var show = new ShowJSON()
            {
                VenueId = _newVenueId,
                ActId = _newActId,
                StartDate = new DateTime(2025, 1, 3),
                EndDate = new DateTime(2025, 3, 3)
            };

            var result1 = await TestUtils.Post(client, _showsRoute, show).ConfigureAwait(false);
            var newShow1 = JsonConvert.DeserializeObject<ShowJSON>
                (await result1.Content.ReadAsStringAsync().ConfigureAwait(false));
            _newShow1Id = newShow1.Id;

            var show2 = new ShowJSON()
            {
                VenueId = _newVenueId,
                ActId = _newActId,
                StartDate = new DateTime(2026, 1, 3),
                EndDate = new DateTime(2026, 3, 3)
            };

            var result2 = await TestUtils.Post(client, _showsRoute, show2).ConfigureAwait(false);
            var newShow2 = JsonConvert.DeserializeObject<ShowJSON>(await result2.Content.ReadAsStringAsync().ConfigureAwait(false));
            _newShow2Id = newShow2.Id;
        }

        [SetUp]
        protected async Task SetUp()
        {
            var ticketsRsv1 = new TicketJSONRequest()
            {
                ShowId = _newShow1Id,
                Quantity = 3,
                Price = 30
            };

            var ticketsRsv2 = new TicketJSONRequest()
            {
                ShowId = _newShow2Id,
                Quantity = 3,
                Price = 30
            };

            using var client = new HttpClient();

            var result1 = await TestUtils.Post(client, _ticketsRoute, ticketsRsv1).ConfigureAwait(false);
            Assert.IsTrue(result1.IsSuccessStatusCode);
            _ticketsLot1 = JsonConvert.DeserializeObject<ICollection<TicketJSONReply>>(await result1.Content.ReadAsStringAsync().ConfigureAwait(false));
            Assert.IsTrue(_ticketsLot1.All(t => t.Id > 0));
            Assert.IsTrue(_ticketsLot1.All(t => t.ShowId == ticketsRsv1.ShowId));
            Assert.IsTrue(_ticketsLot1.All(t => t.Price == ticketsRsv1.Price));

            var result2 = await TestUtils.Post(client, _ticketsRoute, ticketsRsv2).ConfigureAwait(false);
            Assert.IsTrue(result2.IsSuccessStatusCode);
            var _ticketsLot2 = JsonConvert.DeserializeObject<ICollection<TicketJSONReply>>(await result2.Content.ReadAsStringAsync().ConfigureAwait(false));
            Assert.IsTrue(_ticketsLot2.All(t => t.Id > 0));
            Assert.IsTrue(_ticketsLot2.All(t => t.ShowId == ticketsRsv2.ShowId));
            Assert.IsTrue(_ticketsLot2.All(t => t.Price == ticketsRsv2.Price));
        }

        [TearDown]
        public async Task TearDown()
        {
            using var client = new HttpClient();
            var result = await client.GetAsync(new Uri(_ticketsRoute)).ConfigureAwait(false);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var tickets = JsonConvert.DeserializeObject<ICollection<TicketJSONReply>>
                (await result.Content.ReadAsStringAsync().ConfigureAwait(false));
            foreach (var ticket in tickets)
            {
                result = await client.DeleteAsync(new Uri(_ticketsRoute + ticket.Id)).ConfigureAwait(false);
                Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            }
        }

        [OneTimeTearDown]
        public async Task OneTimeTearDown()
        {
            //the following should also delete tickets and shows due to cascading deletes
            await TestUtils.DeleteVenue(_venuesRoute, _newVenueId).ConfigureAwait(false);
            await TestUtils.DeleteAct(_actsRoute, _newActId).ConfigureAwait(false);
        }

        [Test]
        public async Task ReserveTicketsMultipleThreadsTest()
        {
            var ticketsRsv = new TicketJSONRequest()
            {
                ShowId = _newShow1Id,
                Quantity = 2,
                Price = 30
            };

            List<Task> taskList = new List<Task>();
            using var client = new HttpClient();
            taskList.Add(TestUtils.Post(client, _ticketsRoute, ticketsRsv));
            taskList.Add(TestUtils.Post(client, _ticketsRoute, ticketsRsv));
            taskList.Add(TestUtils.Post(client, _ticketsRoute, ticketsRsv));
            await Task.WhenAll(taskList.ToArray()).ConfigureAwait(false);


            var result = await client.GetAsync(new Uri(_ticketsRoute + "show/" + _newShow1Id)).ConfigureAwait(false);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var savedTickets = JsonConvert.DeserializeObject<ICollection<TicketJSONReply>>
                (await result.Content.ReadAsStringAsync().ConfigureAwait(false));
            Debug.WriteLine(savedTickets.Count);
            Assert.IsTrue(savedTickets.Count >= 3 && savedTickets.Count <= _venue.Capacity);
        }
    }
}
