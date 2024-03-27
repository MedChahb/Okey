namespace Okey.AppelsAPI;

using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using AppelsApi.Dtos;
using Dtos;
using Factory;

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
        var url = $"{this._baseUrl}/classement/";
        var response = await this._httpClient.GetAsync(url);
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
        var url = $"{this._baseUrl}/classement/{username}";
        var response = await this._httpClient.GetAsync(url);
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
        var url = $"{this._baseUrl}/classement/{pagination}";
        var response = await this._httpClient.GetAsync(url);
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

    public async Task<object?> GetWatchUsersAsync(string username, IWatchDtoFactory watchDtoFactory)
    {
        var url = $"{this._baseUrl}/compte/watch/{username}";
        this._httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer",
            this._token
        );
        var response = await this._httpClient.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            var rep = await response.Content.ReadAsStringAsync();
            if (this._token == string.Empty)
            {
                return await watchDtoFactory.CreatePublicWatchDtoTask(rep);
            }
            return await watchDtoFactory.CreatePrivateWatchDtoTask(rep);
        }
        throw new Exception(
            $"Erreur d'appel API, [Code]: {response.StatusCode} [Message]: {response.ReasonPhrase}"
        );
    }

    public async Task<LoginDto?> PostLoginAsync(object data)
    {
        var url = $"{this._baseUrl}/compte/login/";
        var jsonData = JsonSerializer.Serialize(data);
        var response = await this._httpClient.PostAsync(
            url,
            new StringContent(jsonData, Encoding.UTF8, "application/json")
        );
        if (response.IsSuccessStatusCode)
        {
            var rep = await response.Content.ReadAsStringAsync();
            var jsonArray = JsonSerializer.Deserialize<LoginDto>(rep);
            if (jsonArray != null)
            {
                this._token = jsonArray.token;
                return jsonArray;
            }
        }
        throw new Exception(
            $"Erreur d'appel API, [Code]: {response.StatusCode} [Message]: {response.ReasonPhrase}"
        );
    }

    public async Task<RegisterDto?> PostRegisterAsync(object data)
    {
        var url = $"{this._baseUrl}/compte/register/";
        var jsonData = JsonSerializer.Serialize(data);
        var response = await this._httpClient.PostAsync(
            url,
            new StringContent(jsonData, Encoding.UTF8, "application/json")
        );
        if (response.IsSuccessStatusCode)
        {
            var rep = await response.Content.ReadAsStringAsync();
            var jsonArray = JsonSerializer.Deserialize<RegisterDto>(rep);
            if (jsonArray != null)
            {
                this._token = jsonArray.token;
                return jsonArray;
            }
        }
        throw new Exception(
            $"Erreur d'appel API, [Code]: {response.StatusCode} [Message]: {response.ReasonPhrase}"
        );
    }
}
