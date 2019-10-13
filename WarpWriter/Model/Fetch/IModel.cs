namespace WarpWriter.Model.Fetch
{
    public interface IModel : IFetch
    {
        uint SizeX { get; }
        uint SizeY { get; }
        uint SizeZ { get; }

        /// <summary>
        /// Recommended (but not required) implementation:
        /// return !Outside(x, y, z);
        /// </summary>
        /// <returns>True if the given coordinate is inside the intended range</returns>
        bool Inside(int x, int y, int z);

        /// <summary>
        /// Recommended (but not required) implementation:
        /// return x < 0 || y < 0 || z < 0 || x >= SizeX || y >= SizeY || z >= SizeZ;
        /// </summary>
        /// <returns>True if the given coordinate is outside the intended range</returns>
        bool Outside(int x, int y, int z);
    }
}
