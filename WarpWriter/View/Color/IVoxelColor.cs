namespace WarpWriter.View.Color
{
    /// <summary>
    /// An IVoxelColor converts from the color index of a voxel to the rgba8888 colors which the visible faces of the cube representing it should be.
    /// </summary>
    public interface IVoxelColor
    {
        uint VerticalFace(byte voxel);

        uint LeftFace(byte voxel);

        uint RightFace(byte voxel);

        uint VerticalFace(byte voxel, int x, int y, int z);

        uint LeftFace(byte voxel, int x, int y, int z);

        uint RightFace(byte voxel, int x, int y, int z);
    }
}
