using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontEnd.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    public List<DoorsModel> ApiResponseDoors { get; set; }
    public List<ElevatorModel> ApiResponseElevators { get; set; }
    public List<SmokeDetectorModel> ApiResponseSmokeDetectors { get; set; }

    public IndexModel(ILogger<IndexModel> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    public async Task OnGetAsyncDoorsStatus()
    {
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Add("X-Api-Key", "secret");
        var response = await client.GetAsync("http://webapi:80/api/doorsstatus");
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Failed to fetch doors status: {StatusCode}", response.StatusCode);
            ApiResponseDoors = [];
            return;
        }
        ApiResponseDoors = await response.Content.ReadFromJsonAsync<List<DoorsModel>>();
        _logger.LogInformation("Doors statuses received: {ApiResponse}", ApiResponseDoors.Count);
    }

    public async Task OnGetAsyncElevatorsStatus()
    {
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Add("X-Api-Key", "secret");
        var response = await client.GetAsync("http://webapi:80/api/elevatorsstatus");
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Failed to fetch elevators status: {StatusCode}", response.StatusCode);
            ApiResponseElevators = [];
            return;
        }
        ApiResponseElevators = await response.Content.ReadFromJsonAsync<List<ElevatorModel>>();
        _logger.LogInformation("Elevator statuses received: {ApiResponse}", ApiResponseElevators.Count);
    }

    public async Task OnGetAsyncSmokeDetectorsStatus()
    {
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Add("X-Api-Key", "secret");
        var response = await client.GetAsync("http://webapi:80/api/smokedetectorsstatus");
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Failed to fetch elevators status: {StatusCode}", response.StatusCode);
            ApiResponseSmokeDetectors = [];
            return;
        }
        ApiResponseSmokeDetectors = await response.Content.ReadFromJsonAsync<List<SmokeDetectorModel>>();
        _logger.LogInformation("Elevator statuses received: {ApiResponse}", ApiResponseSmokeDetectors.Count);
    }

    public async Task OnGetAsync()
    {
        await OnGetAsyncDoorsStatus();
        await OnGetAsyncElevatorsStatus();
        await OnGetAsyncSmokeDetectorsStatus();
        _logger.LogInformation("Received information from database");
    }
}
