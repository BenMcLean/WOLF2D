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

        AddChild(new Sprite()
        {
            Texture = Assets.Textures[0],
            GlobalPosition = new Vector2(200, 200),
        });
    }

    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
