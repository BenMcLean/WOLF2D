namespace WarpWriter.Model.Fetch
{
    /// <summary>
    /// An abstraction for any types that allow querying a 3D position to get a color index(as a byte).
    /// Created by Tommy Ettinger on 10/11/2018.
    /// </summary>
    public interface IFetch
    {
        /// <summary>
        /// Looks up a color index (a byte) from a 3D position as x,y,z int parameters. Index 0 is used to mean an empty position with no color.
        /// </summary>
        /// <param name="x">x position to look up; depending on angle, can be forward/back or left/right</param>
        /// <param name="y">y position to look up; depending on angle, can be left/right or forward/back</param>
        /// <param name="z">z position to look up; almost always up/down</param>
        /// <returns>a color index as a byte; 0 is empty, and this should usually be masked with {@code & 255}</returns>
        byte At(int x, int y, int z);
    }
}
