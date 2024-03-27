namespace AppelsApi;

using System.Text.Json;
using Dtos;

public class APICalls
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    private string _token;

    public APICalls()
    {
        this._httpClient = new HttpClient();
        this._baseUrl =
            "https://mai-projet-integrateur.u-strasbg.fr/vmProjetIntegrateurgrp0-0/okeyapi";
        this._token = string.Empty;
    }

    public APICalls(string token)
    {
        this._httpClient = new HttpClient();
        this._baseUrl =
            "https://mai-projet-integrateur.u-strasbg.fr/vmProjetIntegrateurgrp0-0/okeyapi";
        this._token = token;
    }

    public void clearToken() => this._token = string.Empty;

    public async Task<List<PublicWatchDto>?> GetWatchUsersAsync()
    {
        var url = $"{this._baseUrl}/compte/watch/";
        var response = await this._httpClient.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            var rep = await response.Content.ReadAsStringAsync();
            var jsonArray = JsonSerializer.Deserialize<List<PublicWatchDto>>(rep);
            return jsonArray;
        }
        throw new Exception(
            $"Erreur d'appel API, [Code]: {response.StatusCode} [Message]: {response.ReasonPhrase}"
        );
    }

    public async Task<List<ClassementDto>?> GetClassementAsync()
    {
        var url = $"{_baseUrl}/classement/";
        var response = await _httpClient.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            var rep = await response.Content.ReadAsStringAsync();
            var jsonArray = JsonSerializer.Deserialize<List<ClassementDto>>(rep);
            return jsonArray;
        }
        throw new Exception(
            $"Erreur d'appel API, [Code]: {response.StatusCode} [Message]: {response.ReasonPhrase}"
        );
    }

    public async Task<ClassementDto?> GetClassementAsync(string username)
    {
        var url = $"{_baseUrl}/classement/{username}";
        var response = await _httpClient.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            var rep = await response.Content.ReadAsStringAsync();
            var jsonArray = JsonSerializer.Deserialize<ClassementDto>(rep);
            return jsonArray;
        }
        throw new Exception(
            $"Erreur d'appel API, [Code]: {response.StatusCode} [Message]: {response.ReasonPhrase}"
        );
    }

    public async Task<List<ClassementDto>?> GetClassementAsync(int pagination)
    {
        var url = $"{_baseUrl}/classement/{pagination}";
        var response = await _httpClient.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            var rep = await response.Content.ReadAsStringAsync();
            var jsonArray = JsonSerializer.Deserialize<List<ClassementDto>>(rep);
            return jsonArray;
        }
        throw new Exception(
            $"Erreur d'appel API, [Code]: {response.StatusCode} [Message]: {response.ReasonPhrase}"
        );
    }
}
