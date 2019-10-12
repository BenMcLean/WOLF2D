using System;
using WarpWriter.Model.Extra;

namespace WarpWriter.View.Color
{
    public class PaletteReducer
    {
        public readonly byte[] PaletteMapping = new byte[0x8000];
        public readonly uint[] PaletteArray = new uint[256];
        public int PaletteLength;

        public static readonly uint[] RELAXED_ROLL = {
            0x00000000, 0x100818ff, 0x181818ff, 0x314a6bff, 0x396b7bff, 0x4a9494ff, 0xa5b5adff, 0xb5e7e7ff,
            0xf7efefff, 0x6b1831ff, 0xbd5242ff, 0xef6b4aff, 0xef9c9cff, 0xf7c6deff, 0x6b3921ff, 0xbd8421ff,
            0xefa531ff, 0xe7ce42ff, 0xefd6a5ff, 0x292921ff, 0x7b5231ff, 0x8c7339ff, 0xb59473ff, 0xcec6a5ff,
            0x316b31ff, 0xadbd42ff, 0xefef39ff, 0xeff79cff, 0xe7f7deff, 0x215a21ff, 0x52bd39ff, 0x84e731ff,
            0xb5ef42ff, 0xbdef9cff, 0x295221ff, 0x29ad29ff, 0x31e729ff, 0x39ef7bff, 0x52f7b5ff, 0x214221ff,
            0x318439ff, 0x42ad84ff, 0x4aceadff, 0x5ae7e7ff, 0x180842ff, 0x3118a5ff, 0x3921deff, 0x428cc6ff,
            0x42bde7ff, 0x293163ff, 0x4a63b5ff, 0x5a84efff, 0x9ca5e7ff, 0xced6efff, 0x211073ff, 0x5a3194ff,
            0x8431d6ff, 0xb573b5ff, 0xc6bde7ff, 0x421039ff, 0xa5214aff, 0xde2152ff, 0xde31ceff, 0xe784deff,
        };

        public PaletteReducer() : this(RELAXED_ROLL)
        { }

        public PaletteReducer(uint[] palette)
        {
            Exact(palette);
        }

        public void Exact(uint[] rgbaPalette)
        {
            if (rgbaPalette == null || rgbaPalette.Length < 2)
            {
                rgbaPalette = RELAXED_ROLL;
            }
            Array.Clear(PaletteArray, 0, 256);
            Array.Clear(PaletteMapping, 0, 0x8000);
            PaletteLength = Math.Min(256, rgbaPalette.Length);
            uint color, c2;
            double dist;
            for (int i = 0; i < PaletteLength; i++)
            {
                color = rgbaPalette[i];
                PaletteArray[i] = color;
                PaletteMapping[(color >> 17 & 0x7C00) | (color >> 14 & 0x3E0) | (color >> 11 & 0x1F)] = (byte)i;
            }
            uint rr, gg, bb;
            for (uint r = 0; r < 32; r++)
            {
                rr = (r << 3 | r >> 2);
                for (uint g = 0; g < 32; g++)
                {
                    gg = (g << 3 | g >> 2);
                    for (uint b = 0; b < 32; b++)
                    {
                        c2 = r << 10 | g << 5 | b;
                        if (PaletteMapping[c2] == 0)
                        {
                            bb = (b << 3 | b >> 2);
                            dist = 0x7FFFFFFFU;
                            for (int i = 1; i < PaletteLength; i++)
                            {
                                if (dist > (dist = Math.Min(dist, Difference(PaletteArray[i], rr, gg, bb))))
                                    PaletteMapping[c2] = (byte)i;
                            }
                        }
                    }
                }
            }
        }

        public double Difference(uint rgba1, uint r2, uint g2, uint b2)
        {
            if ((rgba1 & 0x80) == 0) return Double.PositiveInfinity;
            return Difference((rgba1 >> 24), (rgba1 >> 16 & 0xFF), (rgba1 >> 8 & 0xFF), r2, g2, b2);
        }

        public double Difference(uint r1, uint g1, uint b1, uint r2, uint g2, uint b2)
        {
            double rmean = (r1 + r2),
                r = r1 - r2, g = (g1 - g2) * 2.0, b = b1 - b2,
                y = Math.Max(r1, Math.Max(g1, b1)) - Math.Max(r2, Math.Max(g2, b2));
            return (((1024 + rmean) * r * r) / 128.0) + g * g * 12 + (((1534 - rmean) * b * b) / 256.0) + y * y * 14;

            //double x, y, z, r, g, b;

            //r = r1 / 255.0;
            //g = g1 / 255.0;
            //b = b1 / 255.0;

            //r = Math.Pow((r + 0.055) / 1.055, 2.4);
            //g = Math.Pow((g + 0.055) / 1.055, 2.4);
            //b = Math.Pow((b + 0.055) / 1.055, 2.4);

            //x = (r * 0.4124 + g * 0.3576 + b * 0.1805);
            //y = (r * 0.2126 + g * 0.7152 + b * 0.0722);
            //z = (r * 0.0193 + g * 0.1192 + b * 0.9505);

            //x = Math.Sqrt(x);
            //y = Math.Pow(y, 0.333333333);
            //z = Math.Sqrt(z);

            //double L = 100 * y;
            //double A = 500.0 * (x - y);
            //double B = 200.0 * (y - z);

            //r = r2 / 255.0;
            //g = g2 / 255.0;
            //b = b2 / 255.0;

            //r = Math.Pow((r + 0.055) / 1.055, 2.4);
            //g = Math.Pow((g + 0.055) / 1.055, 2.4);
            //b = Math.Pow((b + 0.055) / 1.055, 2.4);

            //x = (r * 0.4124 + g * 0.3576 + b * 0.1805);
            //y = (r * 0.2126 + g * 0.7152 + b * 0.0722);
            //z = (r * 0.0193 + g * 0.1192 + b * 0.9505);

            //x = Math.Sqrt(x);
            //y = Math.Pow(y, 0.333333333);
            //z = Math.Sqrt(z);

            //L -= 100.0 * y;
            //A -= 500.0 * (x - y);
            //B -= 200.0 * (y - z);

            //return L * L * 350.0 + A * A * 25.0 + B * B * 10.0;
        }

        public byte RandomColorIndex(RNG random)
        {
            return PaletteMapping[random.Next(15)];
        }

        /**
         * Retrieves a random non-transparent color from the palette this would reduce to, with a higher likelihood for
         * colors that are used more often in reductions (those with few similar colors). The color is returned as an
         * RGBA8888 int; you can assign one of these into a Color with {@link Color#rgba8888ToColor(Color, int)} or
         * {@link Color#set(int)}.
         * @param random a Random instance, which may be seeded
         * @return a randomly selected color from this palette with a non-uniform distribution
         */
        public uint RandomColor(RNG random)
        {
            return PaletteArray[PaletteMapping[random.Next(15)] & 255];
        }

        /**
         * Looks up {@code color} as if it was part of an image being color-reduced and finds the closest color to it in the
         * palette this holds. Both the parameter and the returned color are RGBA8888 ints.
         * @param color an RGBA8888 int that represents a color this should try to find a similar color for in its palette
         * @return an RGBA8888 int representing a color from this palette, or 0 if color is mostly transparent
         * (0 is often but not always in the palette)
         */
        public uint ReduceSingle(uint color)
        {
            if ((color & 0x80) == 0) // less visible than half-transparent
                return 0; // transparent
            return PaletteArray[PaletteMapping[
                    (color >> 17 & 0x7C00)
                            | (color >> 14 & 0x3E0)
                            | (color >> 11 & 0x1F)] & 0xFF];
        }

        /**
         * Looks up {@code color} as if it was part of an image being color-reduced and finds the closest color to it in the
         * palette this holds. The parameter is a RGBA8888 int, the returned color is a byte index into the
         * {@link #paletteArray} (mask it like: {@code paletteArray[reduceIndex(color) & 0xFF]}).
         * @param color an RGBA8888 int that represents a color this should try to find a similar color for in its palette
         * @return a byte index that can be used to look up a color from the {@link #paletteArray}
         */
        public byte ReduceIndex(uint color)
        {
            if ((color & 0x80) == 0) // less visible than half-transparent
                return 0; // transparent
            return PaletteMapping[
                    (color >> 17 & 0x7C00)
                            | (color >> 14 & 0x3E0)
                            | (color >> 11 & 0x1F)];
        }

    }
}
