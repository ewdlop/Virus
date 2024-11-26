// See https://aka.ms/new-console-template for more information
using ClassLibrary1;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;
using Polly.Registry;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;

#if WINDOWS
partial class Program : IDisposable
{
    private bool disposedValue;
    public static IServiceProvider? ServiceProvider { get; protected set; }

    [STAThread]
    private static void Main(string[] args)
    {

        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

        #region https://github.dev/RehanSaeed/HttpClientSample

        IPolicyRegistry<string> register = builder.Services.AddPolicyRegistry();


        register.Add("timeout", Policy.Timeout(5));
        register.Add("retry", HttpPolicyExtensions.HandleTransientHttpError()
            .WaitAndRetryAsync(Backoff.ExponentialBackoff(TimeSpan.FromMilliseconds(1000), retryCount: 5, factor: 2)));
        register.Add("circuitBreaker", HttpPolicyExtensions.HandleTransientHttpError()
            .AdvancedCircuitBreakerAsync(failureThreshold: 0.5,
                samplingDuration: TimeSpan.FromSeconds(5),
                minimumThroughput: 100,
                durationOfBreak: TimeSpan.FromSeconds(20),
                onBreak: (ex, state, ts, context) => { },
                onReset: context => { },
                onHalfOpen: () => { }));

        #endregion

        builder.Services.AddHttpClient("github", static c =>
        {
            c.BaseAddress = new Uri("https://api.github.com/");
            c.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
            c.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory-Sample");
            c.DefaultRequestVersion = new Version(2, 0);
            c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }).AddPolicyHandler((provider, message) =>
        {
           //https://github.dev/RehanSaeed/HttpClientSample
            IConcurrentPolicyRegistry<string>? registry = provider.GetRequiredService<IConcurrentPolicyRegistry<string>>();//这里需要Polly 7string?string?string? 
            string key = $"circuitBreaker_{message.RequestUri?.AbsolutePath}";
            return registry.GetOrAdd(key, Policy.HandleResult<HttpResponseMessage>(response =>
            {
                return response.StatusCode == HttpStatusCode.RequestTimeout;
            })//这里应该捕获非网络相关的异常，而不是全局异常，如果是全局网络异常，则断路器应该针对所有请求
             .AdvancedCircuitBreakerAsync(failureThreshold: 0.5,
             samplingDuration: TimeSpan.FromSeconds(5),
             minimumThroughput: 100,
             durationOfBreak: TimeSpan.FromSeconds(20),
             onBreak: (ex, state, ts, context) => { },
             onReset: context => { },
             onHalfOpen: () => { }));
        });//.ConfigurePrimaryHttpMessageHandler(() =>
       //{
       //    return new HttpClientHandler()
       //    {
       //        Proxy = new WebProxy(new Uri("Proxy"), BypassOnLocal: true),
       //        UseProxy = true,
       //    };
       //});s
        builder.Services.AddScoped<IAppContext, ConsoleApp1.AppContext>();
        
        ServiceProvider = builder.Build().Services;
        using IServiceScope scope = ServiceProvider.CreateScope();


        _ = Task.Run(() => scope.ServiceProvider.GetRequiredService<IAppContext>().Run(UriHelper.bin, args));
        scope.ServiceProvider.GetRequiredService<IAppContext>().HostHttpServer(8080, args);
    }


    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
               // TODO: dispose managed state (managed objects)
            }

           // TODO: free unmanaged resources (unmanaged objects) and override finalizer
           // TODO: set large fields to null
            disposedValue = true;
        }
    }

   //// TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
   // ~Program()
   // {
   //    // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
   //     Dispose(disposing: false);
   // }

    public void Dispose()
    {
       // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
#else
{
    Console.WriteLine("Please enter Client Secret Json FilePath: ");
    clientSecretJsonFilePath = Console.ReadLine();
}
#endif