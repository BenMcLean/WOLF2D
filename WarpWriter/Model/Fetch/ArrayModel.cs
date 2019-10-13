namespace WarpWriter.Model.Fetch
{
    public class ArrayModel : Fetch, IModel
    {
        public byte[][][] Voxels { get; set; }

        public ArrayModel(byte[][][] voxels)
        {
            Voxels = voxels;
        }

        public ArrayModel(IFetch fetch, uint xSize, uint ySize, uint zSize)
        {
            Voxels = new byte[xSize][][];
            for (uint x = 0; x < Voxels.Length; x++)
            {
                Voxels[x] = new byte[ySize][];
                for (uint y = 0; y < Voxels[x].Length; y++)
                {
                    Voxels[x][y] = new byte[zSize];
                    for (uint z = 0; z < Voxels[x][y].Length; z++)
                        Voxels[x][y][z] = fetch.At((int)x, (int)y, (int)z);
                }
            }
        }

        public ArrayModel(IModel model) : this(model, model.SizeX, model.SizeY, model.SizeZ)
        { }

        public uint SizeX
        {
            get
            {
                return Voxels == null ? 0 : (uint)Voxels.Length;
            }
        }

        public uint SizeY
        {
            get
            {
                return Voxels == null || Voxels[0] == null ? 0 : (uint)Voxels[0].Length;
            }
        }

        public uint SizeZ
        {
            get
            {
                return Voxels == null || Voxels[0] == null || Voxels[0][0] == null ? 0 : (uint)Voxels[0][0].Length;
            }
        }

        public override byte At(int x, int y, int z)
        {
            return Inside(x, y, z) ? Voxels[x][y][z] : (byte)0;
        }

        public bool Inside(int x, int y, int z)
        {
            return !Outside(x, y, z);
        }

        public bool Outside(int x, int y, int z)
        {
            return x < 0 || y < 0 || z < 0 || x >= SizeX || y >= SizeY || z >= SizeZ;
        }
    }
}
