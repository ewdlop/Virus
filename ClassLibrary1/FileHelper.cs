namespace ClassLibrary1;

public static partial class FileHelper
{
    public static bool Encrypt(string path)
    {
        try
        {
            // Ensure the path exists
            if (!File.Exists(path))
            {
                Console.WriteLine("File does not exist.");
                return false;
            }

            // Encrypt the file
            File.Encrypt(path);
            return true;
        }
        catch (IOException ioEx)
        {
            Console.WriteLine($"IO Exception: {ioEx.Message}");
            return false;
        }
        catch (UnauthorizedAccessException uaEx)
        {
            Console.WriteLine($"Unauthorized Access: {uaEx.Message}");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
            return false;
        }
    }

    public static bool Decrypt(string path)
    {
        try
        {
            // Ensure the path exists
            if (!File.Exists(path))
            {
                Console.WriteLine("File does not exist.");
                return false;
            }
            // Decrypt the file
            File.Decrypt(path);
            return true;
        }
        catch (IOException ioEx)
        {
            Console.WriteLine($"IO Exception: {ioEx.Message}");
            return false;
        }
        catch (UnauthorizedAccessException uaEx)
        {
            Console.WriteLine($"Unauthorized Access: {uaEx.Message}");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
            return false;
        }
    }
}