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
        AddChild(new Sprite()
        {
            Texture = Assets.Textures[0],
            GlobalPosition = new Vector2(200, 200),
            Scale = new Vector2(6, 6),
        });


        ByteArrayRenderer renderer = new ByteArrayRenderer()
        {
            Width = 64,
            Height = 64,
            Color = new FlatVoxelColor()
            {
                Palette = WarpWriterFriendlyPalette(Assets.VSwap.Palette),
            },
        };
        uint wallIndex = 0;
        for (uint x = 0; x < 64; x++)
            for (uint y = 0; y < 64; y++)
                renderer.Rect((int)x, (int)y, 1, 1,
                    WarpWriterFriendly(
                    Assets.VSwap.Indexes[wallIndex][(63 - y) * 64 + x]
                    )
                    );

        Godot.Image image = new Image();
        image.CreateFromData((int)renderer.Width, (int)renderer.Height, false, Image.Format.Rgba8, renderer.Bytes);
        ImageTexture it = new ImageTexture();
        it.CreateFromImage(image, 0);

        AddChild(new Sprite()
        {
            Texture = it,
            GlobalPosition = new Vector2(600, 200),
            Scale = new Vector2(6, 6),
        });
    }

    public static uint[] WarpWriterFriendlyPalette(uint[] palette)
    {
        uint[] friendly = new uint[256];
        for (uint i = 0; i < 256; i++)
            friendly[i] = ReverseBytes(palette[WarpWriterFriendly((byte)i)]);
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
