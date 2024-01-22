using BonsaiGlyphs.Code.Tree;
using BonsaiGlyphs.Code.View;
using SadConsole.Input;
using Mouse = SadConsole.Quick.Mouse;

namespace BonsaiGlyphs.Code.Game;

internal class RootScreen : ScreenObject
{
    private BonsaiTree _bonsaiTree;
    private BonsaiGlyphGame _game;
    private Camera _camera;

    public RootScreen()
    {
        _game = new BonsaiGlyphGame(GameSettings.VIEW_WIDTH,
            GameSettings.VIEW_HEIGHT, GameSettings.GAME_WIDTH, GameSettings.GAME_HEIGHT);

        _game.UsePixelPositioning = true;

        _bonsaiTree = new BonsaiTree(GameSettings.GAME_WIDTH, GameSettings.GAME_HEIGHT);

        _game.Layers.Add(_bonsaiTree.Cells);

        _camera = new Camera(ref _game, null, GameSettings.VIEW_WIDTH, GameSettings.VIEW_HEIGHT);
        
        _camera.SetTargetWithCenter(_game.Surface.Area.Center);

        FitToWindow();

        Children.Add(_game);
        Children.Add(_camera);
    }
    

    public override bool ProcessKeyboard(Keyboard keyboard)
    {
        //base.ProcessKeyboard(keyboard);
        
        

        bool playerMoved = false;
        Direction panDir = Direction.None;

        if (keyboard.IsKeyPressed(Keys.D))
        {
            panDir = Direction.Right;
            playerMoved = true;
        }
        else if (keyboard.IsKeyPressed(Keys.A))
        {
            panDir = Direction.Left;
            playerMoved = true;
        }
        else if (keyboard.IsKeyPressed(Keys.W))
        {
            panDir = Direction.Up;
            playerMoved = true;
        }
        else if (keyboard.IsKeyPressed(Keys.S))
        {
            panDir = Direction.Down;
            playerMoved = true;
        }

        if (keyboard.IsKeyPressed(Keys.F1))
        {
            Bonsai.Debug.ShowDebugConsole();
        }

        if (keyboard.IsKeyPressed(Keys.C))
        {
            _camera.FollowTarget = !_camera.FollowTarget;
        }

        if (!playerMoved) return false;

        Point newPos = _camera.TargetPoint.Add(panDir);
        
        if (_game.Surface.Area.Contains(newPos))
        {
            _camera.TargetPoint = newPos;
        }
        
        return true;
    }

   //public override bool ProcessMouse(MouseScreenObjectState state)
   //{
   //    _camera.TargetPoint = state.CellPosition;
   //    _camera.FollowPoint = true;
   //    
   //    _game.Print(state.SurfacePixelPosition.X, state.SurfacePixelPosition.Y, state.SurfacePixelPosition.ToString());
   //
   //    return base.ProcessMouse(state);
   //}

    public void FitToWindow()
    {
        _game.Surface.View = _game.Surface.View.WithSize(
            SadConsole.Game.Instance.MonoGameInstance.WindowWidth / _game.FontSize.X,
            SadConsole.Game.Instance.MonoGameInstance.WindowHeight / _game.FontSize.Y);

        IsFocused = true;
    }

    public void OnResize(object? sender, EventArgs e)
    {
        FitToWindow();
    }
}