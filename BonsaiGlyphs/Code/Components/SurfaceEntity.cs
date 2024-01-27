using System;
using System.Runtime.Serialization;
using SadRogue.Primitives;

namespace SadConsole.Entities;

public partial class Entity
{
    /// <summary>
    /// An entity that is a surface.
    /// </summary>
    [DataContract]
    public class SurfaceEntity
    {
        /// <summary>
        /// The ScreenSurface associated with this SurfaceEntity.
        /// </summary>
        [DataMember]
        public ICellSurface Surface { get; }
        
        /// <summary>
        /// Represents the collision rectangle for this SurfaceEntity surface which is the size of the SurfaceEntity screenSurface.
        /// </summary>
        public Rectangle DefaultCollisionRectangle
        {
            get => new Rectangle(0, 0, Surface.ViewWidth, Surface.ViewHeight);
        }

        /// <summary>
        /// A relative collision rectangle that you can specify. Defaults to the size of the SurfaceEntity surface.
        /// </summary>
        /// <remarks>
        /// This rectangle should be declared without using the SurfaceEntity center. Only apply the center when you're testing for collision and reading this rectangle.
        /// </remarks>
        [DataMember]
        public Rectangle CustomCollisionRectangle { get; set; }

        /// <summary>
        /// When <see langword="true"/>, indicates that this SurfaceEntity is dirty and needs to be redrawn.
        /// </summary>
        public bool IsDirty { get => Surface.IsDirty; set => Surface.IsDirty = value; }

        /// <summary>
        /// Creates a new instance of this type from an animated screen surface.
        /// </summary>
        /// <param name="surface">The animation to use.</param>
        public SurfaceEntity(ICellSurface surface) =>
            Surface = surface;

        /// <summary>
        /// Updates the <see cref="SurfaceEntity"/>.
        /// </summary>
        /// <param name="delta">The time that has elapsed since this method was last called.</param>
        //public void Update(TimeSpan delta) =>
        //    Surface.Update(delta);
    }
}
