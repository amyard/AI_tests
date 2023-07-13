namespace llamaAI.Helpers;

public static class PathHelper
{
    public static string GetPath(string folderName, string fileName)
    {
        string currentDirectory = Directory.GetCurrentDirectory();
        var l1 = Directory.GetParent(currentDirectory);
        var l2 = Directory.GetParent(l1.FullName);
        var l3 = Directory.GetParent(l2.FullName);
        
        return Path.Combine(l3.FullName, folderName, fileName);
    }
}