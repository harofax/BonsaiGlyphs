using System.IO.Compression;
using System.Text.Json;
using System.Text.Json.Nodes;
using BonsaiGlyphs.Code.Entities.Critters;
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

    public static CritterModel[] ParseCritters(string path)
    {
        string jsonData = File.ReadAllText(path);

        CritterModel[] critterData = SadConsole.Serializer.Deserialize<CritterModel[]>(jsonData);
        
        return critterData;
    }

    public static void TestSerializeCritters()
    {
        ColoredGlyph butterflyLeft = new ColoredGlyph(Color.Pink, Color.Transparent, '}');
        ColoredGlyph butterflyMiddle = new ColoredGlyph(Color.Pink, Color.Transparent, 'i');
        ColoredGlyph butterflyRight = new ColoredGlyph(Color.Pink, Color.Transparent, '{');

        string[] butterflyFrames = new[]
        {
            "}i{",
            "[i]",
            "|i|"
        };
        
        string[] flyFrames = new[]
        {
            "-.-",
            "\".\"",
            "\\./"
        };
        
        CritterModel butterfly = new CritterModel(butterflyFrames, (3,1), "butterfly");
        CritterModel fly = new CritterModel(flyFrames, (3, 1), "fly");

        CritterModel[] arrayCritters = new[] {butterfly, fly};

        var json = SadConsole.Serializer.Serialize(arrayCritters);

        System.Console.Out.WriteLine(json);
    }
}