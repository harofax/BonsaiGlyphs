using Timer = SadConsole.Components.Timer;

namespace BonsaiGlyphs.Code.Managers;

public class TickManager : ScreenObject
{
    public Timer TickTimer;
    
    public TickManager(TimeSpan tick)
    {
        TickTimer = new Timer(tick);
        SadComponents.Add(TickTimer);
    }

    public void StartTick()
    {
        TickTimer.Start();
    }

    public void StopTick()
    {
        TickTimer.Stop();
    }
}