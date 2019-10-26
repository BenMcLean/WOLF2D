using Godot;
using NScumm.Audio.OPL.Woody;
using OPL;
using WOLF2D.View;
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

        VisualServer.SetDefaultClearColor(new Color(Assets.BackgroundColor));

        AddChild(new MapWalls()
        {
            Assets = Assets,
            Map = Assets.Maps[0],
        });
    }
}
