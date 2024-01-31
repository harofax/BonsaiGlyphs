using BonsaiGlyphs.Code.Components;
using BonsaiGlyphs.Code.Util;
using SadConsole.Entities;
using SadConsole.Input;

namespace BonsaiGlyphs.Code.Game;

public class LayeredWorld : ScreenSurface
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

    //public ScreenSurface Surface { get; private set; }

    private Dictionary<WorldLayer, ScreenSurface> worldMap;
    private Dictionary<WorldLayer, EntityManager> entityManagerMap;

    private Random rng;

    public WorldViewPortMovementHandler Viewport;

    public int NumLayers { get; }

    public LayeredWorld(int viewWidth, int viewHeight, int worldWidth, int worldHeight)
        : base(viewWidth, viewHeight, worldWidth, worldHeight)
    {
        var worldLayers = Enum.GetValues(typeof(WorldLayer)).Cast<WorldLayer>().ToArray();
        //Surface = new ScreenSurface(viewWidth, viewHeight, worldWidth, worldHeight);

        Surface.IsDirtyChanged += SurfaceOnIsDirtyChanged;

        Surface.Fill(Color.Red, Color.Blue, '?');

        worldMap = new Dictionary<WorldLayer, ScreenSurface>(worldLayers.Length);
        entityManagerMap = new Dictionary<WorldLayer, EntityManager>(worldLayers.Length);

        rng = new Random(Guid.NewGuid().GetHashCode());

        NumLayers = worldLayers.Length;

        foreach (var worldLayer in worldLayers)
        {
            ScreenSurface worldLayerSurface = new ScreenSurface(viewWidth, viewHeight, worldWidth, worldHeight);
            EntityManager manager = new EntityManager();

            worldLayerSurface.UsePixelPositioning = true;

            worldLayerSurface.SadComponents.Add(manager);

            worldMap[worldLayer] = worldLayerSurface;
            entityManagerMap[worldLayer] = manager;

            worldLayerSurface.FocusOnMouseClick = false;
            worldLayerSurface.IsFocused = false;

            worldLayerSurface.UseMouse = false;

            Children.Add(worldLayerSurface);
        }

        var BG = worldMap[WorldLayer.Background];

        AddSky(BG);
        AddStars(worldMap[WorldLayer.Bottom]);

        IsExclusiveMouse = true;

        Viewport = new WorldViewPortMovementHandler();

        SadComponents.Add(Viewport);

        IsFocused = true;
        UseMouse = true;
    }

    private void AddStars(ScreenSurface surface)
    {
        int[] glyphs = new[] {'+', 46, 250, 96, 248, 249};

        int glyph = 0;

        for (int x = 0; x < surface.Width; x++)
        {
            for (int y = 0; y < 190; y++)
            {
                if (rng.Next(130 + (y + 1) * 10) < 5)
                {
                    if (rng.Next(200) < 10 && y < 80)
                    {
                        glyph = 197;
                    }
                    else
                    {
                        glyph = glyphs[rng.Next(0, glyphs.Length)];
                    }
                }
                else
                {
                    glyph = 0;
                }


                surface.Surface.SetCellAppearance(x, y,
                    new ColoredGlyph(Color.LightGoldenrodYellow, Color.Transparent, glyph));
            }
        }
    }

    private void AddSky(ScreenSurface screen)
    {
        Color[] colors = new[]
        {
            Color.Black,
            Color.DarkBlue,
            Color.CornflowerBlue,
            Color.Purple,
            Color.OrangeRed,
            Color.Goldenrod,
        };
        float[] colorStops = new[] {0f, 0.3f, 0.5f, 0.85f, 0.90f, 1f};
        Algorithms.GradientFill(screen.FontSize,
            screen.Surface.Area.Center,
            screen.Height,
            0,
            screen.Surface.Area,
            new Gradient(colors, colorStops),
            (x, y, color) => screen.Surface[x, y].Background = color);
    }


    private void SurfaceOnIsDirtyChanged(object? sender, EventArgs e)
    {
        SetLayersViewPositions(ViewPosition);
    }

    public override bool ProcessKeyboard(Keyboard keyboard)
    {
        Viewport.ProcessKeyboard(this, keyboard, out var handled);

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

    public void SetLayersToDirty(bool isDirty)
    {
        for (int i = 0; i < NumLayers; i++)
        {
            worldMap[(WorldLayer) i].IsDirty = isDirty;
        }
    }

    public void SetLayersViewPositions(Point newViewPosition)
    {
        for (int i = 0; i < NumLayers; i++)
        {
            worldMap[(WorldLayer) i].ViewPosition = newViewPosition;
        }
    }

    public void SetLayersView(Rectangle newView)
    {
        Surface.View = newView;
        for (int i = 0; i < NumLayers; i++)
        {
            worldMap[(WorldLayer) i].Surface.View = newView;
        }
    }

    public void SetLayerPositions(Point position)
    {
        for (int i = 0; i < NumLayers; i++)
        {
            worldMap[(WorldLayer) i].Position = position;
        }
    }


    public bool CastRayAt(Point rayPos, bool skipBG, out RayHitResult hitResult)
    {
        int offset = skipBG ? 1 : 0;
        for (int i = NumLayers - 1; i >= offset; i--)
        {
            ScreenSurface layer = worldMap[(WorldLayer) i];

            var cell = layer.Surface[rayPos.X, rayPos.Y];
            bool empty = cell.Background == Color.Transparent && cell.Background == Color.Transparent;

            if (!empty)
            {
                System.Console.Out.WriteLine("HIT A CELL");
                hitResult = new RayHitResult(true, cell, rayPos);
                return true;
            }
        }

        hitResult = default;
        return false;
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