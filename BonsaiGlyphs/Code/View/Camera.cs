using BonsaiGlyphs.Code.Game;
using SadConsole.Entities;

namespace BonsaiGlyphs.Code.View;

internal class Camera<T> : ScreenObject where T : IScreenSurface
{
    private IScreenObject? targetEntity;

    public IScreenObject? Target
    {
        get => targetEntity;
        set
        {
            targetEntity = value;
            FollowTarget = true;
        }
    }

    private Point targetPoint;

    public Point TargetPoint
    {
        get => targetPoint;
        set
        {
            targetPoint = value;
            FollowPoint = true;
        }
    }

    private bool shouldFollowTarget;

    public bool FollowTarget
    {
        get => shouldFollowTarget;
        set
        {
            if (value == true && shouldFollowPoint == true)
            {
                FollowPoint = false;
            }

            shouldFollowTarget = value;
        }
    }

    private bool shouldFollowPoint;
    private readonly T world;

    public bool FollowPoint
    {
        get => shouldFollowPoint;
        set
        {
            if (value == true && shouldFollowTarget == true)
            {
                FollowTarget = false;
            }

            shouldFollowPoint = value;
        }
    }

    public Camera(ref T world, int width, int height) : this(ref world, width, height, null)
    {
    }

    public Camera(ref T world, int width, int height, IScreenObject? targetEntity)
    {
        this.world = world;
        targetPoint = world.Surface.View.Center;
        this.targetEntity = targetEntity;
    }

    public void UpdateCameraPos()
    {
        if (Target == null)
        {
            return;
        }

        world.Surface.View = world.Surface.View.WithCenter(Target.Position);

        world.Surface.IsDirty = true;
    }

    public void SetTargetWithCenter(Point centerPoint)
    {
        TargetPoint = centerPoint.Translate(-world.Surface.ViewWidth / 2, -world.Surface.ViewHeight / 2);
    }

    public override void Update(TimeSpan delta)
    {
        base.Update(delta);

        if (shouldFollowTarget)
        {
            switch (Target)
            {
                case null:
                    return;
                case Entity ent:
                    world.Surface.View = world.Surface.View.WithCenter(ent.AbsolutePosition);
                    break;
                case IScreenSurface screenSurface:
                    world.Surface.View = world.Surface.View.WithCenter(screenSurface.UsePixelPositioning
                        ? screenSurface.AbsolutePosition / world.FontSize
                        : screenSurface.AbsolutePosition);
                    break;
                default:
                    world.Surface.View = world.Surface.View.WithCenter(Target.Position);
                    break;
            }
        }
        else if (shouldFollowPoint)
        {
            if (world.UsePixelPositioning)
            {
                world.Surface.View = world.Surface.View.WithCenter(TargetPoint / world.FontSize);
            }

            world.Surface.View = world.Surface.View.WithPosition(TargetPoint);
        }
    }
}