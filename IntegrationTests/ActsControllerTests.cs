using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TicketMaster.Api.Filter;
using TicketMaster.Api.Model;
using TicketMaster.Business.Interfaces;
using TicketMaster.Business.Services;
using TicketMaster.Data.Context;
using TicketMaster.Data.Interfaces;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
using TicketMaster.Api.Errors;

namespace TicketMaster.IntegrationTests
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ActsControllerTests
    {        
        private long _act1Id, _act2Id;
        private ActJSON _act1;
        private string _route;

        [OneTimeSetUp]
        protected void OneTimeSetup()
        {          
            _route = SetupClass.hostname + "api/acts/";
        }

        [SetUp]
        protected async Task Setup()
        {

            _act1 = new ActJSON()
            {
                Name = "The Rolling Stones"
            };

            var act2 = new ActJSON()
            {
                Name = "Motorhead"
            };

            using var client = new HttpClient();
            _act1Id = await TestUtils.CreateAct(client, _route, _act1).ConfigureAwait(false);
            _act2Id = await TestUtils.CreateAct(client, _route, act2).ConfigureAwait(false);

        }

        [TearDown]
        protected async Task TearDown()
        {
            using var client = new HttpClient();
            await client.DeleteAsync(new Uri(_route+_act1Id)).ConfigureAwait(false);
            await client.DeleteAsync(new Uri(_route+_act2Id)).ConfigureAwait(false);
        }       

        [Test]
        public async Task RetrieveAllActsTest()
        {
            using var client = new HttpClient();
            var result = await client.GetAsync(new Uri(_route)).ConfigureAwait(false);
            var acts = JsonConvert.DeserializeObject<ICollection<ActJSON>>(await result.Content.ReadAsStringAsync().ConfigureAwait(false));
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.IsTrue(acts.Count >= 1);            
        }

        [Test]
        public async Task RetrieveActExistsTest()
        {
            using var client = new HttpClient();
            var result = await client.GetAsync(new Uri(_route + this._act1Id)).ConfigureAwait(false);
            var newAct = JsonConvert.DeserializeObject<ActJSON>(await result.Content.ReadAsStringAsync().ConfigureAwait(false));            
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.IsTrue(newAct.Id == this._act1Id);
            Assert.IsTrue(newAct.Name == this._act1.Name);
        }

        public async Task RetrieveActDoesNotExistTest()
        {
            using var client = new HttpClient();
            var result = await client.GetAsync(new Uri(_route + long.MaxValue)).ConfigureAwait(false);
            await TestUtils.CheckForNotFound(result).ConfigureAwait(false);
        }


        [Test]
        public async Task DeleteActExistsTest()
        {
            using var client = new HttpClient();
            var result = await client.DeleteAsync(new Uri(_route + this._act1Id)).ConfigureAwait(false);           
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));            
        }

        [Test]
        public async Task DeleteActDoesNotExistTest()
        {
            using var client = new HttpClient();
            var result = await client.DeleteAsync(new Uri(_route + long.MaxValue)).ConfigureAwait(false);
            await TestUtils.CheckForNotFound(result).ConfigureAwait(false);
        }
    }
}
