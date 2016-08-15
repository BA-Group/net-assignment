using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Xunit;
using Newtonsoft.Json;

using net_assignment.Models;

namespace net_assignment.Tests
{
    public class ContactControllerShould
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;
        private readonly string _url = "/contacts";

        private Contact _contact = new Contact()
        {
            FirstName = "Guest",
            LastName = "User",
            Email = "guest@localhost.test"
        };

        public ContactControllerShould()
        {
            _server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>());
            _client = _server.CreateClient();
        }
        [Fact]
        public async Task Return404IfContactNotFoundTest()
        {
            var res = await _client.GetAsync(_url + "/-1");
            Assert.Equal("NotFound", res.StatusCode.ToString());
        }
        [Fact]
        public async Task ReturnOkIDWhenCreatedContactTest()
        {
            var res = await _client.PostAsync(_url, new StringContent(JsonConvert.SerializeObject(_contact),
                Encoding.UTF8, "application/json"));
            Assert.Equal("Created", res.StatusCode.ToString());
            var id = long.Parse(await res.Content.ReadAsStringAsync());
            Assert.True(id > 0);
            _contact.Id = id;
        }
        [Fact]
        public async Task Return400IfBadDataWhenCreatingContactTest()
        {
            var res = await _client.PostAsync(_url, new StringContent(JsonConvert.SerializeObject(new Contact()
            {
                FirstName = "Not"
            }), Encoding.UTF8, "application/json"));
            Assert.Equal("BadRequest", res.StatusCode.ToString());
        }
        [Fact]
        public async Task ReturnOkContactWhenFetchingAllTest()
        {
            var res = await _client.GetAsync(_url);
            Assert.Equal("Ok", res.StatusCode.ToString());
            var contacts = JsonConvert.DeserializeObject<List<Contact>>(await res.Content.ReadAsStringAsync());
            Assert.True(contacts.Contains(_contact));
        }
        [Fact]
        public async Task ReturnOkRowsAffectedWhenUpdatingTest()
        {
            _contact.City = "Turku";
            var res = await _client.PutAsync(_url + "/" + _contact.Id, new StringContent(JsonConvert.SerializeObject(_contact),
                Encoding.UTF8, "application/json"));
            Assert.Equal("Ok", res.StatusCode.ToString());
            var rows = int.Parse(await res.Content.ReadAsStringAsync());
            Assert.True(rows > 0);
        }
        [Fact]
        public async Task ReturnOkContactWhenFetchingByIdTest()
        {
            var res = await _client.GetAsync(_url + "/" + _contact.Id);
            Assert.Equal("Ok", res.StatusCode.ToString());
            var jsString = await res.Content.ReadAsStringAsync();
            Assert.Equal(JsonConvert.SerializeObject(_contact), jsString);
        }
        [Fact]
        public async Task ReturnOkRowsAffectedWhenDeletingContactTest()
        {
            var res = await _client.DeleteAsync(_url + "/" + _contact.Id);
            Assert.Equal("Ok", res.StatusCode.ToString());
            var rows = int.Parse(await res.Content.ReadAsStringAsync());
            Assert.True(rows > 0);
        }
    }
}