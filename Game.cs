using Godot;
using NScumm.Audio.OPL.Woody;
using OPL;
using System;
using WarpWriter.View.Color;
using WarpWriter.View.Render;
using WOLF3D;

public class Game : Node2D
{
    public static string Folder = "WOLF3D";
    public static Assets Assets;

    public override void _Ready()
    {
        DownloadShareware.Main(new string[] { Folder });
        Assets = new Assets(Folder);
        AddChild(Assets.OplPlayer = new OplPlayer()
        {
            Opl = new WoodyEmulatorOpl(NScumm.Core.Audio.OPL.OplType.Opl3)
        });

        AddChild(new Label()
        {
            Text = "Hello World!",
        });

        uint wallIndex = 18;

        //AddChild(new Sprite()
        //{
        //    Texture = Assets.Textures[wallIndex],
        //    GlobalPosition = new Vector2(200, 250),
        //    Scale = new Vector2(6, 6),
        //});

        uint[] palette = WarpWriterFriendlyPalette(Assets.VSwap.Palette);

        ByteArrayRenderer renderer = new ByteArrayRenderer()
        {
            Width = 512,
            Height = 256,
            ScaleX = 2,
            Color = new FlatVoxelColor()
            {
                Palette = palette,
            },
        };

        for (int x = 0; x < renderer.Width; x++)
            for (int y = 0; y < renderer.Height; y++)
                renderer.DrawPixel(x, y, palette[16]);

        byte[][] indexes = new byte[64][];
        for (int x = 0; x < indexes.Length; x++)
        {
            indexes[x] = new byte[indexes.Length];
            for (int y = 0; y < indexes[x].Length; y++)
                indexes[x][y] = WarpWriterFriendly(
                        Assets.VSwap.Indexes[wallIndex][(63 - y) * 64 + x]
                    );
        }

        IsoTile(indexes, palette, renderer);

        Godot.Image image = new Image();
        image.CreateFromData((int)renderer.Width, (int)renderer.Height, false, Image.Format.Rgba8, renderer.Bytes);
        ImageTexture it = new ImageTexture();
        it.CreateFromImage(image, 0);

        AddChild(new Sprite()
        {
            Texture = it,
            Position = new Vector2(700, 200),
            //Scale = new Vector2(6, 6),
        });
    }

    public static void IsoTile<T>(byte[][] indexes, uint[] palette, T renderer) where T : ITriangleRenderer<T>
    {
        //Cartesian to isometric:
        //isoX = cartX - cartY;
        //isoY = (cartX + cartY) / 2;
        int xOffset = indexes.Length + indexes[0].Length - 2;
        for (int x = 0; x < indexes.Length; x++)
            for (int y = 0; y < indexes[x].Length; y++)
                Diamond(
                    xOffset + (x - y) * 2,
                    (x + y) * 2,
                    palette[indexes[x][y]],
                    renderer
                    );
    }

    public static void Diamond<T>(int x, int y, uint color, T renderer) where T : ITriangleRenderer<T>
    {
        renderer.DrawLeftTriangle(x, y, color)
                .DrawRightTriangle(x + 2, y, color);
    }

    public static void IsoSlantUp<T>(byte[][] indexes, uint[] palette, T renderer) where T : IRectangleRenderer<T>
    {
        for (int x = 0; x < indexes.Length; x++)
            for (int y = 0; y < indexes[x].Length; y++)
                renderer.Rect(x * 2, x + y * 2, 1, 2, palette[indexes[x][y]])
                        .Rect(x * 2 + 1, x + y * 2 + 1, 1, 2, palette[indexes[x][y]]);
    }

    public static void IsoSlantDown<T>(byte[][] indexes, uint[] palette, T renderer) where T : IRectangleRenderer<T>
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
    }

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

    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
