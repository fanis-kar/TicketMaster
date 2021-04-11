using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TicketMaster.Api.Errors;
using TicketMaster.Api.Model;

namespace TicketMaster.IntegrationTests
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    class ShowsControllerTests
    {
          
        private string _venuesRoute, _actsRoute, _showsRoute;
        private long _newVenueId;
        private long _newActId;
        private ShowJSON _newShow, _newShow2;

        [OneTimeSetUp]
        protected async Task OneTimeSetup()
        {                       
            _venuesRoute = SetupClass.hostname + "api/venues/";
            _actsRoute = SetupClass.hostname + "api/acts/";
            _showsRoute = SetupClass.hostname + "api/shows/";

            VenueJSON _venue = new VenueJSON()
            {
                Name = "Test Venue 1",
                Capacity = 300
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
        }

        [OneTimeTearDown]
        public async Task OneTimeTearDown()
        {
            await TestUtils.DeleteVenue(_venuesRoute, _newVenueId).ConfigureAwait(false);
            await TestUtils.DeleteAct(_actsRoute, _newActId).ConfigureAwait(false);
        }

        [SetUp]
        public async Task SetUp()
        {
            using var client = new HttpClient();

            var newShow = new ShowJSON()
            {
                VenueId = _newVenueId,
                ActId = _newActId,
                StartDate = new DateTime(2025, 1, 1),
                EndDate = new DateTime(2025, 3, 15)
            };

            var result1 = await TestUtils.Post(client, _showsRoute, newShow).ConfigureAwait(false);
            Assert.That(result1.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            _newShow = JsonConvert.DeserializeObject<ShowJSON>(await result1.Content.ReadAsStringAsync().ConfigureAwait(false));
            Assert.IsTrue(_newShow.Id > 0);
            Assert.AreEqual(_newShow.VenueId, newShow.VenueId);
            Assert.AreEqual(_newShow.ActId, newShow.ActId);
            Assert.AreEqual(_newShow.StartDate, newShow.StartDate);
            Assert.AreEqual(_newShow.EndDate, newShow.EndDate);

            var show2 = new ShowJSON()
            {
                VenueId = _newVenueId,
                ActId = _newActId,
                StartDate = new DateTime(2026, 1, 3),
                EndDate = new DateTime(2026, 3, 3)
            };

            var result2 = await TestUtils.Post(client, _showsRoute, show2).ConfigureAwait(false);
            Assert.That(result2.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            _newShow2 = JsonConvert.DeserializeObject<ShowJSON>(await result2.Content.ReadAsStringAsync().ConfigureAwait(false));
            Assert.IsTrue(_newShow2.Id > 0);
            Assert.AreEqual(_newShow2.VenueId, show2.VenueId);
            Assert.AreEqual(_newShow2.ActId, show2.ActId);
            Assert.AreEqual(_newShow2.StartDate, show2.StartDate);
            Assert.AreEqual(_newShow2.EndDate, show2.EndDate);
        }

        [TearDown]
        public async Task TearDown()
        {
            using var client = new HttpClient();
            await client.DeleteAsync(new Uri(_showsRoute+_newShow.Id)).ConfigureAwait(false);
            await client.DeleteAsync(new Uri(_showsRoute + _newShow2.Id)).ConfigureAwait(false);
        }    

                    
        public async Task RetrieveAllShowsTest()
        {
            using var client = new HttpClient();
            var result = await client.GetAsync(new Uri(_showsRoute)).ConfigureAwait(false);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var shows = JsonConvert.DeserializeObject<ICollection<ShowJSON>>
                (await result.Content.ReadAsStringAsync().ConfigureAwait(false));
            Assert.IsTrue(shows.Count > 1); 
        }

        [Test]
        public async Task RetrieveShowExistsTest()
        {
            using var client = new HttpClient();
            var result = await client.GetAsync(new Uri(_showsRoute +_newShow.Id)).ConfigureAwait(false);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var savedShow = JsonConvert.DeserializeObject<ShowJSON>
                (await result.Content.ReadAsStringAsync().ConfigureAwait(false));
            Assert.AreEqual(_newShow.Id, savedShow.Id);
            Assert.AreEqual(_newShow.VenueId, savedShow.VenueId);
            Assert.AreEqual(_newShow.ActId, savedShow.ActId);
            Assert.AreEqual(_newShow.StartDate, savedShow.StartDate);
            Assert.AreEqual(_newShow.EndDate, savedShow.EndDate);
        }

        [Test]
        public async Task RetrieveShowDoesNotExistTest()
        {
            using var client = new HttpClient();
            var result = await client.GetAsync(new Uri(_showsRoute +long.MaxValue)).ConfigureAwait(false);
            await TestUtils.CheckForNotFound(result).ConfigureAwait(false);
        }
        [Test]
        public async Task RetrieveShowByDateRangeTest()
        {
            using var client = new HttpClient();
            var result = await client.GetAsync(new Uri(_showsRoute + "dates/from/"
                + "2000-01-01"+"/to/"+"2050-01-01")).ConfigureAwait(false);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var savedShows = JsonConvert.DeserializeObject<ICollection<ShowJSON>>
                (await result.Content.ReadAsStringAsync().ConfigureAwait(false));
            Assert.True(savedShows.Count > 1);
        }
        
        [Test]
        public async Task RetrieveShowByVenueTest()
        {
            using var client = new HttpClient();
            var result = await client.GetAsync(new Uri(_showsRoute + "venue/"+ _newVenueId)).ConfigureAwait(false);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var savedShows = JsonConvert.DeserializeObject<ICollection<ShowJSON>>
                (await result.Content.ReadAsStringAsync().ConfigureAwait(false));
            Assert.AreEqual(2, savedShows.Count);
        }

        public async Task CreateShowWhenVenueDoesNotExistTest()
        {
            var show = new ShowJSON()
            {
                VenueId = long.MaxValue,
                ActId = _newActId,
                StartDate = new DateTime(2025, 1, 3),
                EndDate = new DateTime(2025, 3, 3)
            };
            using var client = new HttpClient();
            await TestUtils.Post(client, _showsRoute, show).ConfigureAwait(false);            
        }

        public async Task CreateShowWhenActDoesNotExistTest()
        {
            var show = new ShowJSON()
            {
                VenueId = _newVenueId,
                ActId = long.MaxValue,
                StartDate = new DateTime(2025, 1, 3),
                EndDate = new DateTime(2025, 3, 3)
            };
            
            using var client = new HttpClient();
            var result = await TestUtils.Post(client, _showsRoute, show).ConfigureAwait(false);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
            var error = JsonConvert.DeserializeObject<ErrorDetails>(await result.Content.ReadAsStringAsync().ConfigureAwait(false));
            Assert.True(error.Message.Contains("Internal", StringComparison.InvariantCulture));
            Assert.True(error.StatusCode == (int)HttpStatusCode.InternalServerError);
        }

        [Test]
        public async Task DeleteShowExistsTest()
        {
            using var client = new HttpClient();
            var result = await client.DeleteAsync(new Uri(_showsRoute + _newShow.Id)).ConfigureAwait(false);
            Assert.IsTrue(result.IsSuccessStatusCode);
        }

        [Test]
        public async Task DeleteShowDoesNotExistTest()
        {
            using var client = new HttpClient();
            var result = await client.DeleteAsync(new Uri(_showsRoute+long.MaxValue)).ConfigureAwait(false);
            await TestUtils.CheckForNotFound(result).ConfigureAwait(false);
        }
        
    }
}
