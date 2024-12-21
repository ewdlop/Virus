using System.Diagnostics;
using System.Security;
namespace ClassLibrary1;

public static partial class ProcessHelper
{
    public static string GetProcessName() => Process.GetCurrentProcess().ProcessName;
    public static string GetProcessPath() => Process.GetCurrentProcess().MainModule?.FileName ?? string.Empty;
    public static ProcessModule? GetProcessModule() => Process.GetCurrentProcess().MainModule;
    public static ProcessModuleCollection GetProcessModules() => Process.GetCurrentProcess().Modules;
    public static ProcessThreadCollection GetProcessThreads() => Process.GetCurrentProcess().Threads;
    public static ProcessThread GetProcessThread(int index) => Process.GetCurrentProcess().Threads[index];
    public static void KillProcess() => Process.GetCurrentProcess().Kill();
    public static void KillProcess(int processId) => Process.GetProcessById(processId).Kill();
    public static void KillProcess(string processName) => Process.GetProcessesByName(processName).ToList().ForEach(p => p.Kill());
    public static void StartProcess(string fileName) => Process.Start(fileName);
    public static void StartProcess(string fileName, string arguments) => Process.Start(fileName, arguments);
    public static void StartProcess(ProcessStartInfo startInfo) => Process.Start(startInfo);
    public static void StartProcess(
        string fileName,
        string arguments,
        string userName,
        SecureString password,
        string domain) 
    => Process.Start(new ProcessStartInfo
    {
        FileName = fileName,
        Arguments = arguments,
        UserName = userName,
        Password = password,
        Domain = domain
    });

    public static void StartProcess(
        string fileName,
        string arguments,
        string userName,
        SecureString password,
        string domain,
        bool loadUserProfile,
        bool useShellExecute,
        bool redirectStandardInput,
        bool redirectStandardOutput,
        bool redirectStandardError,
        string workingDirectory)
    => Process.Start(new ProcessStartInfo
    {
        FileName = fileName,
        Arguments = arguments,
        UserName = userName,
        Password = password,
        Domain = domain,
        LoadUserProfile = loadUserProfile,
        UseShellExecute = useShellExecute,
        RedirectStandardInput = redirectStandardInput,
        RedirectStandardOutput = redirectStandardOutput,
        RedirectStandardError = redirectStandardError,
        WorkingDirectory = workingDirectory
    });
}