using ClassLibrary1;
using System.Text;

#if WINDOWS
partial class Program
{
    [STAThread]
    private static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;
        _ = Parallel.ForEach(source: EnumerableHelper.Count<int>(2), static _ =>
        {
            DLLImport.ShowMessageBox("Hello World");
        });
    }
}
#else
{
    Console.WriteLine("Please enter Client Secret Json FilePath: ");
    clientSecretJsonFilePath = Console.ReadLine();
}
#endif