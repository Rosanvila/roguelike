namespace Roguelike;

public class Map
{
    public int Width { get; }
    public int Height { get; }
    public IReadOnlyList<Room> Rooms => _rooms;

    private readonly TileType[,] _tiles;
    private readonly List<Room> _rooms = new();

    public Map(int width, int height, int? seed = null)
    {
        Width = width;
        Height = height;
        _tiles = new TileType[height, width];

        Fill(TileType.Wall);

        var rng = seed.HasValue ? new Random(seed.Value) : new Random();
        GenerateRooms(rng, maxAttempts: 30, minSize: 4, maxSize: 9);
        ConnectRooms(rng);
    }

    private void Fill(TileType t)
    {
        for (int y = 0; y < Height; y++)
            for (int x = 0; x < Width; x++)
                _tiles[y, x] = t;
    }

    private void GenerateRooms(Random rng, int maxAttempts, int minSize, int maxSize)
    {
        for (int i = 0; i < maxAttempts; i++)
        {
            int w = rng.Next(minSize, maxSize + 1);
            int h = rng.Next(minSize, maxSize + 1);
            int x = rng.Next(1, Width - w - 1);
            int y = rng.Next(1, Height - h - 1);

            var candidate = new Room(x, y, w, h);

            if (_rooms.Any(r => r.Intersects(candidate)))
                continue;

            CarveRoom(candidate);
            _rooms.Add(candidate);
        }
    }

    private void CarveRoom(Room r)
    {
        for (int y = r.Y; y <= r.Bottom; y++)
            for (int x = r.X; x <= r.Right; x++)
                _tiles[y, x] = TileType.Floor;
    }

    private void ConnectRooms(Random rng)
    {
        for (int i = 1; i < _rooms.Count; i++)
        {
            var a = _rooms[i - 1];
            var b = _rooms[i];

            if (rng.Next(2) == 0)
            {
                CarveHorizontalCorridor(a.CenterX, b.CenterX, a.CenterY);
                CarveVerticalCorridor(a.CenterY, b.CenterY, b.CenterX);
            }
            else
            {
                CarveVerticalCorridor(a.CenterY, b.CenterY, a.CenterX);
                CarveHorizontalCorridor(a.CenterX, b.CenterX, b.CenterY);
            }
        }
    }

    private void CarveHorizontalCorridor(int x1, int x2, int y)
    {
        for (int x = Math.Min(x1, x2); x <= Math.Max(x1, x2); x++)
            _tiles[y, x] = TileType.Floor;
    }

    private void CarveVerticalCorridor(int y1, int y2, int x)
    {
        for (int y = Math.Min(y1, y2); y <= Math.Max(y1, y2); y++)
            _tiles[y, x] = TileType.Floor;
    }


    public TileType GetTile(int x, int y) => _tiles[y, x];

    public bool IsWalkable(int x, int y) =>
        x >= 0 && x < Width
        && y >= 0 && y < Height
        && _tiles[y, x] == TileType.Floor;
}
