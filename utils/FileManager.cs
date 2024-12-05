using System.Linq;

namespace protonic.utils;
using Godot;

static class FileManager
{

    public static bool HasFile(string path, string fileName)
    {
        using var file = FileAccess.Open(path+"/"+fileName,FileAccess.ModeFlags.Read);
        return file != null;
    }

    public static FileAccess GetFile(string path)
    {
        var file = FileAccess.Open(path, FileAccess.ModeFlags.Read);
        return file;
    }
    public static string LoadTextFromFile(string path)
    {
        var file = GetFile(path);
        if (file == null) return "";
        return file.GetAsText();
    }

    public static Variant LoadJsonFromFile(FileAccess file)
    {
        string contents = LoadTextFromFile(file);
        Variant json = Json.ParseString(contents);
        return json;
    }
    public static string LoadTextFromFile(FileAccess file)
    {
        if (file == null) return "";
        return file.GetAsText();
    }
    public static FileAccess SearchFile(string basePath, string file)
    {
        string[] currentPathArr = basePath.Split('/');

        while (!HasFile(currentPathArr.Join("/"), file) && currentPathArr.Length > 1)
        {
            currentPathArr = currentPathArr.Take(currentPathArr.Length-1).ToArray();
        }

        return GetFile(currentPathArr.Join("/") + "/" + file);
    }

}