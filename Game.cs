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

        Vector2 isoSlantDownWallOffset = new Vector2(0, -64);
        Vector2 isoSlantUpWallOffset = new Vector2(-128, -64);

        TileSet tileSet = new TileSet();
        for (int i = 0; i < Assets.VSwap.SpritePage; i++)
        {
            if (Assets.IsoSlantUp[i] != null)
            {
                tileSet.CreateTile(i);
                tileSet.TileSetTexture(i, Assets.IsoSlantUp[i]);
                tileSet.TileSetTextureOffset(i, isoSlantUpWallOffset);
            }
            if (Assets.IsoSlantDown[i] != null)
            {
                tileSet.CreateTile(i + Assets.VSwap.SpritePage);
                tileSet.TileSetTexture(i + Assets.VSwap.SpritePage, Assets.IsoSlantDown[i]);
                tileSet.TileSetTextureOffset(i + Assets.VSwap.SpritePage, isoSlantDownWallOffset);
            }
        }
        int floorTile = Assets.VSwap.SpritePage * 2;
        tileSet.CreateTile(floorTile);
        tileSet.TileSetTexture(floorTile, Assets.Floor);

        TileMap tileMap = new TileMap()
        {
            Mode = TileMap.ModeEnum.Isometric,
            CellSize = new Vector2(254, 128),
            CellYSort = true,
            CellTileOrigin = TileMap.TileOrigin.BottomLeft,
            TileSet = tileSet,
            //CellHalfOffset = TileMap.HalfOffset.X,
        };

        for (int i = -10; i < 10; i++)
        {
            tileMap.SetCell(0, i, Assets.VSwap.SpritePage + 1);
            tileMap.SetCell(i, 0, 0);
        }

        AddChild(tileMap);
    }
}
