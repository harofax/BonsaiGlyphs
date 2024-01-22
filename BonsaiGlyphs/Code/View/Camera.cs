using BonsaiGlyphs.Code.Game;
using SadConsole.Entities;

namespace BonsaiGlyphs.Code.View;

internal class Camera : ScreenObject
{
    private IScreenObject? _target;

    public IScreenObject? Target
    {
        get => _target;
        set
        {
            _target = value;
            FollowTarget = true;
        }
    }

    private Point _targetPoint;

    public Point TargetPoint
    {
        get => _targetPoint;
        set
        {
            _targetPoint = value;
            FollowPoint = true;
        }
    }

    private bool _followTarget;

    public bool FollowTarget
    {
        get => _followTarget;
        set
        {
            if (value == true && _followPoint == true)
            {
                FollowPoint = false;
            }

            _followTarget = value;
        }
    }

    private bool _followPoint;
    private readonly BonsaiGlyphGame _game;

    public bool FollowPoint
    {
        get => _followPoint;
        set
        {
            if (value == true && _followTarget == true)
            {
                FollowTarget = false;
            }

            _followPoint = value;
        }
    }

    public Camera(ref BonsaiGlyphGame game, IScreenObject? target, int width, int height)
    {
        _game = game;
        _targetPoint = game.Surface.View.Center;
        _target = target;
    }

    public void UpdateCameraPos()
    {
        if (Target == null)
        {
            return;
        }

        _game.Surface.View = _game.Surface.View.WithCenter(Target.Position);

        _game.Surface.IsDirty = true;
    }

    public void SetTargetWithCenter(Point targetPoint)
    {
        TargetPoint = targetPoint.Translate(-_game.ViewWidth / 2, -_game.ViewHeight / 2);
    }

    public override void Update(TimeSpan delta)
    {
        base.Update(delta);
        
        if (_followTarget)
        {
            switch (Target)
            {
                case null:
                    return;
                case Entity ent:
                    _game.Surface.View = _game.Surface.View.WithCenter(ent.AbsolutePosition);
                    break;
                case IScreenSurface screenSurface:
                    _game.Surface.View = _game.Surface.View.WithCenter(screenSurface.UsePixelPositioning
                        ? screenSurface.AbsolutePosition / _game.FontSize
                        : screenSurface.AbsolutePosition);
                    break;
                default:
                    _game.Surface.View = _game.Surface.View.WithCenter(Target.Position);
                    break;
            }
        }
        else if (_followPoint)
        {
            if (_game.UsePixelPositioning)
            {
                _game.Surface.View = _game.Surface.View.WithCenter(TargetPoint / _game.FontSize);
            }
            _game.Surface.View = _game.Surface.View.WithPosition(TargetPoint);
        }
    }
}