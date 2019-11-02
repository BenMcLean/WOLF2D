using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace WOLF3D
{
    public struct VgaGraph
    {
        XElement XML { get; set; }

        public static VgaGraph Load(string folder, XElement xml)
        {
            using (FileStream vgaHead = new FileStream(System.IO.Path.Combine(folder, xml.Element("VgaGraph").Attribute("VgaHead").Value), FileMode.Open))
            using (FileStream vgaGraphStream = new FileStream(System.IO.Path.Combine(folder, xml.Element("VgaGraph").Attribute("VgaGraph").Value), FileMode.Open))
            using (FileStream vgaDict = new FileStream(System.IO.Path.Combine(folder, xml.Element("VgaGraph").Attribute("VgaDict").Value), FileMode.Open))
                return new VgaGraph(vgaHead, vgaGraphStream, vgaDict, xml);
        }

        /// <summary>
        /// Wolfenstein 3-D bitmap fonts don't look right when represented 1:1 from what's in the file. Because of their having been intended to be displayed on VGA screen mode 13h, they need to be 20% taller.
        /// <para/>
        /// I am achieving this by storing them as textures in which each character is five times wider and six times taller than the VGAGRAPH says they should be.
        /// <para/>
        /// Thank God we live in the distant future where we have so much RAM that we can waste it on crap like that.
        /// </summary>
        public struct Font
        {
            public ushort RawHeight;
            public byte[] RawWidth;
            public byte[][] Character;

            public uint Height
            {
                get
                {
                    return (uint)RawHeight * 6;
                }
            }

            public uint Width(char character)
            {
                return Width((byte)character);
            }

            public uint Width(byte character)
            {
                return RawWidth[character] * (uint)5;
            }

            public Font(Stream stream)
            {
                using (BinaryReader binaryReader = new BinaryReader(stream))
                {
                    RawHeight = binaryReader.ReadUInt16();
                    ushort[] location = new ushort[256];
                    for (uint i = 0; i < location.Length; i++)
                        location[i] = binaryReader.ReadUInt16();
                    RawWidth = new byte[location.Length];
                    for (uint i = 0; i < RawWidth.Length; i++)
                        RawWidth[i] = binaryReader.ReadByte();
                    Character = new byte[RawWidth.Length][];
                    for (uint character = 0; character < Character.Length; character++)
                        if (RawWidth[character] > 0)
                        {
                            uint pixelLength = (uint)RawWidth[character] * RawHeight;
                            Character[character] = new byte[Width((byte)character) * Height * 4];
                            stream.Seek(location[character], 0);
                            for (uint pixel = 0; pixel < pixelLength; pixel++)
                                if (binaryReader.ReadByte() != 0)
                                {
                                    uint xStart = (pixel % RawWidth[character]) * 20,
                                        yStart = (pixel / RawWidth[character]) * 6;
                                    for (uint x = xStart; x < xStart + 20; x++)
                                        for (uint y = yStart; y < yStart + 6; y++)
                                            Character[character][y * Width((byte)character) * 4 + x] = 255;
                                }
                        }
                }
            }

            public byte[] Line(string input)
            {
                uint width = CalcWidth(input) * 4;
                byte[] bytes = new byte[width * Height];
                uint rowStart = 0;
                foreach (char c in input)
                {
                    for (int x = 0; x < Width(c) * 4; x++)
                        for (int y = 0; y < Height; y++)
                            bytes[y * width + rowStart + x] = Character[c][y * Width(c) * 4 + x];
                    rowStart += Width(c) * 4;
                }
                return bytes;
            }

            public uint CalcWidth(string input)
            {
                uint result = 0;
                foreach (char c in input)
                    result += Width(c);
                return result;
            }
        }

        public Font[] Fonts { get; set; }
        public byte[][] Pics { get; set; }
        public ushort[][] Sizes { get; set; }
        public uint[] Palette { get; set; }

        public VgaGraph(Stream vgaHead, Stream vgaGraph, Stream dictionary, XElement xml) : this(SplitFile(ParseHead(vgaHead), vgaGraph, Load16BitPairs(dictionary)), xml)
        { }

        public VgaGraph(byte[][] file, XElement xml)
        {
            Palette = VSwap.LoadPalette(xml);
            XML = xml.Element("VgaGraph");
            using (MemoryStream sizes = new MemoryStream(file[(uint)XML.Element("Sizes").Attribute("Chunk")]))
                Sizes = Load16BitPairs(sizes);
            uint startFont = (uint)XML.Element("Sizes").Attribute("StartFont");
            Fonts = new Font[(uint)XML.Element("Sizes").Attribute("NumFont")];
            for (uint i = 0; i < Fonts.Length; i++)
                using (MemoryStream font = new MemoryStream(file[startFont + i]))
                    Fonts[i] = new Font(font);
            uint startPics = (uint)XML.Element("Sizes").Attribute("StartPics");
            Pics = new byte[(uint)XML.Element("Sizes").Attribute("NumPics")][];
            for (uint i = 0; i < Pics.Length; i++)
                Pics[i] = VSwap.Index2ByteArray(Deplanify(file[startPics + i], Sizes[i][0]), Palette);
        }

        public static uint[] ParseHead(Stream stream)
        {
            uint[] head = new uint[stream.Length / 3];
            for (uint i = 0; i < head.Length; i++)
                head[i] = Read24Bits(stream);
            return head;
        }

        public static uint Read24Bits(Stream stream)
        {
            return (uint)(stream.ReadByte() | (stream.ReadByte() << 8) | (stream.ReadByte() << 16));
        }

        public static byte[][] SplitFile(uint[] head, Stream file, ushort[][] dictionary)
        {
            byte[][] split = new byte[head.Length - 1][];
            using (BinaryReader binaryReader = new BinaryReader(file))
                for (uint i = 0; i < split.Length; i++)
                {
                    uint size = head[i + 1] - head[i];
                    if (size > 0)
                    {
                        file.Seek(head[i], 0);
                        uint length = binaryReader.ReadUInt32();
                        binaryReader.Read(split[i] = new byte[size - 2], 0, split[i].Length);
                        split[i] = CAL_HuffExpand(split[i], dictionary, length);
                    }
                }
            return split;
        }

        public static byte[] Deplanify(byte[] input, ushort width)
        {
            return Deplanify(input, width, (ushort)(input.Length / width));
        }

        public static byte[] Deplanify(byte[] input, ushort width, ushort height)
        {
            byte[] bytes = new byte[input.Length];
            int linewidth = width / 4;
            for (int i = 0; i < bytes.Length; i++)
            {
                int plane = i / ((width * height) / 4),
                    sx = ((i % linewidth) * 4) + plane,
                    sy = ((i / linewidth) % height);
                bytes[sy * width + sx] = input[i];
            }
            return bytes;
        }

        /// <summary>
        /// Implementing Huffman decompression. http://www.shikadi.net/moddingwiki/Huffman_Compression#Huffman_implementation_in_ID_Software_games
        /// Translated from https://github.com/mozzwald/wolf4sdl/blob/master/id_ca.cpp#L214-L260
        /// </summary>
        /// <param name="dictionary">The Huffman dictionary is a ushort[255][2]</param>
        public static byte[] CAL_HuffExpand(byte[] source, ushort[][] dictionary, uint length = 0)
        {
            List<byte> dest = new List<byte>();
            ushort[] huffNode = dictionary[254];
            uint read = 0;
            ushort nodeVal;
            byte val = source[read++], mask = 1;
            while (read < source.Length && (length <= 0 || dest.Count < length))
            {
                nodeVal = huffNode[(val & mask) == 0 ? 0 : 1];
                if (mask == 0x80)
                {
                    val = source[read++];
                    mask = 1;
                }
                else
                    mask <<= 1;
                if (nodeVal < 256)
                { // 0-255 is a character, > is a pointer to a node
                    dest.Add((byte)nodeVal);
                    huffNode = dictionary[254];
                }
                else
                    huffNode = dictionary[nodeVal - 256];
            }
            return dest.ToArray();
        }

        public static ushort[][] Load16BitPairs(Stream stream)
        {
            ushort[][] dest = new ushort[stream.Length / 4][];
            using (BinaryReader binaryReader = new BinaryReader(stream))
                for (uint i = 0; i < dest.Length; i++)
                    dest[i] = new ushort[]
                    {
                        binaryReader.ReadUInt16(),
                        binaryReader.ReadUInt16()
                    };
            return dest;
        }
    }
}
