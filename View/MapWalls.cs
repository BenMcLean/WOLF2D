using Godot;
using System.Linq;
using System.Xml.Linq;
using WOLF3D;

namespace WOLF2D.View
{
    public class MapWalls : YSort
    {
        public MapWalls()
        {
            AddChild(Floors);
            //Floors.AddChild(FarWalls);
            //FarWalls.AddChild(NearWalls);
        }

        public static Color partialTransparent = new Color(1f, 1f, 1f, 0.5f);

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
                //FarWalls.TileSet = assets.FarWalls;
                //NearWalls.TileSet = assets.NearWalls;
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
                //FarWalls.Clear();
                //NearWalls.Clear();
                for (uint x = 0; x < map.Width; x++)
                    for (uint z = 0; z < map.Depth; z++)
                        if (!IsWall(x, z))
                        {
                            Floors.SetCell((int)x, (int)z, 0);
                            // Adding near walls
                            if (x > 0)
                            {
                                XElement wall = XWall(map.GetMapData(x - 1, z));
                                if (wall != null)
                                    AddChild(new Sprite()
                                    {
                                        Texture = assets.IsoSlantUp[(uint)wall.Attribute("Page")],
                                        Position = new Vector2(X(x - 1, z), Y(x - 1, z)),
                                    });
                            }
                            if (z > 0)
                            {
                                XElement wall = XWall(map.GetMapData(x, z - 1));
                                if (wall != null)
                                    AddChild(new Sprite()
                                    {
                                        Texture = assets.IsoSlantDown[(int)wall.Attribute("DarkSide")],
                                        Position = new Vector2(X(x, z - 1) - 128, Y(x, z - 1)),
                                    });
                            }
                            // Adding far walls
                            if (x < map.Width)
                            {
                                XElement wall = XWall(map.GetMapData(x + 1, z));
                                if (wall != null)
                                    AddChild(new Sprite()
                                    {
                                        Texture = assets.IsoSlantDown[(uint)wall.Attribute("Page")],
                                        FlipH = true,
                                        Position = new Vector2(X(x, z), Y(x, z)),
                                        SelfModulate = partialTransparent,
                                    });
                            }
                            if (z < map.Width)
                            {
                                XElement wall = XWall(map.GetMapData(x, z + 1));
                                if (wall != null)
                                    AddChild(new Sprite()
                                    {
                                        Texture = assets.IsoSlantUp[(int)wall.Attribute("DarkSide")],
                                        FlipH = true,
                                        Position = new Vector2(X(x, z) - 128, Y(x, z)),
                                        SelfModulate = partialTransparent,
                                    });
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

        public static int X(uint x, uint y)
        {
            return ((int)x - (int)y) * 127 + 64;
        }

        public static int Y(uint x, uint y)
        {
            return ((int)x + (int)y) * 64 + 160;
        }

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
