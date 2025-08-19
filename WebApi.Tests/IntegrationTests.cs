namespace WebApi.Tests;

using Xunit;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

public class IntegrationTests
{
    private readonly ILogger<IntegrationTests> _logger;

    public IntegrationTests()
    {
        var factory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        });

        _logger = factory.CreateLogger<IntegrationTests>();
    }

    private class ElevatorTestModel
    {
        public DateTime EventIdUtc { get; set; }
        public int ElevatorId { get; set; }
        public int Floor { get; set; }
        public required string Status { get; set; }
    }

    [Fact]
    public async Task NotAuthorizedRequest()
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Post, "http://webapi:80/api/elevator/");
        request.Headers.Add("X-Api-Key", "wrongkey");
        request.Content = new StringContent("{}", System.Text.Encoding.UTF8, "application/json");
        var responseTask = await client.SendAsync(request);
        // Assert
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, responseTask.StatusCode);
        //Assert.Contains("Unauthorized", responseTask.Content.ReadAsStringAsync().Result);
    }

    [Fact]
    public async Task WrongData()
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Post, "http://webapi:80/api/elevator/");
        request.Headers.Add("X-Api-Key", "secret");
        request.Content = new StringContent("{\"EventIdUtc\":\"123456\",\"Elevator\":1,\"Flor\":5,\"Stat\":\"Running\"}", System.Text.Encoding.UTF8, "application/json");
        var responseTask = await client.SendAsync(request);
        // Assert
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, responseTask.StatusCode);
    }

    [Fact]
    public async Task ValidRequest()
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Post, "http://webapi:80/api/elevator/");
        request.Headers.Add("X-Api-Key", "secret");
        DateTime eventId = DateTime.UtcNow;
        string payload = "{\"EventIdUtc\":\"{eventId}\",\"ElevatorId\":1,\"Floor\":5,\"Status\":\"Running\"}";
        payload = payload.Replace("{eventId}", eventId.ToString("o")); // ISO 8601 format
        request.Content = new StringContent(payload, System.Text.Encoding.UTF8, "application/json");
        var responseTask = await client.SendAsync(request);
        //Assert
        Assert.Equal(System.Net.HttpStatusCode.OK, responseTask.StatusCode);
    }

    [Fact]
    public async Task EndToEndValidRequest()
    {
        // send POST with valid data
        var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Post, "http://webapi:80/api/elevator/");
        request.Headers.Add("X-Api-Key", "secret");
        DateTime endToEndEventId = DateTime.UtcNow;
        string payload = "{\"EventIdUtc\":\"{eventId}\",\"ElevatorId\":1,\"Floor\":5,\"Status\":\"Running\"}";
        payload = payload.Replace("{eventId}", endToEndEventId.ToString("o")); // ISO 8601 format
        request.Content = new StringContent(payload, System.Text.Encoding.UTF8, "application/json");
        var responseTask = await client.SendAsync(request);
        //Assert
        Assert.Equal(System.Net.HttpStatusCode.OK, responseTask.StatusCode);
        // retrieve last data from API
        client = new HttpClient();
        request = new HttpRequestMessage(HttpMethod.Get, "http://webapi:80/api/elevatorsstatus/");
        request.Headers.Add("X-Api-Key", "secret");
        var getResponseTask = await client.SendAsync(request);
        List<ElevatorTestModel> responseContent = await getResponseTask.Content.ReadFromJsonAsync<List<ElevatorTestModel>>();
        DateTime lastUtcId = responseContent.Where(x => x.ElevatorId == 1).Max(x => x.EventIdUtc);
        // Assert removing eventual vary small differences in milliseconds
        var differences = (endToEndEventId - lastUtcId).Duration();
        Assert.True(differences < TimeSpan.FromMilliseconds(1), 
            $"Difference is: {differences.TotalMilliseconds} ms");
    }
}


