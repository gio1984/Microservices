

public class SmokeDetectorsRequestService : BackgroundService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<SmokeDetectorsRequestService> _logger;

    public SmokeDetectorsRequestService(IHttpClientFactory httpClientFactory, ILogger<SmokeDetectorsRequestService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Add("X-Api-Key", "secret");

        int[] smokeDetectorIds =
        [
            987654, 112233, 443311, 654321, 789076
        ];

        string[] status =
        [
            "Nothing detected", "Small quantity of smoke detected", "Large quantity of smoke detected", "Fire detected", "Smoke detector malfunction"
        ];

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var payload = new SmokeDetectorModel
                {
                    EventIdUtc = DateTime.UtcNow,
                    DetectorId = smokeDetectorIds[Random.Shared.Next(0, 5)],
                    Status = status[Random.Shared.Next(0, 5)]
                };
                var response = await client.PostAsJsonAsync("http://webapi:80/api/smokedetector/", payload, stoppingToken);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Data sent successfully at {EventiId}", payload.EventIdUtc);
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