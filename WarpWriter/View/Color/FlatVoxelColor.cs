namespace WarpWriter.View.Color
{
    public class FlatVoxelColor : IVoxelColor
    {
        public uint[] Palette { get; set; }

        public uint LeftFace(byte voxel)
        {
            return Palette[voxel];
        }

        public uint LeftFace(byte voxel, int x, int y, int z)
        {
            return LeftFace(voxel);
        }

        public uint RightFace(byte voxel)
        {
            return Palette[voxel];
        }

        public uint RightFace(byte voxel, int x, int y, int z)
        {
            return RightFace(voxel);
        }

        public uint VerticalFace(byte voxel)
        {
            return Palette[voxel];
        }

        public uint VerticalFace(byte voxel, int x, int y, int z)
        {
            return VerticalFace(voxel);
        }
    }
}
