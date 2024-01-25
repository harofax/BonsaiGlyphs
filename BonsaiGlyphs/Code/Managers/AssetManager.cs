using System.IO.Compression;
using SadConsole.Readers;

namespace BonsaiGlyphs.Code.Managers;

public static class AssetManager
{
    public static REXPaintImage LoadRexFile(string path)
    {
        using FileStream rexStream = new FileStream(path, FileMode.Open);
        var rexFile = REXPaintImage.Load(rexStream);

        return rexFile;
    }
}