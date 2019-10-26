using Godot;
using OPL;
using System.IO;
using System.Xml.Linq;
using WarpWriter.View.Color;
using WarpWriter.View.Render;
using WarpWriter.WarpWriter.View;

namespace WOLF3D
{
    /// <summary>
    /// Assets takes the bytes extracted from VSwap and creates the corresponding Godot objects for them to be used throughout the game.
    /// </summary>
    public class Assets
    {
        //Tom Hall's Doom Bible and also tweets from John Carmack state that the walls in Wolfenstein 3D were always eight feet thick. The wall textures are 64x64 pixels, which means that the ratio is 8 pixels per foot.
        //However, VR uses the metric system, where 1 game unit is 1 meter in real space. One foot equals 0.3048 meters.
        //Now unless I am a complete failure at basic math (quite possible) this means that to scale Wolfenstein 3D correctly in VR, one pixel must equal 0.0381 in game units, and a Wolfenstein 3D wall must be 2.4384 game units thick.
        public static readonly float PixelWidth = 0.0381f;
        public static readonly float WallWidth = 2.4384f;
        public static readonly float HalfWallWidth = 1.2192f;

        // However, Wolfenstein 3D ran in SVGA screen mode 13h, which has a 320x200 resolution in a 4:3 aspect ratio.
        // This means that the pixels are not square! They have a 1.2:1 aspect ratio.
        public static readonly Vector3 Scale = new Vector3(1f, 1.2f, 1f);
        public static readonly float PixelHeight = 0.04572f;
        public static readonly double WallHeight = 2.92608;

        public static readonly Vector3 BillboardLocal = new Vector3(WallWidth / -2f, 0f, 0f);

        public Assets(string folder, string file = "game.xml") : this(folder, LoadXML(folder, file))
        { }

        public Assets(string folder, XElement xml)
        {
            XML = xml;
            if (XML.Element("VSwap") != null)
                VSwap = VSwap.Load(folder, XML);
            if (XML.Element("Maps") != null)
                Maps = GameMap.Load(folder, XML);
            if (XML.Element("Audio") != null)
                AudioT = AudioT.Load(folder, XML);
            if (XML.Element("VgaGraph") != null)
                VgaGraph = VgaGraph.Load(folder, XML);

            // Creating floor texture
            uint[] palette = WarpWriterFriendlyPalette(VSwap.Palette);
            ByteArrayRenderer renderer = new ByteArrayRenderer()
            {
                Width = 254,
                Height = 128,
                Color = new FlatVoxelColor()
                {
                    Palette = palette,
                },
            };
            renderer.IsoTile(64, 64,
                palette[
                    WarpWriterFriendly(
                        (byte)(int)XML.Element("Maps").Attribute("FloorIndex")
                        )
                    ]
                );
            Godot.Image image = new Image();
            image.CreateFromData((int)renderer.Width, (int)renderer.Height, false, Image.Format.Rgba8, renderer.Bytes);
            Floor = new ImageTexture();
            Floor.CreateFromImage(image, 0);
        }

        public static XElement LoadXML(string folder, string file = "game.xml")
        {
            using (FileStream xmlStream = new FileStream(System.IO.Path.Combine(folder, file), FileMode.Open))
                return XElement.Load(xmlStream);
        }

        public XElement XML { get; set; }
        public GameMap[] Maps { get; set; }
        public OplPlayer OplPlayer { get; set; }
        public AudioT AudioT { get; set; }

        public VSwap VSwap
        {
            get
            {
                return vswap;
            }
            set
            {
                vswap = value;
                Textures = new ImageTexture[VSwap.Pages.Length];
                for (uint i = 0; i < Textures.Length; i++)
                    if (VSwap.Pages[i] != null)
                    {
                        Godot.Image image = new Image();
                        image.CreateFromData(64, 64, false, Image.Format.Rgba8, VSwap.Pages[i]);
                        Textures[i] = new ImageTexture();
                        Textures[i].CreateFromImage(image, 0);
                    }

                // Creating slopes
                uint[] palette = WarpWriterFriendlyPalette(VSwap.Palette);
                IVoxelColor color = new FlatVoxelColor()
                {
                    Palette = palette,
                };

                ByteArrayRenderer newSlopeRenderer()
                {
                    return new ByteArrayRenderer()
                    {
                        Width = 128,
                        Height = 192,
                        Color = color,
                    };
                }

                ByteArrayRenderer newDiamondRenderer()
                {
                    return new ByteArrayRenderer()
                    {
                        Width = 254,
                        Height = 128,
                        Color = color,
                    };
                }

                IsoSlantUp = new ImageTexture[VSwap.SpritePage];
                IsoSlantDown = new ImageTexture[VSwap.SpritePage];
                IsoTile = new ImageTexture[VSwap.SpritePage];
                FarWalls = new TileSet();
                NearWalls = new TileSet();
                for (uint wall = 0; wall < VSwap.SpritePage; wall++)
                    if (VSwap.Indexes[wall] != null)
                    {
                        byte[][] indexes = new byte[64][];
                        for (int x = 0; x < indexes.Length; x++)
                        {
                            indexes[x] = new byte[indexes.Length];
                            for (int y = 0; y < indexes[x].Length; y++)
                                indexes[x][y] = WarpWriterFriendly(
                                        VSwap.Indexes[wall][(63 - y) * 64 + x]
                                    );
                        }
                        ByteArrayRenderer renderer = newSlopeRenderer();
                        //for (int x = 0; x < renderer.Width; x++)
                        //    for (int y = 0; y < renderer.Height; y++)
                        //        renderer.DrawPixel(x, y, palette[16]);
                        renderer.IsoSlantUp(indexes, palette);
                        Godot.Image image = new Image();
                        image.CreateFromData((int)renderer.Width, (int)renderer.Height, false, Image.Format.Rgba8, renderer.Bytes);
                        IsoSlantUp[wall] = new ImageTexture();
                        IsoSlantUp[wall].CreateFromImage(image, 0);
                        renderer = newSlopeRenderer();
                        //for (int x = 0; x < renderer.Width; x++)
                        //    for (int y = 0; y < renderer.Height; y++)
                        //        renderer.DrawPixel(x, y, palette[16]);
                        renderer.IsoSlantDown(indexes, palette);
                        image = new Image();
                        image.CreateFromData((int)renderer.Width, (int)renderer.Height, false, Image.Format.Rgba8, renderer.Bytes);
                        IsoSlantDown[wall] = new ImageTexture();
                        IsoSlantDown[wall].CreateFromImage(image, 0);
                        renderer = newDiamondRenderer();
                        for (int x = 0; x < renderer.Width; x++)
                            for (int y = 0; y < renderer.Height; y++)
                                renderer.DrawPixel(x, y, palette[16]);
                        renderer.IsoTile(indexes, palette);
                        image = new Image();
                        image.CreateFromData((int)renderer.Width, (int)renderer.Height, false, Image.Format.Rgba8, renderer.Bytes);
                        IsoTile[wall] = new ImageTexture();
                        IsoTile[wall].CreateFromImage(image, 0);

                        FarWalls.CreateTile((int)wall);
                        FarWalls.TileSetTexture((int)wall, IsoSlantUp[(int)wall]);
                        FarWalls.TileSetTextureOffset((int)wall, IsoSlantUpFarWallOffset);
                        FarWalls.CreateTile((int)wall + VSwap.SpritePage);
                        FarWalls.TileSetTexture((int)wall + VSwap.SpritePage, IsoSlantDown[(int)wall]);
                        FarWalls.TileSetTextureOffset((int)wall + VSwap.SpritePage, IsoSlantDownFarWallOffset);

                        NearWalls.CreateTile((int)wall);
                        NearWalls.TileSetTexture((int)wall, IsoSlantUp[(int)wall]);
                        NearWalls.TileSetTextureOffset((int)wall, IsoSlantUpNearWallOffset);
                        NearWalls.CreateTile((int)wall + VSwap.SpritePage);
                        NearWalls.TileSetTexture((int)wall + VSwap.SpritePage, IsoSlantDown[(int)wall]);
                        NearWalls.TileSetTextureOffset((int)wall + VSwap.SpritePage, IsoSlantDownNearWallOffset);
                    }
            }
        }
        private VSwap vswap;

        private static readonly Vector2 IsoSlantUpFarWallOffset = new Vector2(128, -64);
        private static readonly Vector2 IsoSlantDownFarWallOffset = new Vector2(0, -64);
        private static readonly Vector2 IsoSlantUpNearWallOffset = new Vector2(0, -128);
        private static readonly Vector2 IsoSlantDownNearWallOffset = new Vector2(128, -128);

        public VgaGraph VgaGraph
        {
            get
            {
                return vgaGraph;
            }
            set
            {
                vgaGraph = value;
                Pics = new ImageTexture[VgaGraph.Pics.Length];
                for (uint i = 0; i < Pics.Length; i++)
                    if (VgaGraph.Pics[i] != null)
                    {
                        Godot.Image image = new Image();
                        image.CreateFromData(VgaGraph.Sizes[i][0], VgaGraph.Sizes[i][1], false, Image.Format.Rgba8, VgaGraph.Pics[i]);
                        Pics[i] = new ImageTexture();
                        Pics[i].CreateFromImage(image, 0); //(int)Texture.FlagsEnum.ConvertToLinear);
                    }
            }
        }
        private VgaGraph vgaGraph;

        public uint BackgroundColor { get; set; } = 255;
        public ImageTexture[] Textures;
        public ImageTexture[] Pics;
        public ImageTexture[] IsoTile;
        public ImageTexture[] IsoSlantUp;
        public ImageTexture[] IsoSlantDown;
        public ImageTexture Floor;
        public TileSet FarWalls;
        public TileSet NearWalls;

        public static uint[] WarpWriterFriendlyPalette(uint[] palette)
        {
            uint[] friendly = new uint[256];
            for (uint i = 0; i < 256; i++)
                friendly[WarpWriterFriendly((byte)i)] = ReverseBytes(palette[i]);
            return friendly;
        }

        public static byte WarpWriterFriendly(byte unfriendly)
        {
            return (byte)(unfriendly - 1);
        }

        public static uint ReverseBytes(uint value)
        {
            return (value & 0x000000FFU) << 24 | (value & 0x0000FF00U) << 8 |
                (value & 0x00FF0000U) >> 8 | (value & 0xFF000000U) >> 24;
        }
    }
}
