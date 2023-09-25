using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using MovieStore.Core.DTOs;
using Newtonsoft.Json;

namespace MovieStore.Testing.Controllers
{
    public class MovieControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public MovieControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }
        [Fact]
        public async Task GetCustomers_ShouldReturnCustomers()
        {
            var response = await _client.GetAsync("/api/v1/customers");

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var customers = JsonConvert.DeserializeObject<ICollection<CustomerDTO>>(content);

            customers.Should().NotBeEmpty();
        }
    }
}
