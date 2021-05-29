using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using GroceryStoreAPI.ViewModel.Requests;
using GroceryStoreAPI.ViewModel.Responses;
using Xunit;

namespace GroceryStoreAPI.Tests.IntegrationTests
{
    [Collection("Sequential")]
    public class GroceryStoreAPIIntegrationTests : IClassFixture<TestFixture<TestStartup>>
    {
        private readonly JsonSerializerOptions jsonSerializerOptions;

        public GroceryStoreAPIIntegrationTests(TestFixture<TestStartup> fixture)
        {
            httpClient = fixture.httpClient;

            jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        private HttpClient httpClient { get; }


        [Fact]
        public async Task Api_Get_Should_Return_Success_With_Data()
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod("GET"), "api/Customer/100");

            // Act
            var response = await httpClient.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            var getCustomerResponse = JsonSerializer.Deserialize<GetCustomerResponse>(content, jsonSerializerOptions);

            getCustomerResponse.Should().NotBeNull();
            getCustomerResponse.Customer.Id.Should().Be(100);
        }

        [Fact]
        public async Task Api_Get_All_Should_Return_Success_With_Data()
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod("GET"), "api/Customer/all");

            // Act
            var response = await httpClient.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            var queryCustomersResponse =
                JsonSerializer.Deserialize<QueryCustomerResponse>(content, jsonSerializerOptions);

            queryCustomersResponse.Should().NotBeNull();
            queryCustomersResponse.Customers.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task Api_Post_Should_Return_Success_Create_Data()
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod("POST"), "api/Customer");
            var createCustomerRequest = new CreateCustomerRequest
            {
                Name = "Joe doe"
            };
            request.Content = new StringContent(JsonSerializer.Serialize(createCustomerRequest, jsonSerializerOptions),
                Encoding.UTF8, "application/json");
            // Act
            var response = await httpClient.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var content = await response.Content.ReadAsStringAsync();
            var createCustomerResponse =
                JsonSerializer.Deserialize<CreateCustomerResponse>(content, jsonSerializerOptions);

            createCustomerResponse.Should().NotBeNull();
            createCustomerResponse.Customer.Name.Should().Be("Joe doe");
        }

        [Fact]
        public async Task Api_Put_Should_Return_Success_Update_Data()
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod("PUT"), "api/Customer");
            var updateCustomerRequest = new UpdateCustomerRequest
            {
                Id = 1,
                Name = "Jane Smith"
            };
            request.Content = new StringContent(JsonSerializer.Serialize(updateCustomerRequest, jsonSerializerOptions),
                Encoding.UTF8, "application/json");
            // Act
            var response = await httpClient.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            var updateCustomerResponse =
                JsonSerializer.Deserialize<UpdateCustomerResponse>(content, jsonSerializerOptions);

            updateCustomerResponse.Should().NotBeNull();
            updateCustomerResponse.Customer.Name.Should().Be("Jane Smith");
        }
    }
}