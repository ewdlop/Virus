// See https://aka.ms/new-console-template for more information
using ClassLibrary1;
using System.Text;
using System.Windows.Forms;

#if WINDOWS
partial class Program : IDisposable
{
    private bool disposedValue;

    [STAThread]
    private static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;
        _ = Parallel.ForEach(source: EnumerableHelper.Count<int>(0), static _ =>
        {

            Form moreForm = new Form();
            moreForm.Text = $"Windows Handle:{moreForm.Handle}";
            //detach the form from the console
            //https://www.bing.com/?ref=aka&shorturl=winforms-warnings/WFO5002???
            moreForm.ShowDialog();
        });
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

    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    // ~Program()
    // {
    //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
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