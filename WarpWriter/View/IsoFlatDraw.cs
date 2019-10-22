using WarpWriter.View.Render;

namespace WarpWriter.WarpWriter.View
{
    public static class IsoFlatDraw
    {
        public static T IsoTile<T>(this T renderer, byte[][] indexes, uint[] palette) where T : IRectangleRenderer<T>
        {
            //Cartesian to isometric:
            //isoX = cartX - cartY;
            //isoY = (cartX + cartY) / 2;
            int xOffset = indexes.Length + indexes[0].Length - 2;
            for (int x = 0; x < indexes.Length; x++)
                for (int y = 0; y < indexes[x].Length; y++)
                    renderer.Rect(
                        xOffset + (x - y) * 2,
                        (x + y),
                        2, 2,
                        palette[indexes[x][y]]
                        );
            return renderer;
        }

        public static T IsoTile<T>(this T renderer, uint xSize, uint ySize, uint color) where T : IRectangleRenderer<T>
        {
            int xOffset = (int)(xSize + ySize) - 2;
            for (int x = 0; x < xSize; x++)
                for (int y = 0; y < ySize; y++)
                    renderer.Rect(
                        xOffset + (x - y) * 2,
                        (x + y),
                        2, 2,
                        color
                        );
            return renderer;
        }

        public static T IsoSlantUp<T>(this T renderer, byte[][] indexes, uint[] palette) where T : IRectangleRenderer<T>
        {
            for (int x = 0; x < indexes.Length; x++)
                for (int y = 0; y < indexes[x].Length; y++)
                    renderer.Rect(x * 2, x + y * 2, 1, 2, palette[indexes[x][y]])
                            .Rect(x * 2 + 1, x + y * 2 + 1, 1, 2, palette[indexes[x][y]]);
            return renderer;
        }

        public static T IsoSlantDown<T>(this T renderer, byte[][] indexes, uint[] palette) where T : IRectangleRenderer<T>
        {
            int height = indexes.Length - 1 + (indexes[0].Length - 1) * 2;
            for (int x = 0; x < indexes.Length; x++)
                for (int y = 0; y < indexes[x].Length; y++)
                {
                    uint color = palette[indexes[x][y]];
                    int y2 = (indexes[x].Length - 1 - y) * 2;
                    renderer.Rect(x * 2, height - (x + y2), 1, 2, color)
                            .Rect(x * 2 + 1, height - (x + y2) - 1, 1, 2, color);
                }
            return renderer;
        }
    }
}
