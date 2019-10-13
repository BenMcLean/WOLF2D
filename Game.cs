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

        int wall = 0,
            offsetX = 500,
            offsetY = 200;


        AddChild(new Sprite()
        {
            Texture = Assets.IsoSlantUp[wall + 1],
            Position = new Vector2(offsetX, offsetY),
        });
        AddChild(new Sprite()
        {
            Texture = Assets.IsoTile[wall + 2],
            Position = new Vector2(offsetX + 64, offsetY + 96),
        });
        AddChild(new Sprite()
        {
            Texture = Assets.IsoSlantDown[wall],
            Position = new Vector2(offsetX + 128, offsetY),
        });
    }



    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
