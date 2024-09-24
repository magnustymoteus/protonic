using System.Linq;

namespace protonic.utils;
using Godot;

static class FileLoader
{
    public static bool HasFile(string path, string fileName)
    {
        using var file = FileAccess.Open(path+"/"+fileName,FileAccess.ModeFlags.Read);
        return file != null;
    }
    
    public static string LoadFromFile(string path)
    {
        using var file = FileAccess.Open(path, FileAccess.ModeFlags.Read);
        if (file == null) return "";
        return file.GetAsText();
    }
    public static string SearchFile(string basePath, string file)
    {
        string[] currentPathArr = basePath.Split('/');

        while (!HasFile(currentPathArr.Join("/"), file) && currentPathArr.Length > 1)
        {
            currentPathArr = currentPathArr.Take(currentPathArr.Length-1).ToArray();
        }

        return LoadFromFile(currentPathArr.Join("/") + "/" + file);
    }
}