namespace BonsaiGlyphs.Code.Util;

public struct RayHitResult
{
    public bool Hit { get; }
    public Point? HitLocation { get; }
    public ColoredGlyphBase? Cell { get; }

    public RayHitResult(bool hit, ColoredGlyphBase? cell, Point hitLocation)
    {
        this.Hit = hit;
        this.Cell = cell;
        this.HitLocation = hitLocation;
    }
}