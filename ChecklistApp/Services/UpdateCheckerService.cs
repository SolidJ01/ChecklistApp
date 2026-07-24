using System.Text.Json;
using ChecklistApp.Model.Remote;
using ChecklistApp.ViewModel;

namespace ChecklistApp.Services;

public class UpdateCheckerService
{
    private readonly string _connectionString;
    private readonly HttpClient _client;

    public UpdateCheckerService(HttpClient client)
    {
        _client = client;
        _connectionString = $"https://www.fastcode.se/api/software/releases/latest/checklist/{DeviceInfo.Current.Platform.ToString().ToLower()}";
    }

    public async Task<Release> GetLatestRelease()
    {
        var response = await _client.GetStringAsync(_connectionString);

        //response = """{"version":{"major":1,"minor":0,"patch":0},"published":"2026-03-27T12:00:00"}""";
        
        return JsonSerializer.Deserialize<Release>(response, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
}