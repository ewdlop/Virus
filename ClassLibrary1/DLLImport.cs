﻿using System.Runtime.InteropServices;

namespace ClassLibrary1;


public static partial class DLLImport
{
#if WINDOWS
    public static string ShowOpenFileDialog()
    {
        using OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "All files (*.*)|*.*|Text files (*.txt)|*.txt";
        openFileDialog.FilterIndex = 1;
        openFileDialog.RestoreDirectory = true;

        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            return openFileDialog.FileName;
        }

        return string.Empty;
    }


    // One or more errors occurred. (Unable to find an entry point named 'MessageBox' in DLL 'user32.dll'.) (Unable to find an entry point named 'MessageBox' in DLL 'user32.dll'.)
    //[LibraryImport("user32.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
    //public static partial int MessageBox(IntPtr hWnd, String text, String caption, uint type);

    [LibraryImport("user32.dll", EntryPoint = "MessageBoxW", StringMarshalling = StringMarshalling.Utf16)]
    private static partial int MessageBox(IntPtr hWnd, string text, string caption, uint type);

    //Compiler Error CS1057
    //https://learn.microsoft.com/en-us/dotnet/csharp/misc/cs1057
    //This error is generated by declaring a protected member inside a static class.
    //protected static partial int MessageBox(IntPtr hWnd, String text, String caption, uint type);
    public static void ShowMessageBox(string message) => MessageBox(IntPtr.Zero, message, "Message", 0);


#else
    public void Method1()
    {
        Console.WriteLine("Please enter Client Secret Json FilePath: ");
        clientSecretJsonFilePath = Console.ReadLine();
    }
}
#endif

    #region references: stackoverflow.com/a/3571627/9931159
    //https://stackoverflow.com/questions/3571627/show-hide-the-console-window-of-a-c-sharp-console-application
    [LibraryImport("kernel32.dll")]
    private static partial IntPtr GetConsoleWindow();

    [LibraryImport("user32.dll")]
    [return:MarshalAs(UnmanagedType.Bool)]
    private static partial bool ShowWindow(IntPtr hWnd, int nCmdShow);

    public static void ShowConsoleWindow()
    {
        var handle = GetConsoleWindow();
        ShowWindow(handle, SW_SHOW);
    }

    public static void HideConsoleWindow()
    {
        var handle = GetConsoleWindow();
        ShowWindow(handle, SW_HIDE);
    }


    /**
     * https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-showwindow
     * SW_HIDE
     * 0 Hides the window and activates another window.
     * SW_SHOWNORMAL
     * SW_NORMAL
     * 1	Activates and displays a window. If the window is minimized, maximized, or arranged, the system restores it to its original size and position. An application should specify this flag when displaying the window for the first time.
     * SW_SHOWMINIMIZED
     * 2	Activates the window and displays it as a minimized window.
     * SW_SHOWMAXIMIZED
     * SW_MAXIMIZE
     * 3	Activates the window and displays it as a maximized window.
     * SW_SHOWNOACTIVATE
     * 4	Displays a window in its most recent size and position. This value is similar to SW_SHOWNORMAL, except that the window is not activated.
     * SW_SHOW
     * 5	Activates the window and displays it in its current size and position.
     * SW_MINIMIZE
     * 6	Minimizes the specified window and activates the next top-level window in the Z order.
     * SW_SHOWMINNOACTIVE
     * 7	Displays the window as a minimized window. This value is similar to SW_SHOWMINIMIZED, except the window is not activated.
     * SW_SHOWNA
     * 8	Displays the window in its current size and position. This value is similar to SW_SHOW, except that the window is not activated.
     * SW_RESTORE
     * 9	Activates and displays the window. If the window is minimized, maximized, or arranged, the system restores it to its original size and position. An application should specify this flag when restoring a minimized window.
     * SW_SHOWDEFAULT
     * 10	Sets the show state based on the SW_ value specified in the STARTUPINFO structure passed to the CreateProcess function by the program that started the application.
     * SW_FORCEMINIMIZE
     * 11
    **/

    public const int SW_HIDE = 0;
    public const int SW_SHOWNORMAL = 1;
    public const int SW_SHOWMINIMIZED = 2;
    public const int SW_SHOWMAXIMIZED = 3;
    public const int SW_SHOWNOACTIVATE = 4;
    public const int SW_SHOW = 5;
    public const int SW_MINIMIZE = 6;
    public const int SW_SHOWMINNOACTIVE = 7;
    public const int SW_SHOWNA = 8;
    public const int SW_RESTORE = 9;
    public const int SW_SHOWDEFAULT = 10;
    public const int SW_FORCEMINIMIZE = 11;

    #endregion
}
