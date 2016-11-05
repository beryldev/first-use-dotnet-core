using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Wrhs.Data;
using Xunit;

namespace Wrhs.WebApp.Tests.Integration
{
    public class ProductsTests : IDisposable
    {
        readonly TestServer server;
        readonly HttpClient client; 

        readonly WrhsContext context;

        public ProductsTests()
        {
            server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            client = server.CreateClient();
            context = server.Host.Services.GetService(typeof(WrhsContext)) as WrhsContext;
            context.Database.EnsureCreated();
        }

        [Theory]
        [InlineData("api/product")]
        [InlineData("api/product?name=somename")]
        [InlineData("api/product?code=somecode")]
        [InlineData("api/product?code=somecode&name=somename")]
        public async Task ShouldReturnEmptyPaginationPage(string url)
        {
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Equal("{\"items\":[],\"page\":1,\"perPage\":10,\"total\":0,\"totalPages\":0}", responseString);
        }

        [Theory]
        [InlineData("api/product/1")]
        [InlineData("api/product/a")]
        public async Task ShouldReturnNotFound(string url)
        {
            var response = await client.GetAsync(url);
            
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task ShouldReturnOk()
        {        
            var content = new StringContent("{\"code\": \"PROD1\", \"name\": \"Product 1\", \"ean\": \"123456\"}", System.Text.Encoding.UTF8);
            content.Headers.ContentType.MediaType = "application/json";
            var response = await client.PostAsync("api/product", content);
            
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task ShouldReturnBadRequest()
        {        
            var content = new StringContent("{\"code\": \"\", \"name\": \"Product 1\", \"ean\": \"123456\"}", System.Text.Encoding.UTF8);
            content.Headers.ContentType.MediaType = "application/json";
            var response = await client.PostAsync("api/product", content);
            
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task ShouldReturnCreatedProduct()
        {        
            var content = new StringContent("{\"code\": \"PROD1\", \"name\": \"Product 1\", \"ean\": \"123456\"}", System.Text.Encoding.UTF8);
            content.Headers.ContentType.MediaType = "application/json";
            await client.PostAsync("api/product", content);
            var response = await client.GetAsync("api/product/1");
            response.EnsureSuccessStatusCode();
            
            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Equal("{\"id\":1,\"code\":\"PROD1\",\"name\":\"Product 1\",\"ean\":\"123456\",\"sku\":null,\"description\":null}", responseString);
        }

        public void Dispose()
        {
            context.Database.EnsureDeleted();
        }
    }
}