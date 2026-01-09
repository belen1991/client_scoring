using System.Text.Json.Serialization;
using System.Text.Json;
using CustomerValidator.Application.Interfaces;
using Microsoft.Extensions.Options;
using CustomerValidator.Infrastructure.Settings;
using System.Net.Http.Headers;
using CustomerValidator.Infrastructure.Models;
using System.Text;

namespace CustomerValidator.Infrastructure.ExternalService;
public class FinScoreClient : IFinScore
{
    private readonly HttpClient _httpClient;
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
        PropertyNameCaseInsensitive = true,
        WriteIndented = true
    };

    public FinScoreClient(
        HttpClient httpClient,
        IOptions<FinScoreSettings> options)
    {
        var settings = options.Value;

        _httpClient.BaseAddress = new Uri(settings.BaseUrl);
        _httpClient.DefaultRequestHeaders.Add("x-api-key", settings.ApiKey);
        _httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

        _httpClient = httpClient;

    }

    public async Task<int?> GetScoreAsync(
        string name,
        string identificator,
        CancellationToken cs)
    {
        var request = new FinScoreRequest
        {
            Job = identificator,
            Name = name
        };

        var response = await PostAsync<FinScoreRequest, FinScoreResponse>(
            "api/users",
            request,
            cancellationToken: cs);

        return response?.Id;
    }

    private async Task<TResponse> PostAsync<TRequest, TResponse>(
        string path,
        TRequest body,
        CancellationToken cancellationToken = default)
    {
        var json = JsonSerializer.Serialize(body, _jsonOptions);

        var request = new HttpRequestMessage(HttpMethod.Post, path)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

        var response = await _httpClient.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new HttpRequestException(
                $"FinScore error {response.StatusCode}: {errorBody}");
        }

        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonSerializer.Deserialize<TResponse>(content, _jsonOptions)!;
    }

}
