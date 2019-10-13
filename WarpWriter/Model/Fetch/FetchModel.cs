namespace WarpWriter.Model.Fetch
{
    public class FetchModel : Fetch, IModel
    {
        public Fetch Fetch { get; set; }
        public uint SizeX { get; set; }
        public uint SizeY { get; set; }
        public uint SizeZ { get; set; }

        public override byte At(int x, int y, int z)
        {
            return Fetch.At(x, y, z);
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
