using Godot;
using NScumm.Audio.OPL.Woody;
using OPL;
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
            Texture = Assets.IsoSlantUp[wall],
            Position = new Vector2(offsetX, offsetY),
        });
        AddChild(new Sprite()
        {
            Texture = Assets.Floor,
            Position = new Vector2(offsetX + 64, offsetY + 96),
        });
        AddChild(new Sprite()
        {
            Texture = Assets.IsoSlantDown[wall + 1],
            Position = new Vector2(offsetX + 128, offsetY),
        });

        AddChild(new Sprite()
        {
            Texture = Assets.Textures[202],
            Position = new Vector2(offsetX + 68, offsetY + 32),
            Scale = new Vector2(2, 2),
        });
    }



    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
