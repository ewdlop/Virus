namespace ClassLibrary1;

public static partial class EnvironmentHelper
{
    public static string GetCommandLine() => Environment.CommandLine;
    public static string[] GetCommandLineArgs() => Environment.GetCommandLineArgs();
    public static string? GetEnvironmentVariable(string variable) => Environment.GetEnvironmentVariable(variable);
    public static string GetFileSystemUrl(string path)
    {
        return Environment.OSVersion.Platform == PlatformID.Win32NT ? UriHelper.GetWindowsFileSystemUrl(path) : UriHelper.GetLinuxFileSystemUrl(path);
    }
    public static string[] GetLogicalDrives() => Environment.GetLogicalDrives();
    public static string GetMachineName() => Environment.MachineName;
    public static string GetNewLine() => Environment.NewLine;
    public static Environment.ProcessCpuUsage GetTotalTime() => new Environment.ProcessCpuUsage();
    public static int GetProcessId() => Environment.ProcessId;
    public static string GetSystemDirectory() => Environment.SystemDirectory;
    public static string GetSystemRoot() => Environment.SystemDirectory;
    public static int GetSystemPageSize() => Environment.SystemPageSize;
    public static int GetTickCount() => Environment.TickCount;
    public static string GetUserName() => Environment.UserName;
    public static bool GetIsPrivilegedProcess() => Environment.IsPrivilegedProcess;
    /// <summary>
    /// Summary:
    ///     The logical Desktop rather than the physical file system location.
    ///Desktop = 0,
    ///
    /// Summary:
    ///     The directory that contains the user's program groups.
    ///Programs = 2,
    ///
    /// Summary:
    ///     The My Documents folder. This member is equivalent to System.Environment.SpecialFolder.Personal.
    ///MyDocuments = 5,
    ///
    /// Summary:
    ///     The directory that serves as a common repository for documents. This member is
    ///     equivalent to System.Environment.SpecialFolder.MyDocuments.
    ///Personal = 5,
    ///
    /// Summary:
    ///     The directory that serves as a common repository for the user's favorite items.
    ///Favorites = 6,
    ///
    /// Summary:
    ///     The directory that corresponds to the user's Startup program group. The system
    ///     starts these programs whenever a user logs on or starts Windows.
    ///Startup = 7,
    ///
    /// Summary:
    ///     The directory that contains the user's most recently used documents.
    ///Recent = 8,
    ///
    /// Summary:
    ///     The directory that contains the Send To menu items.
    ///SendTo = 9,
    ///
    /// Summary:
    ///     The directory that contains the Start menu items.
    ///StartMenu = 11,
    ///
    /// Summary:
    ///     The My Music folder.
    ///MyMusic = 13,
    ///
    /// Summary:
    ///     The file system directory that serves as a repository for videos that belong
    ///     to a user.
    ///MyVideos = 14,
    ///
    /// Summary:
    ///     The directory used to physically store file objects on the desktop. Do not confuse
    ///     this directory with the desktop folder itself, which is a virtual folder.
    ///DesktopDirectory = 16,
    ///
    /// Summary:
    ///     The My Computer folder. When passed to the Environment.GetFolderPath method,
    ///     the MyComputer enumeration member always yields the empty string ("") because
    ///     no path is defined for the My Computer folder.
    ///MyComputer = 17,
    ///
    /// Summary:
    ///     A file system directory that contains the link objects that may exist in the
    ///     My Network Places virtual folder.
    ///NetworkShortcuts = 19,
    ///
    /// Summary:
    ///     A virtual folder that contains fonts.
    ///Fonts = 20,
    ///
    /// Summary:
    ///     The directory that serves as a common repository for document templates.
    ///Templates = 21,
    ///
    /// Summary:
    ///     The file system directory that contains the programs and folders that appear
    ///     on the Start menu for all users.
    ///CommonStartMenu = 22,
    ///
    /// Summary:
    ///     A folder for components that are shared across applications.
    ///CommonPrograms = 23,
    ///
    /// Summary:
    ///     The file system directory that contains the programs that appear in the Startup
    ///     folder for all users.
    ///CommonStartup = 24,
    ///
    /// Summary:
    ///     The file system directory that contains files and folders that appear on the
    ///     desktop for all users.
    ///CommonDesktopDirectory = 25,
    ///
    /// Summary:
    ///     The directory that serves as a common repository for application-specific data
    ///     for the current roaming user. A roaming user works on more than one computer
    ///     on a network. A roaming user's profile is kept on a server on the network and
    ///     is loaded onto a system when the user logs on.
    ///ApplicationData = 26,
    ///
    /// Summary:
    ///     The file system directory that contains the link objects that can exist in the
    ///     Printers virtual folder.
    ///PrinterShortcuts = 27,
    ///
    /// Summary:
    ///     The directory that serves as a common repository for application-specific data
    ///     that is used by the current, non-roaming user.
    ///LocalApplicationData = 28,
    ///
    /// Summary:
    ///     The directory that serves as a common repository for temporary Internet files.
    ///InternetCache = 32,
    ///
    /// Summary:
    ///     The directory that serves as a common repository for Internet cookies.
    ///Cookies = 33,
    ///
    /// Summary:
    ///     The directory that serves as a common repository for Internet history items.
    ///History = 34,
    ///
    /// Summary:
    ///     The directory that serves as a common repository for application-specific data
    ///     that is used by all users.
    ///CommonApplicationData = 35,
    ///
    /// Summary:
    ///     The Windows directory or SYSROOT. This corresponds to the %windir% or %SYSTEMROOT%
    ///     environment variables.
    ///Windows = 36,
    ///
    /// Summary:
    ///     The System directory.
    ///System = 37,
    ///
    /// Summary:
    ///     The program files directory. In a non-x86 process, passing System.Environment.SpecialFolder.ProgramFiles
    ///     to the System.Environment.GetFolderPath(System.Environment.SpecialFolder) method
    ///     returns the path for non-x86 programs. To get the x86 program files directory
    ///     in a non-x86 process, use the System.Environment.SpecialFolder.ProgramFilesX86
    ///     member.
    ///ProgramFiles = 38,
    ///
    /// Summary:
    ///     The My Pictures folder.
    ///MyPictures = 39,
    ///
    /// Summary:
    ///     The user's profile folder. Applications should not create files or folders at
    ///     this level; they should put their data under the locations referred to by System.Environment.SpecialFolder.ApplicationData.
    ///UserProfile = 40,
    ///
    /// Summary:
    ///     The Windows System folder.
    ///SystemX86 = 41,
    ///
    /// Summary:
    ///     The x86 Program Files folder.
    ///ProgramFilesX86 = 42,
    ///
    /// Summary:
    ///     The directory for components that are shared across applications. To get the
    ///     x86 common program files directory in a non-x86 process, use the System.Environment.SpecialFolder.ProgramFilesX86
    ///     member.
    ///CommonProgramFiles = 43,
    ///
    /// Summary:
    ///     The Program Files folder.
    ///CommonProgramFilesX86 = 44,
    ///
    /// Summary:
    ///     The file system directory that contains the templates that are available to all
    ///     users.
    ///CommonTemplates = 45,
    ///
    /// Summary:
    ///     The file system directory that contains documents that are common to all users.
    ///CommonDocuments = 46,
    ///
    /// Summary:
    ///     The file system directory that contains administrative tools for all users of
    ///     the computer.
    ///CommonAdminTools = 47,
    ///
    /// Summary:
    ///     The file system directory that is used to store administrative tools for an individual
    ///     user. The Microsoft Management Console (MMC) will save customized consoles to
    ///     this directory, and it will roam with the user.
    ///AdminTools = 48,
    ///
    /// Summary:
    ///     The file system directory that serves as a repository for music files common
    ///     to all users.
    ///CommonMusic = 53,
    ///
    /// Summary:
    ///     The file system directory that serves as a repository for image files common
    ///     to all users.
    ///CommonPictures = 54,
    ///
    /// Summary:
    ///     The file system directory that serves as a repository for video files common
    ///     to all users.
    ///CommonVideos = 55,
    ///
    /// Summary:
    ///     The file system directory that contains resource data.
    ///Resources = 56,
    ///
    /// Summary:
    ///     The file system directory that contains localized resource data.
    ///LocalizedResources = 57,
    ///
    /// Summary:
    ///     This value is recognized in Windows Vista for backward compatibility, but the
    ///     special folder itself is no longer used.
    ///CommonOemLinks = 58,
    ///
    /// Summary:
    ///     The file system directory that acts as a staging area for files waiting to be
    ///     written to a CD.
    ///CDBurning = 59
    /// </summary>
    /// <param name="folder"></param>
    /// <returns></returns>
    public static string GetFolderPath(Environment.SpecialFolder folder) => Environment.GetFolderPath(folder);
}
