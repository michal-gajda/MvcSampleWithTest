namespace MvcSampleWithTest.IntegrationTests
{
    using System.IO;
    using System.Net.Http;
    using System.Reflection;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.Configuration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MvcSampleWithTest.WebApi;

    [TestClass]
    public class IntegrationTests
    {
        private readonly TestServer server;
        private readonly HttpClient client;

        public IntegrationTests()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            var build = new WebHostBuilder()
                .UseContentRoot(Path.GetDirectoryName(Assembly.GetAssembly(typeof(Startup)).Location))
                .UseConfiguration(configuration)
                .ConfigureTestServices(services => { })
                .UseStartup<Startup>();
            this.server = new TestServer(build);
            this.client = this.server.CreateClient();
        }

        [TestMethod]
        public async Task WeatherForecastControllerTest()
        {
            //Arrange

            //Act
            var response = await this.client.GetStringAsync("/WeatherForecast");

            //Assert
            response.Should().NotBeNullOrWhiteSpace();
        }

        [TestMethod]
        public async Task HealthChecksTest()
        {
            //Arrange

            //Act
            var response = await this.client.GetStringAsync("/health");

            //Assert
            response.Should().NotBeNullOrWhiteSpace();
            response.Should().Equals("Healthy");
        }
    }
}
