using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace TicketMaster.IntegrationTests
{
    [SetUpFixture]
    [ExcludeFromCodeCoverage]
    public class SetupClass
    {
        internal const string hostname = "https://localhost:5001/";
        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            Host.CreateDefaultBuilder().ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
              .ConfigureWebHostDefaults(w => w.UseUrls(hostname))
              .Build()
              .Start();
        }
    }
}
