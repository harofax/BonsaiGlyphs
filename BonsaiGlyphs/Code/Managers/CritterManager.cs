using BonsaiGlyphs.Assets;
using BonsaiGlyphs.Code.Entities.Critters;
using BonsaiGlyphs.Code.Game;
using SadConsole.Entities;
using SadConsole.SerializedTypes;

namespace BonsaiGlyphs.Code.Managers;

public class CritterManager
{
    private LayeredWorld world;

    private List<Entity> critterList = new List<Entity>();

    private ColoredGlyph butterflyTemplate =
        new ColoredGlyph(Color.Pink, Color.Transparent);

    private Dictionary<string, CritterModel> critterNameToData = new Dictionary<string, CritterModel>();

    public CritterManager(LayeredWorld world)
    {
        this.world = world;
        
       var critterData = AssetManager.ParseCritters(Paths.ENTITIES_JSON);

       System.Console.Out.WriteLine("critter len " + critterData.Length);

        foreach (var critter in critterData)
        {
            critterNameToData[critter.Name] = critter;
            AnimatedScreenObject critterAnim = new AnimatedScreenObject(critter.Name, critter.Size.X, critter.Size.Y);
            foreach (var frameString in critter.Frames)
            {
                ColoredGlyphBase[] frame = new ColoredGlyphBase[frameString.Length];

                for (var i = 0; i < frameString.Length; i++)
                {
                    var glyph = frameString[i];
                    frame[i] = new ColoredGlyph(Color.AnsiMagenta, Color.Transparent, glyph);
                }

                critterAnim.Frames.Add(new CellSurface(critter.Size.X, critter.Size.Y, frame));
            }
            
            critterList.Add(new Entity(critterAnim, 1));
        }


        foreach (var critter in critterList)
        {
            world.AddEntityToLayer(LayeredWorld.WorldLayer.Foreground, critter);
            critter.Position = world.Surface.Area.Center.WithX(Random.Shared.Next(GameSettings.WORLD_WIDTH));

            critter.AppearanceSurface.Animation.Repeat = true;
            critter.AppearanceSurface.Animation.AnimationDuration = TimeSpan.FromSeconds(0.3);

            System.Console.Out.WriteLine("frame count " + critter.AppearanceSurface.Animation.Frames.Count);
            
            
            critter.AppearanceSurface.Animation.Start();
            
        }
        
        
    }
}