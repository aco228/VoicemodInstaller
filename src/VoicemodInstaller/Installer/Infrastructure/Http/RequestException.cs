﻿using System.Net;

namespace Installer.Infrastructure.Http;

public class RequestException : Exception
{
    public Exception Original { get; init; }
    public string Url { get; init; }
    public StringContent Request { get; init; }
    public HttpResponseMessage Response { get; init; }
        
        
    public HttpRequestException HttpRequestException => Original as HttpRequestException;
    public HttpStatusCode? HttpStatusCode => HttpRequestException?.StatusCode;
        
    public RequestException(
        Exception original, 
        string url, 
        StringContent request, 
        HttpResponseMessage responseMessage)
        : base(null, original)
    {
        Original = original;
        Url = url;
        Request = request;
        Response = responseMessage;
    }

    public override string Message
    {
        get
        {
            var msg = 
                Environment.NewLine + Environment.NewLine + Environment.NewLine
                + $"URL = {Url}"
                + Environment.NewLine + Environment.NewLine + Environment.NewLine
                + $"REQUEST = {Request?.ReadAsStringAsync().Result}"
                + Environment.NewLine + Environment.NewLine + Environment.NewLine
                + $"RESPONSE = {Response?.Content.ReadAsStringAsync().Result}"
                + Environment.NewLine + Environment.NewLine + Environment.NewLine;
            return msg;
        }
    }
}