namespace WarpWriter.Model.Fetch
{
    public class ColorFetch : Fetch
    {
        private static readonly ColorFetch[] Colors = new ColorFetch[256];
        public static readonly ColorFetch Transparent = Get(0);

        public static ColorFetch Get(byte color)
        {
            if (Colors[color] == null)
                Colors[color] = new ColorFetch(color);
            return Colors[color];
        }

        public byte Color { get; private set; }

        private ColorFetch(byte color)
        {
            Color = color;
        }

        public override byte At(int x, int y, int z)
        {
            return Color;
        }
    }
}
