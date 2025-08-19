using System;
using System.Data;
using System.Threading.Tasks;

public class DoorRequestService : BackgroundService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<DoorRequestService> _logger;

    public DoorRequestService(IHttpClientFactory httpClientFactory, ILogger<DoorRequestService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Add("X-Api-Key", "secret");

        int[] doorIds =
        [
            987654, 112233, 443311, 654321, 789076
        ];

        string[] status =
        [
            "open", "closed"
        ];

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var payload = new DoorsModel
                {
                    EventIdUtc = DateTime.UtcNow,
                    DoorId = doorIds[Random.Shared.Next(0, 5)],
                    Status = status[Random.Shared.Next(0, 2)]
                };
                var response = await client.PostAsJsonAsync("http://webapi:80/api/door/", payload, stoppingToken);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Data sent successfully with id {EventId}", payload.EventIdUtc);
                }
                else
                {
                    _logger.LogWarning("Failed to send data: {StatusCode}", response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending data");
            }
            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }

    }
}