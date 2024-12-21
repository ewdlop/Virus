using System.IO.Enumeration;

namespace ClassLibrary1;

public static partial class UriHelper
{
    public const string GitHubApi = "https://api.github.com/";

   //Linux file system
    public const string bin = "/bin/";
    public const string boot = "/boot/";
    public const string dev = "/dev/";
    public const string etc = "/etc/";
    public const string home = "/home/";
    public const string lib = "/lib/";
    public const string media = "/media/";
    public const string mnt = "/mnt/";
    public const string opt = "/opt/";
    public const string proc = "/proc/";
    public const string root = "/root/";
    public const string run = "/run/";
    public const string sbin = "/sbin/";
    public const string srv = "/srv/";
    public const string sys = "/sys/";
    public const string tmp = "/tmp/";
    public const string usr = "/usr/";

   //Windows file system
    public const string ProgramFiles = "Program Files/";
    public const string ProgramFilesX86 = "Program Files (x86)/";
    public const string ProgramData = "ProgramData/";
    public const string Users = "Users/";
    public const string Windows = "Windows/";
    public const string System32 = "System32/";
    public const string SysWOW64 = "SysWOW64/";
    public const string Temp = "Temp/";
    public const string AppData = "AppData/";
    public const string LocalAppData = "LocalAppData/";
    public const string RoamingAppData = "RoamingAppData/";
    public const string Music = "Music/";
    public const string Videos = "Videos/";
    public const string Pictures = "Pictures/";
    public const string Documents = "Documents/";
    public const string Downloads = "Downloads/";

    public const string CommonAppData = "CommonAppData/";
    public const string CommonProgramFiles = "CommonProgramFiles/";
    public const string CommonProgramFilesX86 = "CommonProgramFiles (x86)/";
    public const string CommonProgramData = "CommonProgramData/";
    public const string CommonStartMenu = "CommonStartMenu/";
    public const string CommonStartup = "CommonStartup/";
    public const string CommonDesktop = "CommonDesktop/";
    public const string CommonDocuments = "CommonDocuments/";
    public const string CommonMusic = "CommonMusic/";

    public static readonly string[] LinuxFileSystem = [bin, boot, dev, etc, home, lib, media, mnt, opt, proc, root, run, sbin, srv, sys, tmp, usr];
    public static readonly string[] WindowsFileSystem = [ProgramFiles, ProgramFilesX86, ProgramData, Users, Windows, System32, SysWOW64, Temp, AppData, LocalAppData, RoamingAppData, Music, Videos, Pictures, Documents, Downloads];
}

public static partial class UriHelper
{
    public const string Http = "http://";
    public const string Https = "https://";
    public const string Ftp = "ftp://";
    public const string Sftp = "sftp://";
    public const string File = "file://";
    public const string MailTo = "mailto:";
    public const string Tel = "tel:";
    public const string Sms = "sms:";
    public const string Gopher = "gopher://";
    public const string Wais = "wais://";
    public const string News = "news:";
    public const string Nntp = "nntp:";
    public const string Prospero = " prospero://";
    public const string Telnet = "telnet://";
    public const string Rlogin = "rlogin://";
    public const string Tn3270 = "tn3270://";
    public const string Mid = "mid:";
    public const string Cid = "cid:";
    public const string Imap = "imap:";
    public const string Pop = "pop:";
    public const string Smtp = "smtp:";
    public const string Bgmp = "bgmp:";
    public const string Ipp = "ipp:";
    public const string Acap = "acap:";
    public const string bolt = "bolt:";
    public const string Url = "url:";
    public const string Dns = "dns:";
    public const string Whois = "whois:";
    public const string HttpProxy = "http://proxy/";
    public const string HttpsProxy = "https://proxy/";
    public const string FtpProxy = "ftp://proxy/";
    public const string localHost = "localhost";

}

public static partial class UriHelper
{
    public static string GetUrl(string path, string? query = null)
    {
        return string.IsNullOrEmpty(query) ? path : $"{path}?{query}";
    }

    public static string GetLinuxFileSystemUrl(string path)
    {
        return GetUrl($"{LinuxFileSystem}{path}");
    }
    public static string GetWindowsFileSystemUrl(string path)
    {
        string windir = Environment.SystemDirectory;// C:\windows\system32
        string? windrive = Path.GetPathRoot(Environment.SystemDirectory);// C:\
        return GetUrl($"{windir}{windrive}", path);
    }
}