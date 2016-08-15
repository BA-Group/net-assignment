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
    // Shared context, ugly perhaps?
    public class ContactFixture
    {
        public TestServer Server { get; private set;}
        public HttpClient Client { get; private set;}
        public string Url { get; private set;}
        public Contact Contact { get; set;}
        public ContactFixture()
        {
            Server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>());
            Client = Server.CreateClient();
            Url = "/contact";
            Contact =  new Contact()
                {
                FirstName = "Guest",
                LastName = "User",
                Email = "guest@localhost.test"
            };
        }
    }
    public class ContactControllerShould: IClassFixture<ContactFixture>
    {
        private readonly ContactFixture _fix;
        public ContactControllerShould(ContactFixture fix)
        {
          _fix = fix;
        }
        [Fact]
        public async Task Return404IfContactNotFoundTest()
        {
            var res = await _fix.Client.GetAsync(_fix.Url + "/-1");
            Assert.Equal("NotFound", res.StatusCode.ToString());
        }
        [Fact]
        public async Task ReturnOkIDWhenCreatedContactTest()
        {
            var res = await _fix.Client.PostAsync(_fix.Url, new StringContent(JsonConvert.SerializeObject(_fix.Contact),
                Encoding.UTF8, "application/json"));
            Assert.Equal("Created", res.StatusCode.ToString());
            var id = long.Parse(await res.Content.ReadAsStringAsync());
            Assert.True(id > 0);
            _fix.Contact.Id = id;
        }
        [Fact]
        public async Task Return400IfBadDataWhenCreatingContactTest()
        {
            var res = await _fix.Client.PostAsync(_fix.Url, new StringContent(JsonConvert.SerializeObject(new Contact()
            {
                FirstName = "Not"
            }), Encoding.UTF8, "application/json"));
            Assert.Equal("BadRequest", res.StatusCode.ToString());
        }
        [Fact]
        public async Task ReturnOkContactWhenFetchingAllTest()
        {
            var res = await _fix.Client.GetAsync(_fix.Url);
            Assert.Equal("Ok", res.StatusCode.ToString());
            var contacts = JsonConvert.DeserializeObject<List<Contact>>(await res.Content.ReadAsStringAsync());
            Assert.True(contacts.Contains(_fix.Contact));
        }
        [Fact]
        public async Task ReturnOkRowsAffectedWhenUpdatingTest()
        {
            _fix.Contact.City = "Turku";
            var res = await _fix.Client.PutAsync(_fix.Url + "/" + _fix.Contact.Id, new StringContent(JsonConvert.SerializeObject(_fix.Contact),
                Encoding.UTF8, "application/json"));
            Assert.Equal("Ok", res.StatusCode.ToString());
            var rows = int.Parse(await res.Content.ReadAsStringAsync());
            Assert.True(rows > 0);
        }
        [Fact]
        public async Task ReturnOkContactWhenFetchingByIdTest()
        {
            var res = await _fix.Client.GetAsync(_fix.Url + "/" + _fix.Contact.Id);
            Assert.Equal("Ok", res.StatusCode.ToString());
            var jsString = await res.Content.ReadAsStringAsync();
            Assert.Equal(JsonConvert.SerializeObject(_fix.Contact), jsString);
        }
        [Fact]
        public async Task ReturnOkRowsAffectedWhenDeletingContactTest()
        {
            var res = await _fix.Client.DeleteAsync(_fix.Url + "/" + _fix.Contact.Id);
            Assert.Equal("Ok", res.StatusCode.ToString());
            var rows = int.Parse(await res.Content.ReadAsStringAsync());
            Assert.True(rows > 0);
        }
    }
}