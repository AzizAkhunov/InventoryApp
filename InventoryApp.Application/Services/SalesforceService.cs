using InventoryApp.Application.Common;
using InventoryApp.Application.DTO;
using InventoryApp.Application.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

public class SalesforceService : ISalesforceService
{
    private readonly HttpClient _http;
    private readonly SalesforceSettings _settings;

    public SalesforceService(
        HttpClient http,
        IOptions<SalesforceSettings> settings)
    {
        _http = http;
        _settings = settings.Value;
    }

    public async Task CreateAccountAsync(SalesforceDto dto)
    {
        var tokenRequest = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "client_credentials"),
            new KeyValuePair<string, string>("client_id", _settings.ClientId),
            new KeyValuePair<string, string>("client_secret", _settings.ClientSecret)
        });

        var tokenResponse = await _http.PostAsync(
            "https://orgfarm-a4003d1e20-dev-ed.develop.my.salesforce.com/services/oauth2/token",
            tokenRequest
        );

        var tokenJson = await tokenResponse.Content.ReadAsStringAsync();

        if (!tokenResponse.IsSuccessStatusCode)
            throw new Exception("AUTH ERROR: " + tokenJson);

        dynamic tokenData = JsonConvert.DeserializeObject(tokenJson);

        string accessToken = tokenData.access_token;
        string instanceUrl = tokenData.instance_url;

        _http.DefaultRequestHeaders.Clear();
        _http.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", accessToken);


        var account = new
        {
            Name = dto.Company,
            Phone = dto.Phone,
            Website = dto.Website
        };

        var accRes = await _http.PostAsJsonAsync(
            $"{instanceUrl}/services/data/v58.0/sobjects/Account",
            account
        );

        var accJson = await accRes.Content.ReadAsStringAsync();

        if (!accRes.IsSuccessStatusCode)
            throw new Exception("ACCOUNT ERROR: " + accJson);

        dynamic accData = JsonConvert.DeserializeObject(accJson);
        string accountId = accData.id;

        var contact = new
        {
            LastName = dto.LastName,
            FirstName = dto.FirstName,
            Email = dto.Email,
            AccountId = accountId
        };

        var contactRes = await _http.PostAsJsonAsync(
            $"{instanceUrl}/services/data/v58.0/sobjects/Contact",
            contact
        );

        var contactJson = await contactRes.Content.ReadAsStringAsync();

        if (!contactRes.IsSuccessStatusCode)
            throw new Exception("CONTACT ERROR: " + contactJson);
    }
}