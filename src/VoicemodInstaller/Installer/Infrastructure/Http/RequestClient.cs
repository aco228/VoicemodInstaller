﻿using System.Net.Http.Headers;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Installer.Infrastructure.Http;

public class RequestClient : IRequestClient, IDisposable
{
    protected string BaseUrl { get; private set; }
    protected HttpClient _client = new();

    public RequestClient() { }
    public RequestClient(string baseUrl)
    {
        BaseUrl = baseUrl;
    }

    public void Dispose()
    {
        _client?.Dispose();
    }

    protected void SetBaseString(string baseString)
    {
        BaseUrl = baseString;
    }

    protected virtual string GetUrl(string url)
        => string.IsNullOrEmpty(BaseUrl) ? url :
            url.StartsWith(BaseUrl) ? url : $"{BaseUrl}{url}";

    protected virtual (string, StringContent) GetRequestData(string url, object data)
        => (GetUrl(url), GetContent(data));

    protected virtual void OnResponseReceived(HttpResponseMessage responseMessage)
    {
    }

    protected virtual Task OnBeforeRequest(string url, object? data = null)
        => Task.FromResult(true);

    protected virtual StringContent OnAddingHeaders(StringContent content)
    {
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        // content.Headers.ContentType.Parameters.Add(new NameValueHeaderValue("charset", "utf-8"));
        return content;
    }

    public Task<HttpResponseMessage> GetResponse(string url)
        => _client.GetAsync(url);

    private StringContent GetContent(object obj)
    {
        var data = string.Empty;

        if (obj != null)
        {
            if (obj is string)
                data = obj as string;
            else
            {
                data = JsonConvert.SerializeObject(obj);
                if (data == null)
                    throw new ArgumentException("Object could not be translated to dictionary");
            }
        }

        var content = OnAddingHeaders(new StringContent(data, Encoding.UTF8));
        return content;
    }


    #region # GET #

    public Task<string> Get(string url)
        => Get<string>(url, null);

    public async Task<T> Get<T>(string url, object obj)
    {
        var response = await Get(GetUrl(url), obj);
        //System.IO.File.WriteAllText(@"C:\Users\aleks\OneDrive\Desktop\api\last.json", response);
        return JsonConvert.DeserializeObject<T>(response);
    }

    public async Task<T> Get<T>(string url)
    {
        var response = await Get(GetUrl(url));
        //System.IO.File.WriteAllText(@"C:\Users\aleks\OneDrive\Desktop\api\last.json", response);
        return JsonConvert.DeserializeObject<T>(response);
    }

    public async Task<string> Get<T>(string url, T obj)
    {
        await OnBeforeRequest(url, obj);

        var query = string.Empty;
        if (obj != null)
        {
            var data = JObject.FromObject(obj).ToObject<Dictionary<string, string>>();
            query = ParamsToStringAsync(data);
        }

        if (!string.IsNullOrEmpty(query))
            query = (url.Contains("?") ? "&" : "?") + query;

        var response = await _client.GetAsync(GetUrl(url) + query);
        OnResponseReceived(response);
        EnsureSuccessStatusCode(response, GetUrl(url) + query, null);
        return await response.Content.ReadAsStringAsync();
    }

    #endregion

    #region # POST

    public async Task<string> Post<T>(string url, T obj)
    {
        await OnBeforeRequest(url, obj);

        (string requestUrl, StringContent requestContent) = GetRequestData(url, obj);
        var response = await _client.PostAsync(requestUrl, requestContent);
        OnResponseReceived(response);
        EnsureSuccessStatusCode(response, requestUrl, requestContent);
        var stringResponse = await response.Content.ReadAsStringAsync();
        return stringResponse;
    }

    public async Task<T> Post<T>(string url, object obj)
    {
        var response = await Post(url, obj);
        return JsonConvert.DeserializeObject<T>(response);
    }

    #endregion

    #region # PATCH

    public async Task<string> Patch<T>(string url, T obj)
    {
        await OnBeforeRequest(url, obj);
        (string requestUrl, StringContent requestContent) = GetRequestData(url, obj);
        var response = await _client.PatchAsync(requestUrl, requestContent);
        OnResponseReceived(response);
        EnsureSuccessStatusCode(response, requestUrl, requestContent);
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<T> Patch<T>(string url, object obj)
    {
        var response = await Patch(url, obj);
        return JsonConvert.DeserializeObject<T>(response);
    }

    #endregion

    #region # PUT

    public async Task<string> Put<T>(string url, T obj)
    {
        await OnBeforeRequest(url, obj);
        (string requestUrl, StringContent requestContent) = GetRequestData(url, obj);
        var response = await _client.PutAsync(requestUrl, requestContent);
        OnResponseReceived(response);
        EnsureSuccessStatusCode(response, requestUrl, requestContent);
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<T> Put<T>(string url, object obj)
    {
        var response = await Put(url, obj);
        return JsonConvert.DeserializeObject<T>(response);
    }

    #endregion

    #region # DELETE

    public async Task Delete(string url)
    {
        await OnBeforeRequest(url, null);
        var response = await _client.DeleteAsync(GetUrl(url));
        EnsureSuccessStatusCode(response, GetUrl(url), null);
    }

    #endregion

    public void AddAuthorization(string value)
        => _client.DefaultRequestHeaders.Authorization = new("Bearer", value);

    public void AddAuthorization(AuthenticationHeaderValue value)
        => _client.DefaultRequestHeaders.Authorization = value;

    public void AddDefaultHeader(string key, string value)
        => _client.DefaultRequestHeaders.Add(key, value);

    public string ParamsToStringAsync<T>(T obj)
    {
        var data = JObject.FromObject(obj).ToObject<Dictionary<string, string>>();
        return ParamsToStringAsync(data);
    }

    public string ParamsToStringAsync(Dictionary<string, string> urlParams)
    {
        if (urlParams == null || urlParams.Count == 0)
            return string.Empty;

        var query = new List<string>();
        foreach (var param in urlParams)
            if (!string.IsNullOrEmpty(param.Value))
                query.Add($"{param.Key}={HttpUtility.HtmlEncode(param.Value)}");

        return String.Join("&", query);
    }

    private void EnsureSuccessStatusCode(HttpResponseMessage response, string url, StringContent request)
    {
        try
        {
            response.EnsureSuccessStatusCode();
        }
        catch (Exception exception)
        {
            throw new RequestException(exception, url, request, response);
        }
    }
}