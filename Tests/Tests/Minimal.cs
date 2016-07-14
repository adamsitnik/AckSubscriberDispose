using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.PlatformAbstractions;
using Repro;
using Xunit;

namespace Tests
{
    public class Minimal
    {
        [Fact]
        public async Task CanCallControllerAndDisposeEverything()
        {
            var appBasePath = PlatformServices.Default.Application.ApplicationBasePath;

            using (var testServer = new TestServer(
                new WebHostBuilder()
                    .UseContentRoot(appBasePath)
                    .UseStartup<Startup>()))
            {
                using (var httpClient = testServer.CreateClient())
                {
                    var response = await httpClient.GetAsync("Home/Index");

                    Assert.True(response.IsSuccessStatusCode);
                }
            }

            Assert.True(Startup.IsDisposed);
        }
    }
}
