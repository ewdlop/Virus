// See https://aka.ms/new-console-template for more information
using ClassLibrary1;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace ConsoleApp1;

public class AppContext(IHttpClientFactory httpClientFactory) : IAppContext
{
    protected string clientName = "github";
    protected readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    protected static readonly Lazy<Version> _version = new Lazy<Version>(() => new Version(2, 0));

    protected static string RandomContext<T>(T serailzable) =>
        Random.Shared.NextDouble() switch
        {
            < 0.25 => "A",
            < 0.5 => Guid.NewGuid().ToString(),
            < 0.75 => string.Empty,
            _ => JsonSerializer.Serialize(serailzable)
        };

    public void Run(string requestUrl, params string[] args) => Parallel.ForEach(source: EnumerableHelper.Count<int>(0), (__) =>
    {
        try
        {
            HttpClient client = _httpClientFactory.CreateClient(clientName);

           //string random = Guid.NewGuid().ToString();

            HttpRequestMessage randomRequest = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(requestUrl),
                Headers =
                {
                    Accept = { new MediaTypeWithQualityHeaderValue("application/json") },
                    UserAgent = { new ProductInfoHeaderValue("HttpClientFactory-Sample", "1.0") }
                },
                Version = _version.Value,
            };
            randomRequest.Content = new StringContent(RandomContext(randomRequest), Encoding.UTF8, MediaTypeNames.Application.Json);

            HttpResponseMessage response = client.Send(randomRequest);
            if (response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(content);
            }
        }
        catch (Exception)
        {
           //ignore
        }
    });


    public Task RunAsync(string requestUrl, params string[] args) => Parallel.ForEachAsync(source: EnumerableHelper.Count<int>(0), async (__, token) =>
    {
        try
        {
            HttpClient client = _httpClientFactory.CreateClient(clientName);

           //string random = Guid.NewGuid().ToString();

            HttpRequestMessage randomRequest = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(requestUrl),
                Headers =
                {
                    Accept = { new MediaTypeWithQualityHeaderValue("application/json") },
                    UserAgent = { new ProductInfoHeaderValue("HttpClientFactory-Sample", "1.0") }
                },
                Version = _version.Value,
            };
            randomRequest.Content = new StringContent(RandomContext(randomRequest), Encoding.UTF8, MediaTypeNames.Application.Json);

            HttpResponseMessage response = await client.SendAsync(randomRequest, token);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync(token);
                Console.WriteLine(content);
            }
        }
        catch (Exception)
        {
           //ignore
        }
    });

    public void HostHttpServer(ushort port, params string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.WebHost.UseKestrel().UseUrls($"http://localhost:{port}");

        WebApplication app = builder.Build();

        app.MapGet("/", () => "Hello World!");
        app.MapGet("/json", () => new { Hello = "World" });

        for (int i = 0; i < 10; i++)
        {
            app.MapGet($"/{i}", () => $"Hello {i}!");
        }

        app.Run();
    }


    public Task HostHttpServerAsync(ushort port, params string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        builder.WebHost.UseKestrel().UseUrls($"http://localhost:{port}");

        WebApplication app = builder.Build();

        app.MapGet("/", () => "Hello World!");
        app.MapGet("/json", () => new { Hello = "World" });

        for (int i = 0; i < 10; i++)
        {
            app.MapGet($"/{i}", () => $"Hello {i}!");
        }

        return app.RunAsync();
    }


    public void RunTCPServer(ushort port, params string[] args)
    {
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Loopback, port);
        using Socket socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        socket.Bind(endPoint);
        try
        {
            socket.Listen(10);
            socket.BeginAccept(async ar =>
            {
                Socket client = socket.EndAccept(ar);
                byte[] buffer = new byte[1024];
                int bytesRead = await client.ReceiveAsync(buffer, SocketFlags.None);
                await client.SendAsync(buffer, SocketFlags.None);
                client.Shutdown(SocketShutdown.Both);
                client.Close();
            }, null);
        }
        catch (SocketException)
        {
            socket.Dispose();
        }
    }
}
