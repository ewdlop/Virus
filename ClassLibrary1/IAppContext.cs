namespace ClassLibrary1;

public interface IAppContext
{
    void Run(string requestUrl, params string[] args);
    Task RunAsync(string requestUrl, params string[] args);
    void HostHttpServer(ushort port, params string[] args);
    Task HostHttpServerAsync(ushort port, params string[] args);
    void RunTCPServer(ushort port, params string[] args);
}

public interface IAppContext<T> : IAppContext
{
    T Value { get; }
}