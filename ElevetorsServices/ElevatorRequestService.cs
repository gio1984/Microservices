using System;
using System.Data;
using System.Threading.Tasks;

public class ElevatorRequestService : BackgroundService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<ElevatorRequestService> _logger;

    public ElevatorRequestService(IHttpClientFactory httpClientFactory, ILogger<ElevatorRequestService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Add("X-Api-Key", "secret");

        int[] elevatorIds =
        [
            123456, 112233, 443311, 654321, 789076
        ];

        string[] status =
        [
            "running", "idle"
        ];

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var payload = new ElevatorModel
                {
                    Date = DateOnly.FromDateTime(DateTime.Now),
                    Id = elevatorIds[Random.Shared.Next(0, 4)],
                    Floor = Random.Shared.Next(1, 15),
                    status = status[Random.Shared.Next(0, 1)]
                };
                var response = await client.PostAsJsonAsync("http://microservices:80/api/elevator/", payload, stoppingToken);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Data sent successfully at {Time}", DateTime.UtcNow);
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
            await Task.Delay(TimeSpan.FromSeconds(15), stoppingToken);
        }

    }
}