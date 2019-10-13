using System;
using System.IO;
using WarpWriter.Model.Fetch;

namespace WarpWriter.Model.IO
{
    public static class VoxIO
    {
        /// <summary>
        /// MagicaVoxel's default palette; used when no palette is specified.
        /// </summary>
        public static readonly uint[] MVPalette = new uint[]{
            0x00000000, 0xffffffff, 0xffffccff, 0xffff99ff, 0xffff66ff, 0xffff33ff, 0xffff00ff, 0xffccffff,
            0xffccccff, 0xffcc99ff, 0xffcc66ff, 0xffcc33ff, 0xffcc00ff, 0xff99ffff, 0xff99ccff, 0xff9999ff,
            0xff9966ff, 0xff9933ff, 0xff9900ff, 0xff66ffff, 0xff66ccff, 0xff6699ff, 0xff6666ff, 0xff6633ff,
            0xff6600ff, 0xff33ffff, 0xff33ccff, 0xff3399ff, 0xff3366ff, 0xff3333ff, 0xff3300ff, 0xff00ffff,
            0xff00ccff, 0xff0099ff, 0xff0066ff, 0xff0033ff, 0xff0000ff, 0xccffffff, 0xccffccff, 0xccff99ff,
            0xccff66ff, 0xccff33ff, 0xccff00ff, 0xccccffff, 0xccccccff, 0xcccc99ff, 0xcccc66ff, 0xcccc33ff,
            0xcccc00ff, 0xcc99ffff, 0xcc99ccff, 0xcc9999ff, 0xcc9966ff, 0xcc9933ff, 0xcc9900ff, 0xcc66ffff,
            0xcc66ccff, 0xcc6699ff, 0xcc6666ff, 0xcc6633ff, 0xcc6600ff, 0xcc33ffff, 0xcc33ccff, 0xcc3399ff,
            0xcc3366ff, 0xcc3333ff, 0xcc3300ff, 0xcc00ffff, 0xcc00ccff, 0xcc0099ff, 0xcc0066ff, 0xcc0033ff,
            0xcc0000ff, 0x99ffffff, 0x99ffccff, 0x99ff99ff, 0x99ff66ff, 0x99ff33ff, 0x99ff00ff, 0x99ccffff,
            0x99ccccff, 0x99cc99ff, 0x99cc66ff, 0x99cc33ff, 0x99cc00ff, 0x9999ffff, 0x9999ccff, 0x999999ff,
            0x999966ff, 0x999933ff, 0x999900ff, 0x9966ffff, 0x9966ccff, 0x996699ff, 0x996666ff, 0x996633ff,
            0x996600ff, 0x9933ffff, 0x9933ccff, 0x993399ff, 0x993366ff, 0x993333ff, 0x993300ff, 0x9900ffff,
            0x9900ccff, 0x990099ff, 0x990066ff, 0x990033ff, 0x990000ff, 0x66ffffff, 0x66ffccff, 0x66ff99ff,
            0x66ff66ff, 0x66ff33ff, 0x66ff00ff, 0x66ccffff, 0x66ccccff, 0x66cc99ff, 0x66cc66ff, 0x66cc33ff,
            0x66cc00ff, 0x6699ffff, 0x6699ccff, 0x669999ff, 0x669966ff, 0x669933ff, 0x669900ff, 0x6666ffff,
            0x6666ccff, 0x666699ff, 0x666666ff, 0x666633ff, 0x666600ff, 0x6633ffff, 0x6633ccff, 0x663399ff,
            0x663366ff, 0x663333ff, 0x663300ff, 0x6600ffff, 0x6600ccff, 0x660099ff, 0x660066ff, 0x660033ff,
            0x660000ff, 0x33ffffff, 0x33ffccff, 0x33ff99ff, 0x33ff66ff, 0x33ff33ff, 0x33ff00ff, 0x33ccffff,
            0x33ccccff, 0x33cc99ff, 0x33cc66ff, 0x33cc33ff, 0x33cc00ff, 0x3399ffff, 0x3399ccff, 0x339999ff,
            0x339966ff, 0x339933ff, 0x339900ff, 0x3366ffff, 0x3366ccff, 0x336699ff, 0x336666ff, 0x336633ff,
            0x336600ff, 0x3333ffff, 0x3333ccff, 0x333399ff, 0x333366ff, 0x333333ff, 0x333300ff, 0x3300ffff,
            0x3300ccff, 0x330099ff, 0x330066ff, 0x330033ff, 0x330000ff, 0x00ffffff, 0x00ffccff, 0x00ff99ff,
            0x00ff66ff, 0x00ff33ff, 0x00ff00ff, 0x00ccffff, 0x00ccccff, 0x00cc99ff, 0x00cc66ff, 0x00cc33ff,
            0x00cc00ff, 0x0099ffff, 0x0099ccff, 0x009999ff, 0x009966ff, 0x009933ff, 0x009900ff, 0x0066ffff,
            0x0066ccff, 0x006699ff, 0x006666ff, 0x006633ff, 0x006600ff, 0x0033ffff, 0x0033ccff, 0x003399ff,
            0x003366ff, 0x003333ff, 0x003300ff, 0x0000ffff, 0x0000ccff, 0x000099ff, 0x000066ff, 0x000033ff,
            0xee0000ff, 0xdd0000ff, 0xbb0000ff, 0xaa0000ff, 0x880000ff, 0x770000ff, 0x550000ff, 0x440000ff,
            0x220000ff, 0x110000ff, 0x00ee00ff, 0x00dd00ff, 0x00bb00ff, 0x00aa00ff, 0x008800ff, 0x007700ff,
            0x005500ff, 0x004400ff, 0x002200ff, 0x001100ff, 0x0000eeff, 0x0000ddff, 0x0000bbff, 0x0000aaff,
            0x000088ff, 0x000077ff, 0x000055ff, 0x000044ff, 0x000022ff, 0x000011ff, 0xeeeeeeff, 0xddddddff,
            0xbbbbbbff, 0xaaaaaaff, 0x888888ff, 0x777777ff, 0x555555ff, 0x444444ff, 0x222222ff, 0x111111ff

        };
        /// <summary>
        /// Reads as much as possible of stream to get a MagicaVoxel .vox file out of it, filling the shape of the
        /// first (or only) frame of the file into model, and the color choices into palette. This will write to
        /// model and palette.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="model"></param>
        /// <param name="palette"></param>
        public static void ReadVox(Stream stream, out byte[][][] model, out uint[] palette)
        {
            int sizex = 0, sizey = 0, sizez = 0;
            model = new byte[0][][];
            palette = new uint[256];
            Array.Copy(MVPalette, palette, 256);
            using (BinaryReader bin = new BinaryReader(stream))
            {
                string magic = new string(bin.ReadChars(4));
                int version = bin.ReadInt32();
                // a MagicaVoxel .vox file starts with a 'magic' 4 character 'VOX ' identifier
                if (magic == "VOX ")
                {
                    while (bin.BaseStream.Position < bin.BaseStream.Length)
                    {
                        // each chunk has an ID, size and child chunks
                        char[] chunkId = bin.ReadChars(4);
                        int chunkSize = bin.ReadInt32();
                        int childChunks = bin.ReadInt32();
                        string chunkName = new string(chunkId);

                        // there are only 2 chunks we only care about, and they are SIZE and XYZI
                        if (chunkName == "SIZE")
                        {
                            sizex = bin.ReadInt32();
                            sizey = bin.ReadInt32();
                            sizez = bin.ReadInt32();

                            bin.ReadBytes(chunkSize - 4 * 3);
                        }
                        else if (chunkName == "XYZI")
                        {
                            model = new byte[sizex][][];
                            for (int x = 0; x < sizex; x++)
                            {
                                model[x] = new byte[sizey][];
                                for (int y = 0; y < sizey; y++)
                                {
                                    model[x][y] = new byte[sizez];
                                }
                            }
                            // XYZI contains n voxels
                            int numVoxels = bin.ReadInt32();

                            // each voxel has x, y, z and color index values
                            for (int i = 0; i < numVoxels; i++)
                            {
                                model[bin.ReadByte()][bin.ReadByte()][bin.ReadByte()] = bin.ReadByte();
                            }
                        }
                        else if (chunkName == "RGBA")
                        {
                            for (int i = 0; i < 256; i++)
                            {
                                palette[i] = bin.ReadUInt32();
                            }
                        }
                        else bin.ReadBytes(chunkSize);   // read any excess bytes
                    }
                }
            }
            //// not needed any more
            //for(int i = 0; i < 256;i++)
            //{
            //    uint u = palette[i];
            //    palette[i] = u << 24 | (u << 8 & 0xFF0000u) | (u >> 8 & 0xFF00u) | u >> 24;
            //}
        }

        public static void ReadVox(Stream stream, out ArrayModel model, out uint[] palette)
        {
            ReadVox(stream, out byte[][][] m, out palette);
            model = new ArrayModel(m);
        }
    }
}
