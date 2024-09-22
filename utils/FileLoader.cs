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
}