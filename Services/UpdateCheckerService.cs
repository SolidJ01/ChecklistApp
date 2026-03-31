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
        string platform;
        #if ANDROID
        platform = "android";
        #elif IOS
        platform = "ios";
        #endif
        _connectionString = $"http://192.168.50.199:5248/api/software/releases/latest/checklist/{platform}";
    }

    public async Task<Release> GetLatestRelease()
    {
        try
        {
            var response = await _client.GetStringAsync(_connectionString);

            return JsonSerializer.Deserialize<Release>(response);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }
}