namespace WarpWriter.View.Render
{
    /// <summary>
    /// An IRectangleRenderer understands how to draw rectangles representing the three visible faces of a voxel cube.
    /// </summary>
    public interface IRectangleRenderer<T> : IVoxelRenderer where T : IRectangleRenderer<T>
    {
        T Rect(int x, int y, int sizeX, int sizeY, uint color);

        T RectVertical(int x, int y, int sizeX, int sizeY, byte voxel);

        T RectLeft(int x, int y, int sizeX, int sizeY, byte voxel);

        T RectRight(int x, int y, int sizeX, int sizeY, byte voxel);

        T RectVertical(int px, int py, int sizeX, int sizeY, byte voxel, int depth, int vx, int vy, int vz);

        T RectLeft(int px, int py, int sizeX, int sizeY, byte voxel, int depth, int vx, int vy, int vz);

        T RectRight(int px, int py, int sizeX, int sizeY, byte voxel, int depth, int vx, int vy, int vz);
    }
}
