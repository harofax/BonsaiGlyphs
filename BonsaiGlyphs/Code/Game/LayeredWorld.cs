using BonsaiGlyphs.Code.Components;
using SadConsole.Entities;
using SadConsole.Input;

namespace BonsaiGlyphs.Code.Game;

public class LayeredWorld : ScreenObject
{
    public enum WorldLayer
    {
        Background,
        Bottom,
        Pot,
        Dirt,
        TreeBackground,
        TreeForeground,
        Top,
        Foreground,
    }

    public ScreenSurface Surface { get; private set; }

    private Dictionary<WorldLayer, ScreenSurface> worldMap;
    private Dictionary<WorldLayer, EntityManager> entityManagerMap;

    
    public Rectangle View { get; private set; }
    public Rectangle Area { get; private set; }
    
    public WorldComponent World;

    private int _numLayers;

    public LayeredWorld(int viewWidth, int viewHeight, int worldWidth, int worldHeight)
        //: base(viewWidth, viewHeight, worldWidth, worldHeight)
    {
        var worldLayers = Enum.GetValues(typeof(WorldLayer)).Cast<WorldLayer>().ToArray();
        Surface = new ScreenSurface(viewWidth, viewHeight, worldWidth, worldHeight);

        Area = new Rectangle(0, 0, worldWidth, worldHeight);

        View = new Rectangle(0,0,viewWidth, viewHeight);

        worldMap = new Dictionary<WorldLayer, ScreenSurface>(worldLayers.Length);
        entityManagerMap = new Dictionary<WorldLayer, EntityManager>(worldLayers.Length);

        _numLayers = worldLayers.Length;

        foreach (var worldLayer in worldLayers)
        {
            ScreenSurface worldLayerSurface = new ScreenSurface(viewWidth, viewHeight, worldWidth, worldHeight);
            EntityManager manager = new EntityManager();
            
            worldLayerSurface.SadComponents.Add(manager);
            
            worldMap[worldLayer] = worldLayerSurface;
            entityManagerMap[worldLayer] = manager;
            
            Children.Add(worldLayerSurface);
        }

        World = new WorldComponent();

        SadComponents.Add(World);

        IsFocused = true;
    }

    public override bool ProcessKeyboard(Keyboard keyboard)
    {
        World.ProcessKeyboard(this, keyboard, out var handled);

        return handled;
    }

    public void AddEntityToLayer(WorldLayer layer, Entity entity)
    {
        entityManagerMap[layer].Add(entity);
    }

    public IReadOnlyList<Entity> GetEntities(WorldLayer layer)
    {
        return entityManagerMap[layer].Entities;
    }

    public IReadOnlyList<Entity> GetVisibleEntities(WorldLayer layer)
    {
        return entityManagerMap[layer].EntitiesVisible;
    }

    public ScreenSurface[] GetSurfaces()
    {
        return worldMap.Values.ToArray();
    }

    public void SetViewPosition(Point newViewPosition)
    {
        View = View.WithPosition(newViewPosition);
        for (int i = 0; i < _numLayers; i++)
        {
            worldMap[(WorldLayer) i].ViewPosition = newViewPosition;
        }
    }

    public ScreenSurface GetLayerSurface(WorldLayer layer)
    {
        return worldMap[layer];
    }

    public void SetSurface(WorldLayer layer, ScreenSurface surface)
    {
        worldMap[layer] = surface;
    }

    public void SetSurface(WorldLayer layer, ICellSurface surface)
    {
        worldMap[layer].Surface = surface;
    }
}