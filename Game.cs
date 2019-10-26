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

        VisualServer.SetDefaultClearColor(new Color(Assets.BackgroundColor));

        TileMap floors = new TileMap()
        {
            Mode = TileMap.ModeEnum.Isometric,
            CellSize = new Vector2(254, 128),
            CellYSort = true,
            CellTileOrigin = TileMap.TileOrigin.BottomLeft,
            TileSet = Assets.FloorTileSet,
        };
        TileMap farWalls = new TileMap()
        {
            Mode = TileMap.ModeEnum.Isometric,
            CellSize = new Vector2(254, 128),
            CellYSort = true,
            CellTileOrigin = TileMap.TileOrigin.BottomLeft,
            TileSet = Assets.FarWalls,
        };
        TileMap nearWalls = new TileMap()
        {
            Mode = TileMap.ModeEnum.Isometric,
            CellSize = new Vector2(254, 128),
            CellYSort = true,
            CellTileOrigin = TileMap.TileOrigin.BottomLeft,
            TileSet = Assets.NearWalls,
            SelfModulate = new Color(1f, 1f, 1f, 0.5f),
        };

        for (int i = 0; i < 10; i++)
        {
            farWalls.SetCell(0, -i - 1, Assets.VSwap.SpritePage + 1);
            floors.SetCell(0, -i - 1, 0);
            farWalls.SetCell(-i - 1, 0, 0);
            floors.SetCell(-i - 1, 0, 0);
            nearWalls.SetCell(0, i, Assets.VSwap.SpritePage + 1);
            floors.SetCell(0, i, 0);
            nearWalls.SetCell(i + 1, 0, 0);
            floors.SetCell(i + 1, 0, 0);
        }

        AddChild(floors);
        AddChild(farWalls);
        AddChild(nearWalls);
    }
}
