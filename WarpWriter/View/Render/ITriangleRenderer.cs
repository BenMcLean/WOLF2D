namespace WarpWriter.View.Render
{
    public interface ITriangleRenderer<T> : IVoxelRenderer where T : ITriangleRenderer<T>
    {
        /// <summary>
        /// Draws a triangle 3 high and 2 wide pointing left
        /// </summary>
        /// <returns>this</returns>
        T DrawLeftTriangle(int x, int y, uint color);

        /// <summary>
        /// Draws a triangle 3 high and 2 wide pointing right
        /// </summary>
        /// <returns>this</returns>
        T DrawRightTriangle(int x, int y, uint color);

        /// <summary>
        /// Draws a triangle 3 high and 2 wide pointing left, representing the visible vertical face of voxel
        /// </summary>
        /// <returns>this</returns>
        T DrawLeftTriangleVerticalFace(int x, int y, byte voxel, int vx, int vy, int vz);


        /// <summary>
        /// Draws a triangle 3 high and 2 wide pointing left, representing the left face of voxel
        /// </summary>
        /// <returns>this</returns>
        T DrawLeftTriangleLeftFace(int x, int y, byte voxel, int vx, int vy, int vz);

        /// <summary>
        /// Draws a triangle 3 high and 2 wide pointing left, representing the right face of voxel
        /// </summary>
        /// <returns>this</returns>
        T DrawLeftTriangleRightFace(int x, int y, byte voxel, int vx, int vy, int vz);

        /// <summary>
        /// Draws a triangle 3 high and 2 wide pointing right representing the visible vertical face of voxel
        /// </summary>
        /// <returns>this</returns>
        T DrawRightTriangleVerticalFace(int x, int y, byte voxel, int vx, int vy, int vz);

        /// <summary>
        /// Draws a triangle 3 high and 2 wide pointing right representing the left face of voxel
        /// </summary>
        /// <returns>this</returns>
        T DrawRightTriangleLeftFace(int x, int y, byte voxel, int vx, int vy, int vz);

        /// <summary>
        /// Draws a triangle 3 high and 2 wide pointing right representing the right face of voxel
        /// </summary>
        /// <returns>this</returns>
        T DrawRightTriangleRightFace(int x, int y, byte voxel, int vx, int vy, int vz);

        /// <summary>
        /// Draws a triangle 3 high and 2 wide pointing left, representing the visible vertical face of voxel
        /// </summary>
        /// <returns>this</returns>
        T DrawLeftTriangleVerticalFace(int x, int y, byte voxel, int depth, int vx, int vy, int vz);

        /// <summary>
        /// Draws a triangle 3 high and 2 wide pointing left, representing the left face of voxel
        /// </summary>
        /// <returns>this</returns>
        T DrawLeftTriangleLeftFace(int x, int y, byte voxel, int depth, int vx, int vy, int vz);

        /// <summary>
        /// Draws a triangle 3 high and 2 wide pointing left, representing the right face of voxel
        /// </summary>
        /// <returns>this</returns>
        T DrawLeftTriangleRightFace(int x, int y, byte voxel, int depth, int vx, int vy, int vz);

        /// <summary>
        /// Draws a triangle 3 high and 2 wide pointing right representing the visible vertical face of voxel
        /// </summary>
        /// <returns>this</returns>
        T DrawRightTriangleVerticalFace(int x, int y, byte voxel, int depth, int vx, int vy, int vz);

        /// <summary>
        /// Draws a triangle 3 high and 2 wide pointing right representing the left face of voxel
        /// </summary>
        /// <returns>this</returns>
        T DrawRightTriangleLeftFace(int x, int y, byte voxel, int depth, int vx, int vy, int vz);

        /// <summary>
        /// Draws a triangle 3 high and 2 wide pointing right representing the right face of voxel
        /// </summary>
        /// <returns>this</returns>
        T DrawRightTriangleRightFace(int x, int y, byte voxel, int depth, int vx, int vy, int vz);
    }
}
