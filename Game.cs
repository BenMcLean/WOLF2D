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

        //for (int x = 0; x < renderer.Width; x++)
        //    for (int y = 0; y < renderer.Height; y++)
        //        renderer.DrawPixel(x, y, palette[16]);
    }



    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
