using Godot;
using System.Linq;
using System.Xml.Linq;
using WOLF3D;

namespace WOLF2D.View
{
    public class MapWalls : Node2D
    {
        public MapWalls()
        {
            AddChild(Floors);
            AddChild(FarWalls);
            AddChild(NearWalls);
        }

        private Assets assets;
        public Assets Assets
        {
            get
            {
                return assets;
            }

            set
            {
                assets = value;
                Floors.TileSet = assets.FloorTileSet;
                FarWalls.TileSet = assets.FarWalls;
                NearWalls.TileSet = assets.NearWalls;
            }
        }

        private GameMap map;
        public GameMap Map
        {
            get
            {
                return map;
            }

            set
            {
                map = value;
                Floors.Clear();
                FarWalls.Clear();
                NearWalls.Clear();
                for (uint x = 0; x < map.Width; x++)
                    for (uint z = 0; z < map.Depth; z++)
                        if (!IsWall(x, z))
                        {
                            Floors.SetCell((int)x, (int)z, 0);
                            if (x > 0)
                            {
                                XElement wall = XWall(map.GetMapData(x - 1, z));
                                if (wall != null)
                                    FarWalls.SetCell((int)x - 1, (int)z, (int)wall.Attribute("Page"));
                            }
                        }
            }
        }

        public TileMap Floors = new TileMap()
        {
            Mode = TileMap.ModeEnum.Isometric,
            CellSize = new Vector2(254, 128),
            CellYSort = true,
            CellTileOrigin = TileMap.TileOrigin.BottomLeft,
        };

        public TileMap FarWalls = new TileMap()
        {
            Mode = TileMap.ModeEnum.Isometric,
            CellSize = new Vector2(254, 128),
            CellYSort = true,
            CellTileOrigin = TileMap.TileOrigin.BottomLeft,
        };

        public TileMap NearWalls = new TileMap()
        {
            Mode = TileMap.ModeEnum.Isometric,
            CellSize = new Vector2(254, 128),
            CellYSort = true,
            CellTileOrigin = TileMap.TileOrigin.BottomLeft,
            SelfModulate = new Color(1f, 1f, 1f, 0.5f),
        };

        public bool IsWall(uint x, uint z)
        {
            return IsWall(Map.GetMapData(x, z));
        }

        public bool IsWall(ushort cell)
        {
            return XWall(cell) != null;
        }

        public XElement XWall(ushort cell)
        {
            return (from e in Assets?.XML?.Element("VSwap")?.Element("Walls")?.Elements("Wall") ?? Enumerable.Empty<XElement>()
                    where (uint)e.Attribute("Number") == cell
                    select e).FirstOrDefault();
        }

        public bool IsDoor(uint x, uint z)
        {
            return IsDoor(Map.GetMapData(x, z));
        }

        public bool IsDoor(ushort cell)
        {
            return XDoor(cell) != null;
        }

        public XElement XDoor(ushort cell)
        {
            return (from e in Assets?.XML?.Element("VSwap")?.Element("Walls")?.Elements("Door") ?? Enumerable.Empty<XElement>()
                    where (uint)e.Attribute("Number") == cell
                    select e).FirstOrDefault();
        }
    }
}
