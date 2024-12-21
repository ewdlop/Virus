namespace ClassLibrary1;

public static partial class PathHelper
{
    public static string GetCombinePath(string path1, string path2) => Path.Combine(path1, path2);
}
