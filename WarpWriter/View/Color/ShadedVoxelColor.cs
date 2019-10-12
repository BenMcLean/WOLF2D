using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarpWriter.View.Color
{
    class ShadedVoxelColor : IVoxelColor
    {
        public Colorizer Palette { get; set; }

        public uint LeftFace(byte voxel)
        {
            return Palette.Dimmer(2, voxel);
        }

        public uint LeftFace(byte voxel, int x, int y, int z)
        {
            return Palette.Dimmer(2, voxel);
        }

        public uint RightFace(byte voxel)
        {
            return Palette.Dimmer(1, voxel);
        }

        public uint RightFace(byte voxel, int x, int y, int z)
        {
            return Palette.Dimmer(1, voxel);
        }

        public uint VerticalFace(byte voxel)
        {
            return Palette.Dimmer(3, voxel);
        }

        public uint VerticalFace(byte voxel, int x, int y, int z)
        {
            return Palette.Dimmer(3, voxel);
        }
    }
}
