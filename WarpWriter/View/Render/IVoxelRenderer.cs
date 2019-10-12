namespace WarpWriter.View.Render
{
    public interface IVoxelRenderer
    {
        /// <summary>
        /// Anything from 0 for fully transparent to 255 for fully opaque.
        /// </summary>
        byte Transparency { get; }
    }
}
