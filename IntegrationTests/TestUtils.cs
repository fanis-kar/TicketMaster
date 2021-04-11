using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TicketMaster.Api.Errors;
using TicketMaster.Api.Model;

namespace TicketMaster.IntegrationTests
{
    internal static class TestUtils
    {
        internal static async Task<long> CreateVenue(HttpClient client, string venuesRoute, VenueJSON venue)
        {
            var result = await Post(client, venuesRoute, venue).ConfigureAwait(false);
            Assert.IsTrue(result.IsSuccessStatusCode);
            var newVenue = JsonConvert.DeserializeObject<VenueJSON>(await result.Content.ReadAsStringAsync().ConfigureAwait(false));
            Assert.IsTrue(newVenue.Name == venue.Name);
            Assert.IsTrue(result.IsSuccessStatusCode);
            Assert.IsTrue(newVenue.Id > 0);
            Assert.IsTrue(newVenue.Name == venue.Name);
            Assert.IsTrue(newVenue.Capacity == venue.Capacity);
            return newVenue.Id;
        }

        internal static async Task<long> CreateAct(HttpClient client, string actsRoute, ActJSON act)
        {
            var result = await Post(client, actsRoute, act).ConfigureAwait(false);
            Assert.IsTrue(result.IsSuccessStatusCode);
            var newAct = JsonConvert.DeserializeObject<ActJSON>(await result.Content.ReadAsStringAsync().ConfigureAwait(false));
            Assert.IsTrue(newAct.Id > 0);
            Assert.IsTrue(newAct.Name == act.Name);
            return newAct.Id;
        }        

        internal static async Task<HttpResponseMessage> Post(HttpClient client, string route, Object obj)
        {
            using var textContent = new ByteArrayContent(Encoding.UTF8.GetBytes(
               JsonConvert.SerializeObject(obj)));
            textContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return await client.PostAsync(new Uri(route), textContent).ConfigureAwait(false);
        }        

        internal static async Task CheckForNotFound(HttpResponseMessage result)
        {
            var error = JsonConvert.DeserializeObject<ErrorDetails>(await result.Content.ReadAsStringAsync().ConfigureAwait(false));
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            Assert.True(error.Message.Contains("not found", StringComparison.InvariantCulture));
            Assert.True(error.StatusCode == (int)HttpStatusCode.NotFound);
        }

        internal static async Task DeleteAct(string actsRoute, long newActId)
        {
            using var client = new HttpClient();
            
            //Delete Act
            var result = await client.DeleteAsync(new Uri(actsRoute+newActId)).ConfigureAwait(false);
            Assert.IsTrue(result.IsSuccessStatusCode);
            result = await client.GetAsync(new Uri(actsRoute+newActId)).ConfigureAwait(false);
            await TestUtils.CheckForNotFound(result).ConfigureAwait(false);
        }

        internal static async Task DeleteVenue(string venuesRoute, long newVenueId)
        {
            using var client = new HttpClient();

            //Delete Venue
            var result = await client.DeleteAsync(new Uri(venuesRoute + newVenueId)).ConfigureAwait(false);
            Assert.IsTrue(result.IsSuccessStatusCode);
            result = await client.GetAsync(new Uri(venuesRoute + newVenueId)).ConfigureAwait(false);
            await TestUtils.CheckForNotFound(result).ConfigureAwait(false);
        }
    }
}
