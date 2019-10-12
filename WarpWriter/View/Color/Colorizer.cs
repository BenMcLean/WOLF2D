using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarpWriter.Model.Extra;

namespace WarpWriter.View.Color
{
    public class Colorizer
    {
        public byte[][] Ramps;
        public uint[][] Values;
        public int Count;
        public byte[] Primary;
        public byte[] Grays;
        public PaletteReducer Reducer;
        public Colorizer(uint[] palette)
        {
            Count = palette.Length;
            Reducer = new PaletteReducer(palette);

            Primary = new byte[] {
                Reducer.ReduceIndex(0xFF0000FF), Reducer.ReduceIndex(0xFFFF00FF), Reducer.ReduceIndex(0x00FF00FF),
                Reducer.ReduceIndex(0x00FFFFFF), Reducer.ReduceIndex(0x0000FFFF), Reducer.ReduceIndex(0xFF00FFFF)
            };
            Grays = new byte[] {
                Reducer.ReduceIndex(0xFF000000), Reducer.ReduceIndex(0xFF444444), Reducer.ReduceIndex(0xFF888888),
                Reducer.ReduceIndex(0xFFCCCCCC), Reducer.ReduceIndex(0xFFFFFFFF)
            };
            int THRESHOLD = 64;//0.011; // threshold controls the "stark-ness" of color changes; must not be negative.
            byte[] paletteMapping = new byte[1 << 16];
            uint[] reverse = new uint[Count];
            Ramps = BasicTools.Fill((byte)0, Count, 4);
            Values = BasicTools.Fill(0u, Count, 4);

            uint[] lumas = new uint[Count], cws = new uint[Count], cms = new uint[Count];
            uint yLim = 63, cwLim = 31, cmLim = 31;
            int shift1 = 6, shift2 = 11;
            uint color;
            int r, g, b;
            int cw, cm;
            for (int i = 1; i < Count; i++)
            {
                color = palette[i];
                if ((color & 0x80000000) == 0)
                {
                    color |= 0xFF000000;
                }
                r = (int)(color & 0xFF);
                g = (int)(color >> 8 & 0xFF);
                b = (int)(color >> 16 & 0xFF);
                cw = r - b;
                cm = g - b;
                paletteMapping[
                        reverse[i] =
                                  (lumas[i] = (uint)(r * 3 + g * 4 + b) >> 5)
                                | (cws[i] = (uint)cw + 255 >> 4) << shift1
                                | (cms[i] = (uint)cm + 255 >> 4) << shift2] = (byte)i;
            }

            for (int icm = 0; icm <= cmLim; icm++)
            {
                for (int icw = 0; icw <= cwLim; icw++)
                {
                    for (int iy = 0; iy <= yLim; iy++)
                    {
                        int c2 = icm << shift2 | icw << shift1 | iy;
                        if (paletteMapping[c2] == 0)
                        {
                            double dist = double.PositiveInfinity;
                            for (int i = 1; i < Count; i++)
                            {
                                if (Math.Abs(lumas[i] - (int)iy) < 28 && dist > (dist = Math.Min(dist, DifferenceWarmMild((int)lumas[i], (int)cws[i], (int)cms[i], iy, icw, icm))))
                                    paletteMapping[c2] = (byte)i;
                            }
                        }
                    }
                }
            }

            float adj, cwf, cmf;
            int idx2;
            for (int i = 1; i < Count; i++)
            {
                uint rev = reverse[i];
                int y = (int)(rev & yLim), match = i, yBright = y * 9 / 2, yDim = y * 3, yDark = y, luma, warm, mild;

                cwf = (cw = (int)cws[i] - 16) / 30f;
                cmf = (cm = (int)cms[i] - 16) / 30f;
                //Console.WriteLine("i={0:D3}: y={1:D3}, cw={2:D3}, cm={3:D3}", i, y, cw, cm);
                //values[i][0] = values[i][1] = values[i][3] = 
                Values[i][2] = palette[i];

                luma = yDim;
                //warm = ((cw * 395 + 31) / 32 - 192) / 16;
                //mild = ((cm * 395 + 31) / 32 - 192) / 16;
                warm = cw * 5 / 3;
                mild = cm * 5 / 3;
                r = (luma + warm * 5 - mild * 4);
                g = (luma - warm * 3 + mild * 4);
                b = (luma - warm * 3 - mild * 4);

                Values[i][1] = (uint)(
                        BasicTools.Clamp(r, 0, 255) |
                                BasicTools.Clamp(g, 0, 255) << 8 |
                                BasicTools.Clamp(b, 0, 255) << 16) | 0xFF000000U;

                luma = yBright;
                //warm = ((cw * 333 + 31) / 32 - 162) / 16;
                //mild = ((cm * 333 + 31) / 32 - 162) / 16;
                warm = cw * 5 / 3;
                mild = cm * 5 / 3;
                r = (luma + warm * 5 - mild * 4);
                g = (luma - warm * 3 + mild * 4);
                b = (luma - warm * 3 - mild * 4);
                Values[i][3] = (uint)(
                        BasicTools.Clamp(r, 0, 255) |
                                BasicTools.Clamp(g, 0, 255) << 8 |
                                BasicTools.Clamp(b, 0, 255) << 16) | 0xFF000000U;
                luma = yDark;
                //warm = ((cw * 215) / 16 - 208) / 16;
                //mild = ((cm * 215) / 16 - 208) / 16;
                warm = cw * 3 / 2;
                mild = cm * 3 / 2;
                r = (luma + warm * 5 - mild * 4);
                g = (luma - warm * 3 + mild * 4);
                b = (luma - warm * 3 - mild * 4);
                Values[i][0] = (uint)(
                        BasicTools.Clamp(r, 0, 255) |
                                BasicTools.Clamp(g, 0, 255) << 8 |
                                BasicTools.Clamp(b, 0, 255) << 16) | 0xFF000000U;

                //Console.WriteLine("{0:D}:{1:X8},{2:X8},{3:X8},{4:X8}", i, Values[i][0], Values[i][1], Values[i][2], Values[i][3]);
                
                //// This seems like a much simpler approach than the large comment below it.
                //// The downside is that in some small palettes, the lighter or darker variants may be the same as the original, or both
                //// darker variants could be the same.
                Ramps[i][0] = Reducer.ReduceIndex(Values[i][0]);
                Ramps[i][1] = Reducer.ReduceIndex(Values[i][1]);
                Ramps[i][2] = (byte)i;
                Ramps[i][3] = Reducer.ReduceIndex(Values[i][3]);
                //Ramps[i][2] = (byte)i;
                //Ramps[i][3] = Grays[4];//15;  //0xFFFFFFFF, white
                //Ramps[i][1] = Grays[0];//0x010101FF, black
                //Ramps[i][0] = Grays[0];//0x010101FF, black
                //for (int yy = y + 2, rr = (int)rev + 2; yy <= yLim; yy++, rr++)
                //{
                //    if ((idx2 = paletteMapping[rr] & 255) != i && DifferenceWarmMild((int)lumas[idx2], (int)cws[idx2], (int)cms[idx2], y, cw, cm) > THRESHOLD)
                //    {
                //        Ramps[i][3] = paletteMapping[rr];
                //        break;
                //    }
                //    adj = 1f + ((yLim + 1) * 0.5f - yy) / 1024f;
                //    cwf = BasicTools.Clamp(cwf * adj, -0.5f, 0.5f);
                //    cmf = BasicTools.Clamp(cmf * adj, -0.5f, 0.5f);

                //    rr = yy
                //            | (cw = (int)((cwf + 0.5f) * cwLim)) << shift1
                //            | (cm = (int)((cmf + 0.5f) * cmLim)) << shift2;
                //}
                //cwf = ((cw = (int)cws[i]) - 16) * 11f / 32f;
                //cmf = ((cm = (int)cms[i]) - 16) * 11f / 32f;
                //for (int yy = y - 2, rr = (int)rev - 2; yy > 0; rr--)
                //{
                //    if ((idx2 = paletteMapping[rr] & 255) != i && DifferenceWarmMild((int)lumas[idx2], (int)cws[idx2], (int)cms[idx2], y, cw, cm) > THRESHOLD)
                //    {
                //        Ramps[i][1] = paletteMapping[rr];
                //        rev = (uint)rr;
                //        y = yy;
                //        match = paletteMapping[rr] & 255;
                //        break;
                //    }

                //    adj = 1f + (yy - (yLim + 1) * 0.5f) / 1024f;
                //    cwf = BasicTools.Clamp(cwf * adj, -0.5f, 0.5f);
                //    cmf = BasicTools.Clamp(cmf * adj, -0.5f, 0.5f);

                //    rr = yy
                //            | (cw = (int)((cwf + 0.5f) * cwLim)) << shift1
                //            | (cm = (int)((cmf + 0.5f) * cmLim)) << shift2;

                //    if (--yy == 0)
                //    {
                //        match = -1;
                //    }
                //}
                //if (match >= 0)
                //{
                //    cwf = ((cw = (int)cws[match]) - 16) / 30f;
                //    cmf = ((cm = (int)cms[match]) - 16) / 30f;
                //    for (int yy = y - 3, rr = (int)rev - 3; yy > 0; yy--, rr--)
                //    {
                //        if ((idx2 = paletteMapping[rr] & 255) != match && DifferenceWarmMild((int)lumas[idx2], (int)cws[idx2], (int)cms[idx2], y, cw, cm) > THRESHOLD)
                //        {
                //            Ramps[i][0] = paletteMapping[rr];
                //            break;
                //        }

                //        adj = 1f + (yy - (yLim + 1) * 0.5f) / 1024f;
                //        cwf = BasicTools.Clamp(cwf * adj, -0.5f, 0.5f);
                //        cmf = BasicTools.Clamp(cmf * adj, -0.5f, 0.5f);

                //        rr = yy
                //                | (cw = (int)((cwf + 0.5f) * cwLim)) << shift1
                //                | (cm = (int)((cmf + 0.5f) * cmLim)) << shift2;
                //    }
                //}
            }
        }

        public byte[] MainColors()
        {
            return Primary;
        }

        /**
         * @return An array of grayscale or close-to-grayscale color indices, with the darkest first and lightest last.
         */
        public byte[] Grayscale()
        {
            return Grays;
        }
        
        public byte Brighten(byte voxel)
        {
            return Ramps[voxel % Count][3];
        }

        public byte Darken(byte voxel)
        {
            return Ramps[voxel % Count][1];
        }

        public uint Dimmer(int brightness, byte voxel)
        {
            return Values[voxel % Count][BasicTools.Clamp(brightness, 0, 3)];
        }

        public byte Colorize(byte voxel, int brightness)
        {
            if (brightness > 0)
            {
                for (int i = 0; i < brightness; i++)
                {
                    voxel = Brighten(voxel);
                }
            }
            else if (brightness < 0)
            {
                for (int i = 0; i > brightness; i--)
                {
                    voxel = Darken(voxel);
                }
            }
            return voxel;
        }

        public byte Reduce(uint color)
        {
            return Reducer.ReduceIndex(color);
        }


        private static int DifferenceWarmMild(int y1, int cw1, int cm1, int y2, int cw2, int cm2)
        {
            cw1 -= 16;
            cm1 -= 16;
            cw2 -= 16;
            cm2 -= 16;
            return ((y1 - y2) * (y1 - y2) << 2) + (((cw1 - cw2) * (cw1 - cw2) + (cm1 - cm2) * (cm1 - cm2)) * 3);
        }



    }
}
