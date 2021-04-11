using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public class VenuesControllerTests
    {
        private readonly VenueJSON [] _venues = new VenueJSON[2];
        private readonly long[] _venueIds = new long[2];
        private string _route;

        [OneTimeSetUp]
        protected void OneTimeSetup()
        {
            _venues[0] = new VenueJSON()
            {
                Name = "Hammersmith Odeon",
                Capacity =2000
            };

            _venues[1] = new VenueJSON()
            {
                Name = "Brixton Academy",
                Capacity = 1500            
            };

            _route = SetupClass.hostname + "api/venues/";            
        }

        [SetUp]
        protected async Task Setup()
        {
            using var client = new HttpClient();
            _venueIds[0] = await TestUtils.CreateVenue(client, _route, _venues[0]).ConfigureAwait(false);
            _venueIds[1] = await TestUtils.CreateVenue(client, _route, _venues[1]).ConfigureAwait(false);            
        }

        [TearDown]
        protected async Task TearDown()
        {
            using var client = new HttpClient();
            await client.DeleteAsync(new Uri(_route+_venueIds[0])).ConfigureAwait(false);
            await client.DeleteAsync(new Uri(_route+_venueIds[1])).ConfigureAwait(false);
        }
        
        [Test]
        public async Task RetrieveAllVenuesTest()
        {
            using var client = new HttpClient();           
           var result = await client.GetAsync(new Uri(_route)).ConfigureAwait(false);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var venues = JsonConvert.DeserializeObject<ICollection<VenueJSON>>(await result.Content.ReadAsStringAsync().ConfigureAwait(false));           
            Assert.IsTrue(venues.Count >= 1);
        }

        [Test]
        public async Task RetrieveVenueExistsTest()
        {
            using var client = new HttpClient();
            var result = await client.GetAsync(new Uri(_route + this._venueIds[0])).ConfigureAwait(false);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var newVenue = JsonConvert.DeserializeObject<VenueJSON>(await result.Content.ReadAsStringAsync().ConfigureAwait(false));            
            Assert.IsTrue(newVenue.Name == _venues[0].Name);
            Assert.IsTrue(newVenue.Id == this._venueIds[0]);
        }
        [Test]
        public async Task RetrieveVenueDoesNotExistTest()
        {
            using var client = new HttpClient();
            var result = await client.GetAsync(new Uri(_route + long.MaxValue)).ConfigureAwait(false);
            await TestUtils.CheckForNotFound(result).ConfigureAwait(false);           
        }

        [Test]
        public async Task DeleteVenueExistsTest()
        {
            using var client = new HttpClient();
            var result = await client.DeleteAsync(new Uri(_route+this._venueIds[0])).ConfigureAwait(false);
            Assert.IsTrue(result.IsSuccessStatusCode);
        }

        [Test]
        public async Task DeleteVenueDoesNotExistTest()
        {                      
            using var client = new HttpClient();
            var result = await client.DeleteAsync(new Uri(_route + long.MaxValue)).ConfigureAwait(false);
            await TestUtils.CheckForNotFound(result).ConfigureAwait(false);
        }
    }
}
