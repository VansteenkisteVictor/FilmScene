using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MovieApp_v2;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationTest
{
    [TestClass]
    public class IntegrationTest
    {
        private HttpClient _client; //.NetHttp
        private WebApplicationFactory<Startup> _factory;
        public void MovieControllerIntegrationTests()
        {
            _factory = new WebApplicationFactory<Startup>();
            _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }
        [TestMethod]
        public async Task ClientGETstudents_returns_Students()
        {
            var httpResponse = await _client.GetAsync("/Movie/Index");
            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            Assert.AreEqual("text/html; charset=utf-8",
           httpResponse.Content.Headers.ContentType.ToString());
            Assert.IsTrue(stringResponse.Contains("TitleMovie"));
        }
    }
}
