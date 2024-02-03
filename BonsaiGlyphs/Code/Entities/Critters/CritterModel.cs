using SadConsole.Entities;

namespace BonsaiGlyphs.Code.Entities.Critters;
public class CritterModel
{
    public CritterModel(string[] frames, Point size, string name)
    {
        Size = size;
        Frames = frames;
        Name = name;
    }

    public string Name { get; set; }

    public string[] Frames { get; set; }

    public Point Size { get; set; }
}

